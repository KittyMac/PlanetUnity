
using UnityEngine;
using System.Xml;

public class PlanetUnity_Gradient : PlanetUnity_GradientBase
{
	public new void gaxb_load (XmlReader reader, object _parent)
	{
		// Create our specific GameObject, set any defaults
		gameObject = (GameObject)new GameObject ("<Gradient/>", typeof(MeshRenderer), typeof(MeshFilter));

		base.gaxb_load (reader, _parent);

		if (titleExists) {
			gameObject.name = title;
		}

		Color cTop = new Color (colorTop.r, colorTop.g, colorTop.b, colorTop.a);
		Color cBottom = new Color (colorBottom.r, colorBottom.g, colorBottom.b, colorBottom.a);
		PlanetUnity_Color.CreateGradient (gameObject, bounds, anchor, cTop, cBottom);

		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}
}
