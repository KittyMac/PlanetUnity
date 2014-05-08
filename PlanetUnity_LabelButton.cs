using System.Xml;
using UnityEngine;


public class PlanetUnity_LabelButton : PlanetUnity_LabelButtonBase, iPlanetUnity_Button {
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

		// Re-use the button code from PlanetUnity_Button
		var collider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		if(touchSizeExists)
			collider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w)*pxScale, (touchSize.y != 0 ? touchSize.y : bounds.h)*pxScale, 1.0f);
		else
			collider.size = new Vector3(bounds.w*pxScale, bounds.h*pxScale, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;
	}
}
