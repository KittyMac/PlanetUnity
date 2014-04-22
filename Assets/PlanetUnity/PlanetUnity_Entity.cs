
using UnityEngine;
using System.Xml;

public class PlanetUnity_Entity : PlanetUnity_EntityBase {
	public GameObject gameObject;

	public new void gaxb_unload()
	{
		base.gaxb_unload ();
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if (gameObject == null) {
			gameObject = new GameObject ("<Entity />");

			if (titleExists) {
				gameObject.name = title;
			}
		}

		if (_parent is PlanetUnity_Entity) {
			PlanetUnity_Entity parentEntity = (PlanetUnity_Entity)_parent;
			gameObject.transform.parent = parentEntity.gameObject.transform;

			if (boundsExists) {
				bounds.y = (parentEntity.bounds.h - bounds.y) - bounds.h;
			}
		}

		gameObject.layer = 31;
	}
}
