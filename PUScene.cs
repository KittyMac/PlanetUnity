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
using System.Reflection;
using System.Collections;
using System.Text;

public class PlanetUnityCameraObject : MonoBehaviour {

	public PUScene scene;
	public Camera camera;

	protected float currentAspectRatio;

	const float depth = 1024;

	float timeSinceLastFPSChange = 0;
	int newDesiredFPSRate = 0;

	public void Update() {
		HandleDynamicFPS ();

		if (Time.frameCount % 30 == 0)
		{
			GC.Collect();
		}
	}

	public void OnPreCull() {
		camera.ResetAspect ();
		camera.ResetWorldToCameraMatrix ();
		camera.ResetProjectionMatrix ();

		AdjustCamera ();
	}

	public void HandleDynamicFPS()
	{
		if (scene.fps > 0) {
			Application.targetFrameRate = scene.fps;
		} else {
			int newFPS = (int)PlanetUnityGameObject.desiredFPS;

			if (newDesiredFPSRate != newFPS) {
				newDesiredFPSRate = newFPS;
				timeSinceLastFPSChange = 0;
			}

			timeSinceLastFPSChange += Time.deltaTime;

			// When throttling down, we want to wait a long time before doing that
			// Otherwise, we want to speed up as fast as possible.
			if (timeSinceLastFPSChange > 2.0f && Application.targetFrameRate > newFPS) {
				Application.targetFrameRate = newFPS;
			} else if (Application.targetFrameRate < newFPS) {
				Application.targetFrameRate = newFPS;
			}

			PlanetUnityGameObject.desiredFPS = PlanetUnityOverride.minFPS;
		}
	}
		
	public void AdjustCamera() {
		// Force the scene to reload so we can easily test different screen resolutions
		if (camera.aspect != currentAspectRatio && currentAspectRatio != 0)
		{
			if (PlanetUnityOverride.orientationDidChange (scene)) {
				return;
			}
		}

		UpdateCameraForNewAspectRatio ();
	}

	public void UpdateCameraForNewAspectRatio()
	{
		camera.transform.position = new Vector3 (scene.bounds.w / 2, scene.bounds.h / 2, -depth);

		float screenW = camera.pixelRect.width;
		float screenH = camera.pixelRect.height;
		bool isWide = (screenW / screenH) > (scene.bounds.w / scene.bounds.h);
		float h;

		if (isWide) {
			h = scene.bounds.w / (screenW / screenH);
		} else {
			h = scene.bounds.h;
		}

		double radtheta;
		radtheta = 2.0 * Math.Atan2 (h * 0.5f, depth);
		camera.fieldOfView = (float)((180.0 * radtheta) / Math.PI);

		camera.clearFlags = CameraClearFlags.Depth;
		camera.farClipPlane = depth * 10;
		camera.orthographic = false;
		camera.depth = 100;
		camera.enabled = true;    

		camera.cullingMask = 0x70000000;
		camera.eventMask = 1 << PlanetUnityOverride.puEventLayer;

		currentAspectRatio = camera.aspect;

		Type planetOverride = Type.GetType("PlanetUnityOverride");
		if (planetOverride != null) {
			FieldInfo fInfo = planetOverride.GetField ("sceneTop");
			if (fInfo != null) {
				fInfo.SetValue (planetOverride, (scene.bounds.h - h) / 2);
			}
			fInfo = planetOverride.GetField ("sceneBottom");
			if (fInfo != null) {
				fInfo.SetValue (planetOverride, scene.bounds.h - ((scene.bounds.h - h) / 2));
			}
			fInfo = planetOverride.GetField ("sceneScale");
			if (fInfo != null) {
				fInfo.SetValue (planetOverride, h / scene.bounds.h);
			}

			fInfo = planetOverride.GetField ("sceneLeft");
			if (fInfo != null) {
				if (!isWide) {
					float screenScale = scene.bounds.h / screenH;
					fInfo.SetValue (planetOverride, (scene.bounds.w - (screenW * screenScale)) / 2);
				} else {
					fInfo.SetValue (planetOverride, 0);
				}
			}
			fInfo = planetOverride.GetField ("sceneRight");
			if (fInfo != null) {
				if (!isWide) {
					float screenScale = scene.bounds.h / screenH;
					float sceneLeft = (scene.bounds.w - (screenW * screenScale)) / 2;
					fInfo.SetValue (planetOverride, sceneLeft + (scene.bounds.w - sceneLeft * 2));
				} else {
					fInfo.SetValue (planetOverride, scene.bounds.w);
				}
			}
		}
	}
}
	
