

using UnityEngine;
using System.Xml;
using System;
using System.Reflection;
using System.Collections.Generic;

interface IPlanetUnity_Controller {

}

public class PlanetUnity_Controller : PlanetUnity_ControllerBase {

	IPlanetUnity_Controller controller;

	public void gaxb_unload()
	{
		base.gaxb_unload ();
		NotificationCenter.removeObserver (controller);
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		gameObject.name = _class;
	}

	public void gaxb_loadComplete()
	{
		if (_classExists) {
			try {
				controller = (IPlanetUnity_Controller)gameObject.AddComponent(Type.GetType (_class, true));

				PlanetUnity_Entity scene = scope() as PlanetUnity_Entity;
				if(scene != null)
				{
					scene.peformOnChildren(val =>
						{
							PlanetUnity_ObservableObject oo = val as PlanetUnity_ObservableObject;
							if(oo != null && oo.title != null)
							{
								FieldInfo field = controller.GetType ().GetField (oo.title);
								if (field != null)
								{
									field.SetValue (controller, oo);
								}
							}
						});
				}
			}
			catch(Exception e) {
				UnityEngine.Debug.Log ("Controller error: " + e);
			}
		}

		foreach(PlanetUnity_Subscribe subscribe in Subscribes)
		{
			NotificationCenter.addObserver(controller, subscribe.name, subscribe.name, scope());
		}
	}

}
