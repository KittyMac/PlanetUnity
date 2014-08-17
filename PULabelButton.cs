
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


public partial class PULabelButton : PULabelButtonBase, IPUButton {
	public PlanetUnityButtonState state = PlanetUnityButtonState.Normal;

	public virtual void performTouchUp(bool isLongPress)
	{
		if (onTouchUpExists) {
			NotificationCenter.postNotification (scope (), this.onTouchUp, NotificationCenter.Args("sender", this, "isLongPress", isLongPress));
		}
	}

	public virtual void performTouchDown()
	{
		if (onTouchDownExists) {
			NotificationCenter.postNotification (scope (), this.onTouchDown, NotificationCenter.Args("sender", this));
		}
	}

	public void updateButtonToState(PlanetUnityButtonState newState)
	{
		TextMesh textMeshComponent = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;
		
		state = newState;

		if(state == PlanetUnityButtonState.Normal)
		{
			textMeshComponent.color = new Color (textColor.r, textColor.g, textColor.b, textColor.a);
		}
		if(state == PlanetUnityButtonState.Highlighted)
		{
			textMeshComponent.color = new Color (touchColor.r, touchColor.g, touchColor.b, touchColor.a);
		}
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		CreateCollider ();
	}

	private void CreateCollider(){
		if(touchSizeExists)
			gameCollider.size = new Vector3((touchSize.x != 0 ? touchSize.x : bounds.w), (touchSize.y != 0 ? touchSize.y : bounds.h), 1.0f);
		else
			gameCollider.size = new Vector3(bounds.w, bounds.h, 1.0f);

		if (this.alignment == PlanetUnity.LabelAlignment.left) {
			gameCollider.center = new Vector3 (bounds.w / 2, -bounds.h / 2, 0.0f);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.center) {
			gameCollider.center = new Vector3 (0.0f, -bounds.h / 2, 0.0f);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.right) {
			gameCollider.center = new Vector3 (-bounds.w / 2, -bounds.h / 2, 0.0f);
		}
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		gameCollider = (BoxCollider) gameObject.AddComponent(typeof(BoxCollider));
		CreateCollider ();

		PlanetUnityButtonScript buttonScript = (PlanetUnityButtonScript)gameObject.AddComponent(typeof(PlanetUnityButtonScript));
		buttonScript.entity = this;
	}
}
