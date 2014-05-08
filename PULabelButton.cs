
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

using System.Xml;
using UnityEngine;


public class PULabelButton : PULabelButtonBase, IPUButton {
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

		if(state == PlanetUnityButtonState.Normal)
		{
			text.renderer.material.color = new Color (textColor.r, textColor.g, textColor.b, textColor.a);
		}
		if(state == PlanetUnityButtonState.Highlighted)
		{
			text.renderer.material.color = new Color (1.0f, 0.0f, 0.0f, 1.0f);
		}
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);
		/*
		text.VerticalAnchor = CCText.VerticalAnchorMode.Middle;

		gameObject.transform.localPosition += new Vector3(0, bounds.h/2, 0.0f);
		gameObject.transform.localScale = new Vector3 (1.0f/pxScale, 1.0f/pxScale, 1.0f);
		*/

		float pxScale = ccfont.pixelScale;

		// Re-use the button code from PUButton
		var collider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		if(touchSizeExists)
			collider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w)*pxScale, (touchSize.y != 0 ? touchSize.y : bounds.h)*pxScale, 1.0f);
		else
			collider.size = new Vector3(bounds.w*pxScale, bounds.h*pxScale, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;
	}
}