public class PlanetUnityEventMonitor : MonoBehaviour {

	public PUScene scene;
	public Camera camera;
	private bool mouseCancelled = false;

	public void PassMouseMethod(string methodName)
	{
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		LayerMask mask = PlanetUnityOverride.puCameraLayer;

		RaycastHit closeHit;
		RaycastHit[] hits = Physics.RaycastAll (ray, Mathf.Infinity, ~mask.value);
		Collider[] colliders = new Collider[hits.Length];
		bool localShouldCancelMouse = mouseCancelled;

		Physics.Raycast (ray, out closeHit, Mathf.Infinity, ~mask.value);

		for (int i = 0; i < hits.Length; i++){
			colliders [i] = hits [i].collider;
		}

		if (colliders.Length == 1 && colliders[0].gameObject == gameObject) {
			NotificationCenter.postNotification (scene.scope (), PlanetUnity.EVENTWITHNOCOLLIDER, NotificationCenter.Args ("event", methodName));
			return;
		}

		if (methodName == "OnMouseCancelled") {
			mouseCancelled = false;
			localShouldCancelMouse = false;
		}

		// Otherwise, we havd multiple things under me.
		// We need to figure out who should get the event.
		if (scene.performOnChildren (val => {
			PUGameObject oo = val as PUGameObject;

			if (oo != null) {
				if(oo.gameCollider != null){
					bool hasColliderBeenHit = (Array.IndexOf(colliders, oo.gameCollider) >= 0);

					// If this PlanetUnity object has a collider that has been hit, send it the event
					if(hasColliderBeenHit && oo.gameObject.activeInHierarchy) {
						oo.gameCollider.gameObject.SendMessage (methodName);
						if (oo.captureMouse ()) {
							return false;
						}
					}
				}else if(oo.children.Count == 0) {
					if(oo.gameObject != null){
					
						// We want to check any child game objects which might have been placed here dynamically.  If do, we bail and send PlanetUnity.EVENTWITHUNREGISTEREDCOLLIDER
						foreach(Collider collider in oo.gameObject.GetComponentsInChildren<Collider>(false)){
							if(collider == closeHit.collider){
								NotificationCenter.postNotification (scene.scope (), PlanetUnity.EVENTWITHUNREGISTEREDCOLLIDER, NotificationCenter.Args ("sender", collider, "event", methodName));
								return false;
							}
						}
					}
				}
			}

			if(localShouldCancelMouse)
				return false;

			return true;
		})) {
			// If we get here, no one "captured" the event. Run through colliders and send of notification
			// for unregistered colliders hit
			foreach (Collider collider in colliders) {
				if (collider.gameObject != gameObject) {
					NotificationCenter.postNotification (scene.scope (), PlanetUnity.EVENTWITHUNREGISTEREDCOLLIDER, NotificationCenter.Args ("sender", collider, "event", methodName));
				}
			}
		}


	}

	public void PassMouseMethod2(string methodName)
	{
		if (methodName == "OnMouseCancelled") {
			mouseCancelled = false;
		}

		scene.performOnChildren (val => {
			PUGameObject oo = val as PUGameObject;
			if (oo != null && oo.gameCollider != null && oo.gameObject.activeInHierarchy) {
				oo.gameCollider.gameObject.SendMessage(methodName);
			}

			return true;
		});


		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		LayerMask mask = PlanetUnityOverride.puCameraLayer;

		RaycastHit[] hits = Physics.RaycastAll (ray, Mathf.Infinity, ~mask.value);
		if (hits.Length == 1 && hits[0].collider.gameObject == gameObject) {
			NotificationCenter.postNotification (scene.scope (), PlanetUnity.EVENTWITHNOCOLLIDER, NotificationCenter.Args ("event", methodName));
			return;
		}

		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject != gameObject) {
				NotificationCenter.postNotification (scene.scope (), PlanetUnity.EVENTWITHUNREGISTEREDCOLLIDER, NotificationCenter.Args ("sender", hit.collider, "event", methodName));
			}
		}
	}


	public void OnMouseEnter ()
	{
		mouseCancelled = false;
		PassMouseMethod ("OnMouseEnter");
		mouseCancelled = false;
	}

	public void OnMouseExit ()
	{
		mouseCancelled = false;
		PassMouseMethod ("OnMouseExit");
		mouseCancelled = false;
	}

	public void OnMouseDown ()
	{
		mouseCancelled = false;
		PassMouseMethod ("OnMouseDown");
		mouseCancelled = false;
	}

	public void OnMouseUp ()
	{
		mouseCancelled = false;
		PassMouseMethod2 ("OnMouseUp");
		mouseCancelled = false;
	}

	public void OnMouseMoved()
	{
		mouseCancelled = false;
		PassMouseMethod ("OnMouseMoved");
		mouseCancelled = false;
	}
		
	public void OnMouseDrag()
	{
		mouseCancelled = false;
		PassMouseMethod2 ("OnMouseDrag");
		mouseCancelled = false;
	}

	public void OnMouseCancelled()
	{
		mouseCancelled = true;
		PassMouseMethod2 ("OnMouseCancelled");
	}
}


