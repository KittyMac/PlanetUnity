using System.Xml;
using System;
using System.Reflection;

public class PlanetUnity_ObservableObject : PlanetUnity_ObservableObjectBase {

	private int renderQeueuCount = 0;
	public int getRenderQueue()
	{
		return renderQeueuCount++;
	}

	public void peformOnChildren(Action<object> block)
	{
		foreach(object child in children)
		{
			block (child);

			MethodInfo method = child.GetType().GetMethod ("peformOnChildren");
			if (method != null) { method.Invoke (child, new[] { block }); }
		}
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);
		renderQeueuCount = 0;
	}

	public void gaxb_unload()
	{
		NotificationCenter.removeObserver (this);
	}

	public PlanetUnity_ObservableObject scope()
	{
		if (isScopeContainer ())
			return this;
		if (parent == null)
			return this;
		return (parent as PlanetUnity_ObservableObject).scope();
	}

	public bool isScopeContainer()
	{
		return false;
	}
}
