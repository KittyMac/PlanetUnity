
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
using System.Collections;


public partial class PUColorButton : PUColorButtonBase, IPUButton {
	public PlanetUnityButtonState state = PlanetUnityButtonState.Normal;
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
		state = newState;

		if(state == PlanetUnityButtonState.Normal)
		{
			if (touchColorExists) {
				if (savedColor != null) {
					gameObject.renderer.material.color = savedColor;
				} else if (colorExists) {
					gameObject.renderer.material.color = new Color (color.r, color.g, color.b, color.a);
				} else {
					gameObject.renderer.material.color = Color.white;
				}
			} else {
				gameObject.renderer.material.color = savedColor;
			}
		}
		if(state == PlanetUnityButtonState.Highlighted)
		{
			if(touchColorExists) {
				savedColor = gameObject.renderer.material.color;
				gameObject.renderer.material.color = new Color(touchColor.r, touchColor.g, touchColor.b, touchColor.a);
			}
		}
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		if (touchSizeExists) {
			gameCollider.center = new Vector3 ((touchSize.x != 0 ? touchSize.x : bounds.w)/2, (touchSize.y != 0 ? touchSize.y : bounds.h)/2, 1.0f);
			gameCollider.size = new Vector3 ((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		} else {
			gameCollider.center = new Vector3 (bounds.w/2.0f, bounds.h/2.0f, 0.0f);
			gameCollider.size = new Vector3 (bounds.w, bounds.h, 1.0f);
		}
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (colorExists) {
			savedColor = new Color (color.r, color.g, color.b, color.a);
		} else {
			savedColor = new Color (0,0,0,0);
		}

		gameCollider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		if(touchSizeExists)
			gameCollider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		else
			gameCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;
	}

	public void SetSavedColor(cColor c) {
		savedColor = new Color (c.r, c.g, c.b, c.a);
	}
}