public partial class PUScene : PUSceneBase {

	public PlanetUnityCameraObject cameraObject = null;
	public PlanetUnityEventMonitor eventMonitor = null;
	private GameObject eventsObject = null;

	public Camera sceneCamera;

	public void gaxb_loadComplete()
	{
		foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true)) {
			if (trans.gameObject.layer != PlanetUnityOverride.puEventLayer) {
				trans.gameObject.layer = PlanetUnityOverride.puCameraLayer;
			}
		}

		NotificationCenter.addObserver (this, "PlanetUnityCancelMouse", this, (args, name) => {
			eventMonitor.OnMouseCancelled();
		});

		base.gaxb_loadComplete ();
	}

	public override void gaxb_unload()
	{
		base.gaxb_unload ();

		GameObject.Destroy(cameraObject);
		cameraObject = null;

		GameObject.Destroy(eventMonitor);
		eventMonitor = null;
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		UpdateCollider ();
	}

	private void UpdateCollider() {
		gameCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);
		gameCollider.center = new Vector3 (bounds.w/2.0f, bounds.h/2.0f, 0.0f);
	}

	private void CreateSceneCamera() {
		sceneCamera = (Camera)Camera.Instantiate (
			Camera.main, 
			new Vector3 (0, 0, 0), 
			Quaternion.FromToRotation (new Vector3 (0, 0, 0), new Vector3 (0, 0, 1)));

		sceneCamera.name = "PlanetUnityCamera";
		sceneCamera.transform.parent = gameObject.transform;
		sceneCamera.eventMask = 0;

		Camera.main.cullingMask = 0x0FFFFFFF;

		foreach (Component comp in sceneCamera.GetComponents(typeof(Component))) {
			if (comp is Transform)
				continue;
			if (comp is Camera)
				continue;
			GameObject.Destroy (comp);
		}
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (adjustCamera) {
			CreateSceneCamera ();

			cameraObject = (PlanetUnityCameraObject)sceneCamera.gameObject.AddComponent (typeof(PlanetUnityCameraObject));
			cameraObject.scene = this;
			cameraObject.camera = sceneCamera;
			cameraObject.AdjustCamera ();
		}

		eventsObject = new GameObject ("PlanetUnityEvents");
		eventsObject.layer = PlanetUnityOverride.puEventLayer;
		eventsObject.transform.parent = gameObject.transform;

		eventMonitor = (PlanetUnityEventMonitor)eventsObject.AddComponent (typeof(PlanetUnityEventMonitor));
		eventMonitor.scene = this;

		if (sceneCamera != null) {
			eventMonitor.camera = sceneCamera;
		}

		// We use a collider to capture all of our own touches and manually handle touch events
		gameCollider = (BoxCollider) eventsObject.AddComponent(typeof(BoxCollider));
		UpdateCollider ();

	}

	public override bool isScopeContainer()
	{
		return true;
	}

	public void UpdateCameraForNewAspectRatio()
	{
		if (cameraObject != null) {
			cameraObject.UpdateCameraForNewAspectRatio ();
		}
	}

	public void ResetRenderQueues()
	{
		clearRenderQueue ();

		this.performOnChildrenForward (v => {
			PUGameObject oo = v as PUGameObject;
			if(oo != null){
				if(oo.gameObject.renderer != null) {
					oo.gameObject.renderer.material.renderQueue = oo.scope().getRenderQueue() + oo.renderQueueOffset;
				}
				foreach(Transform t in oo.gameObject.transform){
					if(t.renderer != null){
						t.renderer.material.renderQueue = oo.scope().getRenderQueue() + oo.renderQueueOffset;
					}
				}
			}
			return true;
		});
	}

	public bool TestUserTouch (Vector3 touchPos)
	{
		// If we have a custom camera, we need to ray cast against it
		if (cameraObject != null) {
			Ray ray = cameraObject.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			LayerMask mask = PlanetUnityOverride.puCameraLayer;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity, ~mask.value)) {

				return true;
			}
		}
		return false;
	}


}
