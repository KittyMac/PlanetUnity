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

public class PlanetUnityCameraObject : MonoBehaviour {
	public PlanetUnity_Scene scene;
	public new Camera camera;
	protected float currentAspectRatio;

	public void AdjustCamera() {
		const float depth = 1024;

		if (camera == null) {
			var original = GameObject.FindWithTag ("MainCamera");

			camera = (Camera)Camera.Instantiate (
				original.camera, 
				new Vector3 (0, 0, 0), 
				Quaternion.FromToRotation (new Vector3 (0, 0, 0), new Vector3 (0, 0, 1)));

			camera.name = "PlanetUnityCamera";
			camera.transform.parent = scene.gameObject.transform;

			original.camera.cullingMask &= 0x7FFFFFFF;


			foreach (Component comp in camera.GetComponents(typeof(Component))) {
				if (comp is Transform)
					continue;
				if (comp is Camera)
					continue;
				Destroy (comp);
			}
		} else {
			// ************************ EDITOR ONLY *******************************
			#if UNITY_EDITOR
			// Force the scene to reload so we can easily test different screen resolutions
			if (camera.aspect != currentAspectRatio)
			{
				NotificationCenter.postNotification (null, "PlanetUnityReloadScene");
			}
			#endif
		}

		if (camera.aspect == currentAspectRatio)
			return;

		camera.transform.position = new Vector3 (scene.bounds.w / 2, scene.bounds.h / 2, -depth);

		float screenW = camera.pixelRect.width;
		float screenH = camera.pixelRect.height;

		float h = scene.bounds.w / (screenW / screenH);

		double radtheta;
		radtheta = 2.0 * Math.Atan2 (h * 0.5f, depth);
		camera.fieldOfView = (float)((180.0 * radtheta) / Math.PI);

		camera.clearFlags = CameraClearFlags.Depth;
		camera.farClipPlane = depth * 10;
		camera.orthographic = false;
		camera.depth = 100;
		camera.enabled = true;    
		camera.cullingMask = 1 << 31;

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
		}
	}

	void LateUpdate () {
		AdjustCamera ();
	}

}

public class PlanetUnity_Scene : PlanetUnity_SceneBase {

	public void gaxb_loadComplete()
	{
		foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true)) {
			trans.gameObject.layer = 31;
		}
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if (adjustCamera) {
			PlanetUnityCameraObject cameraObject = (PlanetUnityCameraObject)gameObject.AddComponent (typeof(PlanetUnityCameraObject));
			cameraObject.scene = this;
			cameraObject.AdjustCamera ();
		}
	}

	public new bool isScopeContainer()
	{
		return true;
	}
}
