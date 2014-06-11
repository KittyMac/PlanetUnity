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

public enum PlanetUnityButtonState {Normal, Highlighted};

public interface IPUButton {
	void updateButtonToState(PlanetUnityButtonState newState);
	void performTouchUp();
	void performTouchDown();
}

public class PlanetUnityButtonScript : MonoBehaviour {

	public IPUButton entity;

	private bool trackingMouse = false;
	private PlanetUnityButtonState btnState; 

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

		trackingMouse = true;
		btnState = PlanetUnityButtonState.Highlighted;
		entity.updateButtonToState (PlanetUnityButtonState.Highlighted);
	}

	public void OnMouseUp() {

		if (btnState == PlanetUnityButtonState.Highlighted) {
			entity.performTouchUp ();
		}
		trackingMouse = false;
		btnState = PlanetUnityButtonState.Normal;
		entity.updateButtonToState (PlanetUnityButtonState.Normal);
	}
}

public partial class PUImageButton : PUImageButtonBase, IPUButton {

	public PlanetUnityButtonState state = PlanetUnityButtonState.Normal;

	public void performTouchUp()
	{
		if (onTouchUpExists) {
			NotificationCenter.postNotification (scope (), this.onTouchUp);
		}
	}

	public void performTouchDown()
	{
		if (onTouchDownExists) {
			NotificationCenter.postNotification (scope (), this.onTouchDown);
		}
	}

	public void updateButtonToState(PlanetUnityButtonState newState)
	{
		state = newState;

		Texture tex = null;
		if(state == PlanetUnityButtonState.Normal)
		{
			tex = (Texture) Resources.Load (normalResourcePath);
		}
		if(state == PlanetUnityButtonState.Highlighted)
		{
			tex = (Texture) Resources.Load (highlightedResourcePath);
		}

		gameObject.renderer.material.mainTexture = tex;
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		var collider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		if(touchSizeExists)
			collider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		else
			collider.size = new Vector3(bounds.w, bounds.h, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;

		updateButtonToState (PlanetUnityButtonState.Normal);
	}

}
