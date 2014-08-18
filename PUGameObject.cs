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
using System.Reflection;
using System;
using System.Collections;
using System.Text;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class PUGameObject : PUGameObjectBase {

	static public int depthMaskCounter = 0;
	static public int stencilMaskCounter = 0;

	public BoxCollider gameCollider;

	public GameObject gameObject;

	private PUColor depthMask1;
	private PUColor depthMask2;

	public string XmlBounds()
	{
		if (bounds == null) {
			return "0,0,0,0";
		}

		// This method attempts to convert the current transform into a bounds string
		// suitable for using in the XML
		PUGameObject parentObj = parent as PUGameObject;
		if (parentObj != null && gameObject != null && gameObject.transform != null) {
			return string.Format ("{0},{1},{2},{3}",
				gameObject.transform.localPosition.x,
				(parentObj.bounds.h-gameObject.transform.localPosition.y)-bounds.h,
				bounds.w,
				bounds.h);
		}

		return string.Format ("{0},{1},{2},{3}",
			gameObject.transform.localPosition.x,
			(Screen.height-gameObject.transform.localPosition.y)-bounds.h,
			bounds.w,
			bounds.h);
	}

	public virtual bool captureMouse()
	{
		return true;
	}

	public virtual GameObject contentGameObject()
	{
		// overriden by subclasses if they want objects to go into a different gameobject
		return gameObject;
	}

	public virtual Vector3 maskOffset()
	{
		// Overridden by subclasses to manually offset the mask rectangle
		return Vector3.zero;
	}

	public virtual void UpdateGeometry()
	{
		// Overridden by subclasses to rejigger meshes / colliders after a bounds change
		if (bounds != null) {
			gameObject.transform.localPosition = new Vector3 (bounds.x, bounds.y, 0);
		}

		gaxb_loadComplete ();
	}

	public string fullShaderPath (string shaderPath)
	{
		if (shaderPath.StartsWith ("PlanetUnity/DepthMask")) {
			return shaderPath;
		}

		if (shaderPath.StartsWith ("PlanetUnity/") == false) {
			return shaderPath;
		}
		
		// Check to see if I am inside of something that is clipping...
		if (depthMaskCounter > 0) {
			return shaderPath + "/DepthMask";
		}

		if (stencilMaskCounter > 0) {
			return shaderPath + "/StencilMask";
		}

		return shaderPath + "/Normal";
	}

	public override void gaxb_unload()
	{
		base.gaxb_unload ();
	}

	public void gaxb_loadComplete()
	{

		#if UNITY_EDITOR
		PUGameObjectEditor editorScript = (PUGameObjectEditor)gameObject.AddComponent(typeof(PUGameObjectEditor));
		editorScript.entity = this;
		#endif

		if (clipDepth) {
			depthMaskCounter--;
		}

		if (clipDepth) {

			// Remove any existing depth masks
			GameObject dm1 = null;
			GameObject dm2 = null;
			foreach (Transform dm in contentGameObject().transform) {
				if (dm.gameObject.name.Equals ("Depth Mask 1")) {
					dm1 = dm.gameObject;
				}
				if (dm.name.Equals ("Depth Mask 2")) {
					dm2 = dm.gameObject;
				}
			}
			if (dm1 != null) {
				GameObject.Destroy (dm1);
			}
			if (dm2 != null) {
				GameObject.Destroy (dm2);
			}

			// We need to create a Color to render the DepthMask shader to do depth-based culling
			depthMask1 = new PUColor("PlanetUnity/DepthMask/Set", new cColor(0,0,0,1), null, new cVector2 (0, 0), bounds);
			depthMask1.SetTitle ("Depth Mask 1");
			depthMask1.loadIntoGameObject (contentGameObject());
			depthMask1.gameObject.transform.localPosition = maskOffset ();

			if (gameObject.renderer) {
				depthMask1.gameObject.renderer.material.renderQueue = gameObject.renderer.material.renderQueue - 1;
			} else if(gameObject.transform.childCount > 0){
				int minRenderQueue = 99999999;
				foreach (Transform child in contentGameObject().GetComponentsInChildren<Transform>(true)) {
					if (child.renderer != null &&
						child.renderer.material.renderQueue < minRenderQueue &&
						child.name.StartsWith("Depth Mask") == false) {
						minRenderQueue = child.renderer.material.renderQueue;
					}
				}
				depthMask1.gameObject.renderer.material.renderQueue = minRenderQueue - 1;
			}

			depthMask2 = new PUColor("PlanetUnity/DepthMask/Clear", new cColor(0,0,0,1), null, new cVector2 (0, 0), bounds);
			depthMask2.SetTitle ("Depth Mask 2");
			depthMask2.loadIntoGameObject (contentGameObject());
			depthMask2.gameObject.transform.localPosition = maskOffset ();

			// Get the renderQueue of the last child and make sure we render after that
			if (gameObject.transform.childCount > 0) {
				int maxRenderQueue = -1;
				foreach (Transform child in contentGameObject().GetComponentsInChildren<Transform>(true)) {
					if (child.renderer != null &&
						child.renderer.material.renderQueue > maxRenderQueue &&
						child.name.StartsWith("Depth Mask") == false) {
						maxRenderQueue = child.renderer.material.renderQueue;
					}
				}
				depthMask2.gameObject.renderer.material.renderQueue = maxRenderQueue + 1;
			}

			depthMask1.gameObject.layer = PlanetUnityOverride.puCameraLayer;
			depthMask2.gameObject.layer = PlanetUnityOverride.puCameraLayer;
		}

	}

	public void setParentGameObject(GameObject p)
	{
		gameObject.transform.parent = p.transform;
	}

	public void loadIntoGameObject(GameObject _parent)
	{
		depthMaskCounter = 0;
		stencilMaskCounter = 0;

		gaxb_load (null, null, null);

		Vector3 savedPos = gameObject.transform.localPosition;
		Vector3 savedScale = gameObject.transform.localScale;
		Quaternion savedRot = gameObject.transform.localRotation;

		gameObject.transform.parent = _parent.transform;

		gameObject.transform.localPosition = savedPos;
		gameObject.transform.localRotation = savedRot;
		gameObject.transform.localScale = savedScale;

		gameObject.layer = PlanetUnityOverride.puCameraLayer;

		if (gameObject.renderer != null) {
			gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
		}
	}

	public void loadIntoPUGameObject(PUGameObject _parent)
	{
		parent = _parent;
		_parent.children.Add (this);
		loadIntoGameObject (_parent.contentGameObject());
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (clipDepth) {
			depthMaskCounter++;
		}

		if (gameObject == null) {
			gameObject = new GameObject ("<GameObject />");

			if (titleExists) {
				gameObject.name = title;
			}
		}

		if (_parent is GameObject) {
			setParentGameObject (_parent as GameObject);
		} else if (_parent is PUGameObject) {
			PUGameObject parentEntity = (PUGameObject)_parent;

			setParentGameObject (parentEntity.gameObject);

			if (boundsExists) {
				bounds.y = (parentEntity.bounds.h - bounds.y) - bounds.h;
				gameObject.transform.localPosition = new Vector3 (bounds.x, bounds.y, 0.0f);
			}
		} else if (_parent == null) {
			gameObject.transform.localPosition = new Vector3 (bounds.x, bounds.y, 0.0f);
		}

		if (reader != null) {
			gameObject.layer = PlanetUnityOverride.puCameraLayer;
		}

		if (hidden) {
			gameObject.SetActive (false);
		}
	}

	public void unload(){
		GameObject.Destroy (gameObject);
		gameObject = null;
	}
}

#if UNITY_EDITOR

public class PUGameObjectEditor : MonoBehaviour
{
	public PUGameObject entity;
	public void CopyBoundsToClipboard()
	{
		UnityEngine.Debug.Log ("Bounds copied to clipboard");
		EditorGUIUtility.systemCopyBuffer = string.Format("@{0}_BOUNDS={1}\n", entity.title.ToUpper(), entity.XmlBounds());
	}
}

[CustomEditor(typeof(PUGameObjectEditor))]
public class PUGameObjectEditor2 : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PUGameObjectEditor myScript = (PUGameObjectEditor)target;
		if(GUILayout.Button("Copy Bounds To Clipboard"))
		{
			myScript.CopyBoundsToClipboard();
		}
	}
}
	
#endif