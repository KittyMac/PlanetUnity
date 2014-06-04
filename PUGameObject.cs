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

public partial class PUGameObject : PUGameObjectBase {

	static public int depthMaskCounter = 0;
	static public int stencilMaskCounter = 0;

	public GameObject gameObject;

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

	public string fullShaderPath (string shaderPath)
	{
		if (shaderPath.StartsWith ("PlanetUnity/DepthMask")) {
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

	public new void gaxb_unload()
	{
		base.gaxb_unload ();
	}

	public void gaxb_loadComplete()
	{
		if (clipDepth) {
			depthMaskCounter--;
		}

		if (clipDepth) {
			// We need to create a Color to render the DepthMask shader to do depth-based culling
			PUColor depthMask1 = new PUColor("PlanetUnity/DepthMask/Set", new cColor(0,0,0,1), new cVector2 (0, 0), bounds);
			depthMask1.SetTitle ("Depth Mask 1");
			depthMask1.loadIntoPUGameObject (this);
			depthMask1.gameObject.transform.localPosition += maskOffset ();

			if (gameObject.renderer) {
				depthMask1.gameObject.renderer.material.renderQueue = gameObject.renderer.material.renderQueue - 1;
			} else if(gameObject.transform.childCount > 0){
				depthMask1.gameObject.renderer.material.renderQueue = gameObject.transform.GetChild(0).renderer.material.renderQueue - 1;
			}

			PUColor depthMask2 = new PUColor("PlanetUnity/DepthMask/Clear", new cColor(0,0,0,1), new cVector2 (0, 0), bounds);
			depthMask2.SetTitle ("Depth Mask 2");
			depthMask2.loadIntoPUGameObject (this);
			depthMask2.gameObject.transform.localPosition += maskOffset ();

			// Get the renderQueue of the last child and make sure we render after that
			Transform lastChild = gameObject.transform.GetChild (gameObject.transform.childCount - 1);
			depthMask2.gameObject.renderer.material.renderQueue = lastChild.renderer.material.renderQueue+1;
		}
	}

	public void setParentGameObject(GameObject p)
	{
		gameObject.transform.parent = p.transform;
	}

	public void loadIntoGameObject(GameObject _parent)
	{
		object Null = null;

		MethodInfo mInfo;
		mInfo = this.GetType().GetMethod("gaxb_load");
		if(mInfo != null) { mInfo.Invoke (this, new[] { Null, Null }); }

		gameObject.transform.parent = _parent.transform;

		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	public void loadIntoPUGameObject(PUGameObject _parent)
	{
		parent = _parent;
		loadIntoGameObject (_parent.contentGameObject());
		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if (clipDepth) {
			depthMaskCounter++;
		}

		if (gameObject == null) {
			gameObject = new GameObject ("<Entity />");

			if (titleExists) {
				gameObject.name = title;
			}
		}

		if (_parent is GameObject) {
			setParentGameObject (_parent as GameObject);
		}
		else if (_parent is PUGameObject) {
			PUGameObject parentEntity = (PUGameObject)_parent;

			setParentGameObject (parentEntity.gameObject);

			if (boundsExists) {
				bounds.y = (parentEntity.bounds.h - bounds.y) - bounds.h;
				gameObject.transform.localPosition = new Vector3 (bounds.x, bounds.y, 0.0f);
			}
		}

		if (reader != null) {
			gameObject.layer = 31;
		}

		if (hidden) {
			gameObject.SetActive (false);
		}
	}
}
