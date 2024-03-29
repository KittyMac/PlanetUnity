/* Copyright (c) 2012 Small Planet Digital, LLC
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
 * (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using System.Xml;
using System.Collections;
using System.Security.Cryptography;

public partial class PUScroll : PUScrollBase
{
	public GameObject contentObject;
	private PlanetUnityScrollScript script;

	public override bool captureMouse()
	{
		return false;
	}

	public override GameObject contentGameObject()
	{
		return contentObject;
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		UpdateCollider ();

		// we need to copy our localPos to the contentObject
		contentObject.transform.localPosition = gameObject.transform.localPosition;
	}

	private void UpdateCollider() {
		BoxCollider boxCollider = gameCollider as BoxCollider;
		boxCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);
		boxCollider.center = new Vector3 (bounds.w/2.0f, bounds.h/2.0f, 0.0f);
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		// This is a little tricky; we want to insert our contentObject between our gameObject and its parent
		contentObject = new GameObject ("ScrollContent");
		contentObject.layer = PlanetUnityOverride.puCameraLayer;

		Vector3 savedPos = gameObject.transform.localPosition;

		contentObject.transform.parent = gameObject.transform.parent;
		gameObject.transform.parent = contentObject.transform;

		contentObject.transform.localPosition = savedPos;
		gameObject.transform.localPosition = Vector3.zero;

		gameCollider = (BoxCollider) contentObject.AddComponent(typeof(BoxCollider));
		UpdateCollider ();

		script = (PlanetUnityScrollScript) contentObject.AddComponent(typeof(PlanetUnityScrollScript));
		script.entity = this;

		if(scrollDirection == PlanetUnity.ScrollDirection.both)
			script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Both;
		if(scrollDirection == PlanetUnity.ScrollDirection.vertical)
			script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Vertical;
		if(scrollDirection == PlanetUnity.ScrollDirection.horizontal)
			script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Horizontal;

		script.scrollEnabled = scrollEnabled;
		script.pagingEnabled = pagingEnabled;
		script.bounces = bounces;
		script.directionalLockEnabled = directionalLockEnabled;
	}

	public void CalculateContentSize()
	{
		// if contentSize does not exist, run through planet children and calculate a content size
		float minX = 999999, maxX = -999999;
		float minY = 999999, maxY = -999999;

		foreach (PUGameObject go in children) {
			if (go.bounds.x < minX)
				minX = go.bounds.x;
			if (go.bounds.y < minY)
				minY = go.bounds.y;

			if ((go.bounds.x+go.bounds.w) > maxX)
				maxX = (go.bounds.x+go.bounds.w);
			if ((go.bounds.y+go.bounds.h) > maxY)
				maxY = (go.bounds.y+go.bounds.h);
		}

		contentSize = new cVector2 (maxX - minX, maxY - minY);

		CalculateScrollDirection ();
	}

	public void CalculateScrollDirection()
	{
		if (scrollDirectionExists == false) {

			script.scrollEnabled = false;
			script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Both;

			// If we don't specify a scroll direction, check out content size and use the size which exceeds our bounds
			if (contentSize.x > bounds.w && contentSize.y > bounds.h) {
				script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Both;
				script.scrollEnabled = true;
			} else if (contentSize.x > bounds.w) {
				script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Horizontal;
				script.scrollEnabled = true;
			} else if (contentSize.y > bounds.h) {
				script.scrollDirection = PlanetUnityScrollScript.PlanetScrollDirection.Vertical;
				script.scrollEnabled = true;
			}
		}
	}

	public void gaxb_loadComplete()
	{
		if ((int)contentSize.x == 0 || (int)contentSize.y == 0) {
			CalculateContentSize ();
		}

		CalculateScrollDirection ();

		base.gaxb_loadComplete ();
	}

	public void ResetScroll()
	{
		script.ResetScroll ();
	}
}

public class PlanetUnityScrollScript : MonoBehaviour
{
	public PUScroll entity;

	private const float kEdgeBounceDuration = 0.5f;
	private const float kEdgeBounceEaseTimePercent = 0.2f;
	private const float kBungeeStretchCoefficient = 0.55f;
	private const float kMinCancelTouchesVelocity = 25;
	private const float kScrollDuration = 2.75f;
	private const float kPageVelocity = 200.0f;
	private const float kSwipeDistancePerVelocity = 0.3f;
	private const float kBounceDistancePerVelocity = 0.03f;
	private const float kAnimationDuration = 0.5f;
	private const float kMinScrollSpeed = 1.0f;

	public enum PlanetScrollDirection
	{
		Vertical = (1 << 0),
		Horizontal = (1 << 1),
		Both = (Vertical|Horizontal),
	};

	public enum PlanetScrollState
	{
		Idle,
		UserDragging,
		Decelerating,
		Animating,
	};

	enum PlanetScrollDeceleratingState
	{
		Idle,
		Bouncing,
		ReturningFromEdge,
		Scrolling,
	};
		
	private bool isTracking = false;
	private bool userTouching = false;

	public PlanetScrollDirection scrollDirection = PlanetScrollDirection.Both;
	public PlanetScrollDirection scrollLockDirection;
	private PlanetScrollState scrollState;
	private PlanetScrollDeceleratingState horizontalDecelerationState;
	private PlanetScrollDeceleratingState verticalDecelerationState;

	private float scrollStateTime;
	private float horizontalDecelerationTime;
	private float verticalDecelerationTime;
	private float minScale;
	private float maxScale;
	public bool scrollEnabled;
	public bool pagingEnabled;
	public bool bounces = true;
	public bool directionalLockEnabled = false;
	private bool directionalLockIsSet = false;


	//touch interaction
	private float userTouchScale;
	private Vector2 velocity;
	private Vector2 scroll;
	private Vector2 absScroll;
	private Vector2 animStartVelocity;
	private Vector2 animStartScroll;
	private Vector2 animEndScroll;
	private Vector2 touchEdgeOffset;
	private bool shouldCancelTouches;
	private long touchTimestamp;

	private Vector2 previousMousePosition = PlanetUnityGameObject.MousePosition();

	public void OnMouseCancelled()
	{

	}

	public void OnMouseEnter ()
	{
		//Debug.Log ("OnMouseEnter");
	}

	public void OnMouseExit ()
	{
		//Debug.Log ("OnMouseExit");
	}

	public void OnMouseDown ()
	{
		if (scrollEnabled == false)
			return;

		if (userTouching == false) {
			userTouching = true;

			shouldCancelTouches = false;

			previousMousePosition = PlanetUnityGameObject.MousePosition();

			//we have some touches that will stop this scroll view in it's tracks
			velocity.x = 0;
			velocity.y = 0;

			absScroll.x = 0;
			absScroll.y = 0;

			scrollLockDirection = PlanetScrollDirection.Both;

			//check if the view was rubber banding when we touched down
			if (scroll.x > 0) {
				touchEdgeOffset.x = scroll.x;
			} else {
				float minScroll = calcMinScrollX();
				if(scroll.x < minScroll)
					touchEdgeOffset.x = scroll.x - minScroll;
				else
					touchEdgeOffset.x = 0;
			}

			if (scroll.y < 0) {
				touchEdgeOffset.y = scroll.y;
			} else {
				float minScroll = calcMaxScrollY();
				if(scroll.y > minScroll)
					touchEdgeOffset.y = scroll.y - minScroll;
				else
					touchEdgeOffset.y = 0;
			}
				
			//update state
			setScrollState(PlanetScrollState.UserDragging);

		}
	}

	public void OnMouseDrag()
	{

	}

	public void OnMouseUp ()
	{
		if (scrollEnabled == false)
			return;

		if (userTouching) {
		
			userTouching = false;

			//this will reset directional lock only when paging is not enabled
			if(!pagingEnabled)
				directionalLockIsSet = false;

			absScroll.x += Mathf.Abs (velocity.x);
			absScroll.y += Mathf.Abs (velocity.y);

			if (absScroll.x >= kMinCancelTouchesVelocity || absScroll.y >= kMinCancelTouchesVelocity) {
				shouldCancelTouches = true;
			}

			if (shouldCancelTouches) {
				NotificationCenter.postNotification (entity.scope (), "PlanetUnityCancelMouse");
			}

			//check if we flicked the scroll view
			if(Mathf.Abs(velocity.x) < kMinCancelTouchesVelocity)
				velocity.x = 0;
			if(Mathf.Abs(velocity.y) < kMinCancelTouchesVelocity)
				velocity.y = 0;

			//we do some weird shifting around of the scroll position to correctly calculate the bungee effect
			//recover the value from the bungee effect here to avoid any position snapping
			scroll = entity.gameObject.transform.localPosition;

			if(pagingEnabled)
			{
				//we're paging, animate if we're not already correctly positioned
				calcPagingTarget();

				//always switch states to ensure delegates get certain callbacks
				setScrollState(PlanetScrollState.Animating);

				//if we're already lined up, stop the animation state immediately
				if(!shouldBeAnimating())
				{
					//set the position to to line up exactly with the current page
					scroll = animEndScroll;
					//entity.gameObject.transform.localPosition = scroll;

					setScrollState(PlanetScrollState.Idle);
				}
			}
			else
			{
				bool isPastHorizontalEdge = scroll.x > 0.0f || scroll.x < calcMinScrollX();
				bool isPastVerticalEdge = scroll.y < 0.0f || scroll.y > calcMaxScrollY();
				bool isMoving = Mathf.Abs(velocity.x) > kMinScrollSpeed || Mathf.Abs(velocity.y) > kMinScrollSpeed;

				if(isMoving || isPastHorizontalEdge || isPastVerticalEdge)
				{
					//switch to decelerating
					setScrollState(PlanetScrollState.Decelerating);

					//we should scroll, calculate where our scroll should end
					if(isPastHorizontalEdge)
					{
						//animate back to the edge instead of scrolling
						horizontalDecelerationTime = 0.0f;
						horizontalDecelerationState = PlanetScrollDeceleratingState.ReturningFromEdge;
						animEndScroll.x = bounceEdgeX(scroll.x);
					}
					else
						animEndScroll.x = scroll.x + kSwipeDistancePerVelocity*velocity.x;

					if(isPastVerticalEdge)
					{
						//animate back to the edge instead of scrolling
						verticalDecelerationTime = 0.0f;
						verticalDecelerationState = PlanetScrollDeceleratingState.ReturningFromEdge;
						animEndScroll.y = bounceEdgeY(scroll.y);
					}
					else
						animEndScroll.y = scroll.y + kSwipeDistancePerVelocity*velocity.y;
				}
				else
				{
					//we're not moving or past the edge of the scroll, just idle
					setScrollState(PlanetScrollState.Idle);
				}
			}
		}
	}

	public void OnMouseMoved() {

		if (scrollEnabled == false)
			return;

		if(userTouching)
		{
			//get the overall translation
			Vector2 userTouchPosition = PlanetUnityGameObject.MousePosition();
			Vector2 avgPrevTouchLoc = previousMousePosition;


			//calculate the touch velocity
			Vector2 dLoc = new Vector2(userTouchPosition.x-avgPrevTouchLoc.x, userTouchPosition.y-avgPrevTouchLoc.y);
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Horizontal) != 0)
				velocity.x = dLoc.x / Time.deltaTime;
			else
				velocity.x = 0.0f;
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Vertical) != 0)
				velocity.y = dLoc.y / Time.deltaTime;
			else
				velocity.y = 0.0f;
				
			previousMousePosition = userTouchPosition;

			absScroll.x += Mathf.Abs (velocity.x);
			absScroll.y += Mathf.Abs (velocity.y);

			if (absScroll.x >= kMinCancelTouchesVelocity || absScroll.y >= kMinCancelTouchesVelocity) {
				shouldCancelTouches = true;
			}

			if (shouldCancelTouches) {
				NotificationCenter.postNotification (entity.scope (), "PlanetUnityCancelMouse");
			}

			//we might need to cancel touches on inner nodes when we start a scroll, which is expensive. avoid this if we can by not cancelling when the user touches, but doesn't actually scroll
			if(Mathf.Abs(velocity.x) < kMinCancelTouchesVelocity && Mathf.Abs(velocity.y) < kMinCancelTouchesVelocity)
				return;

			if(directionalLockEnabled && !directionalLockIsSet)
			{
				if(Mathf.Abs(dLoc.x) > Mathf.Abs(dLoc.y))
				{
					scrollLockDirection = PlanetScrollDirection.Horizontal;
					velocity.y = 0.0f;
				}
				else
				{
					scrollLockDirection = PlanetScrollDirection.Vertical;
					velocity.x = 0.0f;
				}
				directionalLockIsSet = true;
			}

			//update the scroll
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Horizontal) != 0)
				scroll.x += dLoc.x;
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Vertical) != 0)
				scroll.y += dLoc.y;

			// TODO: what to do for this?  I dunno
			//cancel the touch for any targeted delegates (PlanetX normally wouldn't use any of these)
			//CCDirector::sharedDirector()->getTouchDispatcher()->cancelTouches(relevantTouches, this);


			/*
			//cancel touches for PlanetX stuff
			if(pcEntity)
			{
				//find the touch node
				PlanetCore_ObservableObject* scopeObj = [pcEntity scope];
				CCPlanetTouchNode* touchNode = NULL;
				while(scopeObj)
				{
					if([scopeObj isKindOfClass:[PlanetCore_Scene class]] && [(PlanetCore_Scene*)scopeObj touchNode])
					{
						touchNode = [(PlanetCore_Scene*)scopeObj touchNode];
						break;
					}

					scopeObj = [[scopeObj parent] scope];
				}

				if(scopeObj)
				{
					//we have our PlanetX touch node, cancel the children of the scroll view
					for(int i = 0; i < innerNode->getChildrenCount(); i++)
					{
						touchNode->cancelTouchesForNode((CCNode*)innerNode->getChildren()->objectAtIndex(i));
					}
				}
			}*/
		}
	}

	// tweens

	private float easeOutCubic(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	private float easeOutQuad(float start, float end, float value) {
		end -= start;
		return -end * value * (value - 2) + start;
	}

	private float easeOutQuint(float start, float end, float value) {
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}

	// helpers

	float bungee(float stretchDist, float contentSize)
	{
		return (1.0f - (1.0f / ((stretchDist * kBungeeStretchCoefficient / contentSize) + 1.0f))) * contentSize;
	}

	int calcPage(float scrollAmount, float visibleSize, float contentSize, float scrollVelocity)
	{
		//calc the page
		float pageScrollAmount = -scrollAmount/visibleSize;
		int pageIndex = (int)Mathf.Round(pageScrollAmount);
		pageScrollAmount -= pageIndex;

		//if we're paging, we never touch the velociy, we just constantly check it to see if the user did a flick
		if(scrollVelocity >= kPageVelocity && pageScrollAmount < 0.0f)
			pageIndex--;
		else if(scrollVelocity <= -kPageVelocity && pageScrollAmount > 0.0f)
			pageIndex++;

		//cap the page number to an acceptable value
		if(pageIndex < 0)
			return 0;
		int maxIndex = (int)(contentSize/visibleSize)-1;
		if(pageIndex > maxIndex)
			return maxIndex;

		return pageIndex;
	}


	bool shouldBeAnimating()
	{
		float remainingPageDistX = (scroll.x - animEndScroll.x);
		float remainingPageDistY = (scroll.y - animEndScroll.y);
		return (Mathf.Abs(remainingPageDistX) > kMinScrollSpeed || Mathf.Abs(remainingPageDistY) > kMinScrollSpeed);
	}

	void calcPagingTarget()
	{
		if (((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Horizontal) != 0) {
			animEndScroll.x = -entity.bounds.w * calcPage (scroll.x, entity.bounds.w, entity.contentSize.x, velocity.x);
		}
		if (((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Vertical) != 0) {
			animEndScroll.y = entity.bounds.h * calcPage (-scroll.y, entity.bounds.h, entity.contentSize.y, -velocity.y);
		}
	}

	float calcMinScrollX()
	{
		if(entity.contentSize.x > entity.bounds.w)
			return entity.bounds.w - entity.contentSize.x;
		return 0.0f;
	}

	float calcMinScrollY()
	{
		if(entity.contentSize.y > entity.bounds.h)
			return entity.bounds.h - entity.contentSize.y;
		return 0.0f;
	}

	float calcMaxScrollX()
	{
		if(entity.contentSize.x > entity.bounds.w)
			return entity.contentSize.x-entity.bounds.w;
		return 0.0f;
	}

	float calcMaxScrollY()
	{
		if(entity.contentSize.y > entity.bounds.h)
			return entity.contentSize.y-entity.bounds.h;
		return 0.0f;
	}

	float bounceEdgeX(float scrollAmount)
	{
		if(scrollAmount > 0.0f)
			return 0.0f;
		else
			return calcMinScrollX();
	}

	float bounceEdgeY(float scrollAmount)
	{
		if(scrollAmount < 0.0f)
			return 0.0f;
		else
			return calcMaxScrollY();
	}

	void setScrollState(PlanetScrollState newState)
	{
		//ignore redundant changes
		if(scrollState != newState)
		{
			if(newState == PlanetScrollState.UserDragging)
			{
				// TODO: call scrollViewWillBeginDragging on delegate
			}
			else if(newState == PlanetScrollState.Decelerating)
			{
				// TODO: call scrollViewWillBeginDecelerating on delegate
			}
			else if(newState == PlanetScrollState.Animating)
			{
				// TODO: call scrollViewWillBeginAnimating on delegate
			}
			if(scrollState == PlanetScrollState.UserDragging)
			{
				// TODO: call scrollViewWillEndDragging on delegate
			}

			if(newState == PlanetScrollState.Animating || newState == PlanetScrollState.Decelerating)
			{
				animStartScroll = entity.gameObject.transform.localPosition;
				animStartVelocity = velocity;
				horizontalDecelerationState = PlanetScrollDeceleratingState.Scrolling;
				verticalDecelerationState = PlanetScrollDeceleratingState.Scrolling;
			}

			//this will reset directional lock when paging is enabled
			if(newState == PlanetScrollState.Idle && userTouching == false)
				directionalLockIsSet = false;

			PlanetScrollState oldState = scrollState;
			scrollState = newState;
			scrollStateTime = 0.0f;

			if(oldState == PlanetScrollState.UserDragging)
			{
				// TODO: call scrollViewDidEndDragging on delegate
			}
			else if(oldState == PlanetScrollState.Decelerating)
			{
				// TODO: call scrollViewDidEndDecelerating on delegate
			}
			else if(oldState == PlanetScrollState.Animating)
			{
				// TODO: call scrollViewDidEndAnimating on delegate
			}
		}
	}



	public void Update()
	{
		OnMouseMoved ();

		float dt = Time.deltaTime;

		scrollStateTime += dt;
		Vector2 assignScroll = scroll;
		bool updateScroll = false;

		switch(scrollState)
		{
		case PlanetScrollState.Decelerating:

			//update the horizontal deceleration
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Horizontal) != 0)
			{
				if(horizontalDecelerationState == PlanetScrollDeceleratingState.ReturningFromEdge)
				{
					//we're returning from being scrolled past the edge
					horizontalDecelerationTime += dt;
					assignScroll.x = easeOutCubic(animStartScroll.x, animEndScroll.x, horizontalDecelerationTime/kAnimationDuration);

					//are we done animating?
					if(horizontalDecelerationTime >= kAnimationDuration)
					{
						velocity.x = 0.0f;
						assignScroll.x = animEndScroll.x;
						horizontalDecelerationState = PlanetScrollDeceleratingState.Idle;
					}
				}
				else
				{
					//we were not scrolled past the edge, scroll normally
					if(horizontalDecelerationState == PlanetScrollDeceleratingState.Scrolling)
					{
						//we're scrolling, and have not yet hit the edge
						float deceleratingPercent = scrollStateTime/kScrollDuration;
						assignScroll.x = easeOutQuint(animStartScroll.x, animEndScroll.x, deceleratingPercent);
						velocity.x = easeOutQuint(animStartVelocity.x, 0.0f, deceleratingPercent);

						//check if we've hit the edge
						if(assignScroll.x > 0.0f || assignScroll.x < calcMinScrollX())
						{
							if(bounces)
							{
								horizontalDecelerationState = PlanetScrollDeceleratingState.Bouncing;
								horizontalDecelerationTime = 0.0f;
							}
							else
							{
								velocity.x = 0.0f;
								if(assignScroll.x > 0.0f)
									assignScroll.x = 0.0f;
								else if(assignScroll.x < calcMinScrollX())
									assignScroll.x = calcMinScrollX();
								horizontalDecelerationState = PlanetScrollDeceleratingState.Idle;
							}
						}
						else if(scrollStateTime > kScrollDuration)
						{
							velocity.x = 0.0f;
							assignScroll.x = animEndScroll.x;
							horizontalDecelerationState = PlanetScrollDeceleratingState.Idle;
						}
					}
					if(horizontalDecelerationState == PlanetScrollDeceleratingState.Bouncing)
					{
						horizontalDecelerationTime += dt;

						//which edge are we bouncing from?
						float edge = bounceEdgeX(assignScroll.x);

						//are we done bouncing?
						if(horizontalDecelerationTime >= kEdgeBounceDuration)
						{
							assignScroll.x = edge;
							velocity.x = 0.0f;
							horizontalDecelerationState = PlanetScrollDeceleratingState.Idle;
						}
						else
						{
							//assign the bounce position
							float bounceDistPastEdge = edge + velocity.x*kBounceDistancePerVelocity;
							if(horizontalDecelerationTime < kEdgeBounceEaseTimePercent*kEdgeBounceDuration)
							{
								assignScroll.x = easeOutQuad(edge, bounceDistPastEdge, horizontalDecelerationTime / (kEdgeBounceEaseTimePercent*kEdgeBounceDuration));
							}
							else
							{
								assignScroll.x = easeOutQuad(bounceDistPastEdge, edge, (horizontalDecelerationTime - kEdgeBounceEaseTimePercent*kEdgeBounceDuration) / ((1.0f-kEdgeBounceEaseTimePercent)*kEdgeBounceDuration));
							}
						}
					}
				}
			}

			//update the vertical deceleration
			if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Vertical) != 0)
			{
				if(verticalDecelerationState == PlanetScrollDeceleratingState.ReturningFromEdge)
				{
					//we're returning from being scrolled past the edge
					verticalDecelerationTime += dt;
					assignScroll.y = easeOutCubic(animStartScroll.y, animEndScroll.y, verticalDecelerationTime/kAnimationDuration);

					//are we done animating?
					if(verticalDecelerationTime >= kAnimationDuration)
					{
						velocity.y = 0.0f;
						assignScroll.y = animEndScroll.y;
						verticalDecelerationState = PlanetScrollDeceleratingState.Idle;
					}
				}
				else
				{
					//we were not scrolled past the edge, scroll normally
					if(verticalDecelerationState == PlanetScrollDeceleratingState.Scrolling)
					{
						//we're scrolling, and have not yet hit the edge
						float deceleratingPercent = scrollStateTime/kScrollDuration;
						assignScroll.y = easeOutQuint(animStartScroll.y, animEndScroll.y, deceleratingPercent);
						velocity.y = easeOutQuint(animStartVelocity.y, 0.0f, deceleratingPercent);

						//check if we've hit the edge
						if(assignScroll.y < 0.0f || assignScroll.y > calcMaxScrollY())
						{
							if(bounces)
							{
								verticalDecelerationState = PlanetScrollDeceleratingState.Bouncing;
								verticalDecelerationTime = 0.0f;
							}
							else
							{
								velocity.y = 0.0f;
								if(assignScroll.y < 0.0f)
									assignScroll.y = 0.0f;
								else if(assignScroll.y > calcMaxScrollY())
									assignScroll.y = calcMaxScrollY();
								verticalDecelerationState = PlanetScrollDeceleratingState.Idle;
							}
						}
						else if(scrollStateTime > kScrollDuration)
						{
							velocity.y = 0.0f;
							assignScroll.y = animEndScroll.y;
							verticalDecelerationState = PlanetScrollDeceleratingState.Idle;
						}
					}
					if(verticalDecelerationState == PlanetScrollDeceleratingState.Bouncing)
					{
						verticalDecelerationTime += dt;

						//which edge are we bouncing from?
						float edge = bounceEdgeY(assignScroll.y);

						//are we done bouncing?
						if(verticalDecelerationTime >= kEdgeBounceDuration)
						{
							assignScroll.y = edge;
							velocity.y = 0.0f;
							verticalDecelerationState = PlanetScrollDeceleratingState.Idle;
						}
						else
						{
							//assign the bounce position
							float bounceDistPastEdge = edge + velocity.y*kBounceDistancePerVelocity;
							if(verticalDecelerationTime < kEdgeBounceEaseTimePercent*kEdgeBounceDuration)
							{
								assignScroll.y = easeOutQuad(edge, bounceDistPastEdge, verticalDecelerationTime / (kEdgeBounceEaseTimePercent*kEdgeBounceDuration));
							}
							else
							{
								assignScroll.y = easeOutQuad(bounceDistPastEdge, edge, (verticalDecelerationTime - kEdgeBounceEaseTimePercent*kEdgeBounceDuration) / ((1.0f-kEdgeBounceEaseTimePercent)*kEdgeBounceDuration));
							}
						}
					}
				}
			}

			//check if we're done decelerating
			if(horizontalDecelerationState == PlanetScrollDeceleratingState.Idle &&
				verticalDecelerationState == PlanetScrollDeceleratingState.Idle)
			{
				setScrollState(PlanetScrollState.Idle);
			}

			updateScroll = true;

			break;
		case PlanetScrollState.UserDragging:
			{
				//account for touch offset
				assignScroll.x += touchEdgeOffset.x;
				assignScroll.y += touchEdgeOffset.y;

				//do bungee effect horizontally
				if(assignScroll.x > 0)
				{
					if(bounces)
						assignScroll.x = bungee(assignScroll.x, entity.bounds.w);
					else
					{
						assignScroll.x = 0.0f;
						velocity.x = 0.0f;
					}
				}
				else
				{
					float minScroll = calcMinScrollX();
					if(assignScroll.x < minScroll)
					{
						if(bounces)
							assignScroll.x = entity.bounds.w - bungee((minScroll - assignScroll.x), entity.bounds.w) - entity.contentSize.x;
						else
						{
							assignScroll.x = minScroll;
							velocity.x = 0.0f;
						}
					}
				}

				//do bungee effect vertically
				if(assignScroll.y < 0)
				{
					if (bounces) {
						assignScroll.y = bungee (assignScroll.y, -entity.bounds.h);
					} else {
						assignScroll.y = 0.0f;
						velocity.y = 0.0f;
					}
				}
				else
				{
					float minScroll = calcMaxScrollY();
					if(assignScroll.y > minScroll)
					{
						if(bounces)
							assignScroll.y = bungee((assignScroll.y-minScroll), entity.bounds.h) + entity.contentSize.y - entity.bounds.h;
						else
						{
							assignScroll.y = minScroll;
							velocity.y = 0.0f;
						}
					}
				}
			}
			break;
		case PlanetScrollState.Animating:
			{
				//animate horizontally
				if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Horizontal) != 0)
				{
					//animate towards our desired scroll position
					assignScroll.x = easeOutCubic(animStartScroll.x, animEndScroll.x, scrollStateTime/kAnimationDuration);
				}

				//animate vertically
				if(((int)scrollDirection & (int)scrollLockDirection & (int)PlanetScrollDirection.Vertical) != 0)
				{
					//animate towards our desired scroll position
					assignScroll.y = easeOutCubic(animStartScroll.y, animEndScroll.y, scrollStateTime/kAnimationDuration);
				}

				//ready to switch states?
				if(scrollStateTime >= kAnimationDuration)
				{
					assignScroll = animEndScroll;
					velocity.x = velocity.y = 0.0f;
					setScrollState(PlanetScrollState.Idle);
				}

				//we want the position that we calculated to persist
				updateScroll = true;
			}
			break;
		default:
			break;
		}

		//update the scroll position
		internalScroll(assignScroll, updateScroll);
	}

	void internalScroll(Vector2 newScroll, bool updateStoredScroll)
	{
		//update the scroll position
		Vector2 prevScroll = entity.gameObject.transform.localPosition;
		if(prevScroll.x != newScroll.x || prevScroll.y != newScroll.y)
		{
			entity.gameObject.transform.localPosition = new Vector2 (newScroll.x, newScroll.y);

			// When actively scrolling we want the best frame rate possible
			PlanetUnityGameObject.RequestFPS (PlanetUnityOverride.maxFPS);

			// TODO: call paging changes to the delegate

			// TODO: call scroll view did scroll to the delegate

			//make this change permanent?
			if(updateStoredScroll)
				scroll = newScroll;
		}
	}

	public void ResetScroll()
	{
		entity.gameObject.transform.localPosition = Vector2.zero;
		scroll = Vector2.zero;
	}

}