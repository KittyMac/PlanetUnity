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
using System;
using System.Collections;

public enum PlanetUnityButtonState {Normal, Highlighted, Undefined};

public interface IPUButton {
	void updateButtonToState(PlanetUnityButtonState newState);
	void performTouchUp(bool isLongPress);
	void performTouchDown();
}

public class PlanetUnityButtonScript : MonoBehaviour {

	public IPUButton entity;

	private bool trackingMouse = false;
	private PlanetUnityButtonState btnState; 
	private DateTime mouseDownTime;
	private Vector3 mouseDownPos;

	public void OnMouseCancelled() {
		trackingMouse = false;
		btnState = PlanetUnityButtonState.Normal;
		entity.updateButtonToState (PlanetUnityButtonState.Normal);
	}

	public void OnMouseEnter() {
		if (trackingMouse) {
			btnState = PlanetUnityButtonState.Highlighted;
			entity.updateButtonToState (btnState);
		}
	}

	public void OnMouseExit() {
		if (trackingMouse) {
			btnState = PlanetUnityButtonState.Normal;
			entity.updateButtonToState (PlanetUnityButtonState.Normal);
		}
	}

	public void OnMouseDown() {
		entity.performTouchDown ();

		mouseDownTime = DateTime.Now;
		mouseDownPos = Input.mousePosition;

		trackingMouse = true;
		btnState = PlanetUnityButtonState.Highlighted;
		entity.updateButtonToState (PlanetUnityButtonState.Highlighted);

		NotificationCenter.postNotification (null, PlanetUnity.BUTTONTOUCHDOWN, NotificationCenter.Args("sender", this));
	}

	public void OnMouseDrag()
	{
		if (trackingMouse) {
			Vector3 newMousePos = Input.mousePosition;
			float diffInSeconds = (float)((DateTime.Now - mouseDownTime).TotalSeconds);
			if (Vector3.Distance (newMousePos, mouseDownPos) > 25.0f) {
				mouseDownTime = DateTime.Now;
			} else if(diffInSeconds > 1.0f) {
				OnMouseUp ();
			}
		}
	}

	public void OnMouseUp() {
		bool shouldCallTouchUp = (trackingMouse && btnState == PlanetUnityButtonState.Highlighted);

		trackingMouse = false;
		btnState = PlanetUnityButtonState.Normal;
		entity.updateButtonToState (PlanetUnityButtonState.Normal);

		float diffInSeconds = (float)((DateTime.Now - mouseDownTime).TotalSeconds);

		if (shouldCallTouchUp) {
			entity.performTouchUp ((diffInSeconds > 1.0f));
		}

		NotificationCenter.postNotification (null, PlanetUnity.BUTTONTOUCHUP, NotificationCenter.Args("sender", this));
	}
}

public partial class PUImageButton : PUImageButtonBase, IPUButton {

	public PlanetUnityButtonState state = PlanetUnityButtonState.Undefined;
	private Color savedColor = Color.white;

	public void performTouchUp(bool isLongPress)
	{
		if (onTouchUpExists) {
			NotificationCenter.postNotification (scope (), this.onTouchUp, NotificationCenter.Args("sender", this, "isLongPress", isLongPress));
		}
	}

	public void performTouchDown()
	{
		if (onTouchDownExists) {
			NotificationCenter.postNotification (scope (), this.onTouchDown, NotificationCenter.Args("sender", this));
		}
	}

	public void updateButtonToState(PlanetUnityButtonState newState)
	{
		if (state == newState) {
			return;
		}

		state = newState;

		Texture tex = null;
		if(state == PlanetUnityButtonState.Normal)
		{
			tex = PlanetUnityResourceCache.GetTexture (normalResourcePath);
			gameObject.renderer.material.mainTexture = tex;

			if (touchColorExists) {
				if (savedColor != null) {
					gameObject.renderer.material.color = savedColor;
				} else if (colorExists) {
					gameObject.renderer.material.color = new Color (color.r, color.g, color.b, color.a);
				} else {
					gameObject.renderer.material.color = Color.white;
				}
			} else {
				gameObject.renderer.material.color = Color.white;
			}
		}
		if(state == PlanetUnityButtonState.Highlighted)
		{
			if (highlightedResourcePathExists) {
				tex = PlanetUnityResourceCache.GetTexture (highlightedResourcePath);
				gameObject.renderer.material.mainTexture = tex;
			}
			if(touchColorExists) {
				savedColor = gameObject.renderer.material.color;
				gameObject.renderer.material.color = new Color(touchColor.r, touchColor.g, touchColor.b, touchColor.a);
			}
		}
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		BoxCollider boxCollider = gameCollider as BoxCollider;
		if(touchSizeExists)
			boxCollider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		else
			boxCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (colorExists) {
			savedColor = new Color (color.r, color.g, color.b, color.a);
		}

		gameCollider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		BoxCollider boxCollider = gameCollider as BoxCollider;
		if(touchSizeExists)
			boxCollider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		else
			boxCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;

		updateButtonToState (PlanetUnityButtonState.Normal);
	}

}
