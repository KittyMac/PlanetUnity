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
using System.IO;

public class PlanetUnityCameraObject : MonoBehaviour {

	public PUScene scene;
	public Camera camera;

	protected float currentAspectRatio;

	float timeSinceLastFPSChange = 0;
	int newDesiredFPSRate = 0;

	public void AdjustCamera() {
		const float depth = 1024;

		// TODO: put in a timer before we drop to a lower FPS from a higher FPS

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

			PlanetUnityGameObject.desiredFPS = 10;
		}

		if (camera == null) {
			var original = GameObject.FindWithTag ("MainCamera");

			camera = (Camera)Camera.Instantiate (
				original.camera, 
				new Vector3 (0, 0, 0), 
				Quaternion.FromToRotation (new Vector3 (0, 0, 0), new Vector3 (0, 0, 1)));

			camera.name = "PlanetUnityCamera";
			camera.transform.parent = scene.gameObject.transform;
			camera.eventMask = 0;

			original.camera.cullingMask = 0x0FFFFFFF;


			foreach (Component comp in camera.GetComponents(typeof(Component))) {
				if (comp is Transform)
					continue;
				if (comp is Camera)
					continue;
				Destroy (comp);
			}
		} else {
			// Force the scene to reload so we can easily test different screen resolutions
			if (camera.aspect != currentAspectRatio)
			{
				NotificationCenter.postNotification (null, "PlanetUnityReloadScene");
			}
		}

		if (camera.aspect == currentAspectRatio)
			return;

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

	void Awake() {

	}

	void Update () {
		AdjustCamera ();
	}
}


public class PlanetUnityEventMonitor : MonoBehaviour {

	public PUScene scene;
	public Camera camera;

	public void PassMouseMethod(string methodName)
	{
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		LayerMask mask = PlanetUnityOverride.puCameraLayer;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, ~mask.value)) {
			Collider[] colliders = Physics.OverlapSphere (hit.point, 2.0f, ~mask.value);

			// Fast path, just pass the call...
			if (colliders.Length == 1) {
				if (collider.gameObject != gameObject) {
					collider.gameObject.BroadcastMessage (methodName);
				}
			} else {
				// Otherwise, we havd multiple things under me.
				// We need to figure out who should get the event.
				scene.performOnChildren (val => {
					PUGameObject oo = val as PUGameObject;
					if (oo != null && oo.gameCollider != null && oo.gameObject.activeInHierarchy) {

						if (Array.IndexOf (colliders, oo.gameCollider) >= 0) {
							oo.gameCollider.gameObject.SendMessage (methodName);

							if(oo.captureMouse()) {
								return false;
							}
						}
					}
					return true;
				});
			}
		}
	}

	public void PassMouseMethod2(string methodName)
	{
		scene.performOnChildren (val => {
			PUGameObject oo = val as PUGameObject;
			if (oo != null && oo.gameCollider != null && oo.gameObject.activeInHierarchy) {
				oo.gameCollider.gameObject.SendMessage(methodName);
			}
			return true;
		});
	}


	public void OnMouseEnter ()
	{
		PassMouseMethod ("OnMouseEnter");
	}

	public void OnMouseExit ()
	{
		PassMouseMethod ("OnMouseExit");
	}

	public void OnMouseDown ()
	{
		PassMouseMethod ("OnMouseDown");
	}

	public void OnMouseUp ()
	{
		PassMouseMethod2 ("OnMouseUp");
	}

	public void OnMouseMoved()
	{
		PassMouseMethod ("OnMouseMoved");
	}

	public void OnMouseCancelled()
	{
		PassMouseMethod ("OnMouseCancelled");
	}
}


public partial class PUScene : PUSceneBase {

	private PlanetUnityCameraObject cameraObject = null;
	private PlanetUnityEventMonitor eventMonitor = null;
	private GameObject eventsObject = null;

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

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (adjustCamera) {
			cameraObject = (PlanetUnityCameraObject)gameObject.AddComponent (typeof(PlanetUnityCameraObject));
			cameraObject.scene = this;
			cameraObject.AdjustCamera ();
		}

		eventsObject = new GameObject ("PlanetUnityEvents");
		eventsObject.layer = PlanetUnityOverride.puEventLayer;
		eventsObject.transform.parent = gameObject.transform;

		eventMonitor = (PlanetUnityEventMonitor)eventsObject.AddComponent (typeof(PlanetUnityEventMonitor));
		eventMonitor.scene = this;
		eventMonitor.camera = cameraObject.camera;

		// We use a collider to capture all of our own touches and manually handle touch events
		var collider = (BoxCollider) eventsObject.AddComponent(typeof(BoxCollider));
		collider.size = new Vector3(bounds.w, bounds.h, 1.0f);
		collider.center = new Vector3 (bounds.w/2.0f, bounds.h/2.0f, 0.0f);


	}

	public override bool isScopeContainer()
	{
		return true;
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
