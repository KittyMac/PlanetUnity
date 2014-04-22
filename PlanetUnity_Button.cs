
using UnityEngine;
using System.Xml;
using System;

public enum PlanetUnityButtonState {Normal, Highlighted};

public interface iPlanetUnity_Button {
	void updateButtonToState(PlanetUnityButtonState newState);
	void performTouchUp();
	void performTouchDown();
}

public class PlanetUnityButtonScript : MonoBehaviour {

	public iPlanetUnity_Button entity;

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

public class PlanetUnity_Button : PlanetUnity_ButtonBase, iPlanetUnity_Button {

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

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		var collider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		collider.size = new Vector3(bounds.w, bounds.h, 1.0f);

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;

		updateButtonToState (PlanetUnityButtonState.Normal);
	}

}
