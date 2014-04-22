
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
