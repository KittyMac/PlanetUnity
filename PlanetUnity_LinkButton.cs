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

public class PlanetUnity_LinkButton : PlanetUnity_LinkButtonBase, iPlanetUnity_Button {
	public PlanetUnityButtonState state = PlanetUnityButtonState.Normal;
	protected CCText text;

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

		if (titleExists == false) {
			gameObject.name = "\""+value+"\"";
		}

		text = gameObject.AddComponent(typeof(CCText)) as CCText;

		CCFont ccfont = Resources.Load ("bmfonts/"+font, typeof(CCFont)) as CCFont;

		if (ccfont == null) {
			UnityEngine.Debug.Log ("Unable to find bmfont " + font);
			return;
		}

		text.Font = ccfont;
		text.Text = value;

		updateButtonToState (PlanetUnityButtonState.Normal);

		var tex = (Texture) Resources.Load ("bmfonts/"+font, typeof(Texture));
		text.renderer.material.mainTexture = tex;

		var shaderObj = Shader.Find("Somian/Unlit/Transparent");

		text.renderer.material.color = new Color (1, 1, 1, 1);
		text.renderer.material.shader = shaderObj;
		text.renderer.material.renderQueue = scope().getRenderQueue();

		float pxScale = ccfont.pixelScale;

		text.Width = bounds.w * pxScale;
		text.LineHeight = 0.8f;
		text.Bounding = CCText.BoundingMode.Margin;
		text.Alignment = CCText.AlignmentMode.Center;

		gameObject.transform.position = new Vector3(bounds.x, bounds.y+bounds.h, 1.0f);
		gameObject.transform.localScale = new Vector3 (1.0f/pxScale, 1.0f/pxScale, 1.0f);

		// Re-use the button code from PlanetUnity_Button
		var collider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		collider.size = new Vector3(bounds.w*pxScale, bounds.h*pxScale, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;
	}
}
