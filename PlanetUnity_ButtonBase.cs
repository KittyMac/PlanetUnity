

//
// Autogenerated by gaxb at 02:06:01 PM on 05/01/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_ButtonBase : PlanetUnity_Image {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string normalResourcePath;
	public bool normalResourcePathExists;

	public string highlightedResourcePath;
	public bool highlightedResourcePathExists;

	public cVector2 touchSize;
	public bool touchSizeExists;

	public string onTouchUp;
	public bool onTouchUpExists;

	public string onTouchDown;
	public bool onTouchDownExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		if(reader == null && _parent == null)
			return;
			
		base.gaxb_load(reader, _parent);


		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_Button ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Buttons");
				List<object> parentChildren = null;
				if(parentField != null)
				{
					parentChildren = (List<object>)(parentField.GetValue(_parent));
				}
				if(parentChildren == null)
				{
					FieldInfo childrenField = _parent.GetType().GetField("children");
					if(childrenField != null)
					{
						parentChildren = (List<object>)childrenField.GetValue(_parent);
					}
				}
				if(parentChildren != null)
				{
					parentChildren.Add(this);
				}
			}
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("normalResourcePath");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { normalResourcePath = attr; normalResourcePathExists = true; } 
		
		attr = reader.GetAttribute("highlightedResourcePath");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { highlightedResourcePath = attr; highlightedResourcePathExists = true; } 
		
		attr = reader.GetAttribute("touchSize");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { touchSize = attr; touchSizeExists = true; } 
		
		attr = reader.GetAttribute("onTouchUp");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { onTouchUp = attr; onTouchUpExists = true; } 
		
		attr = reader.GetAttribute("onTouchDown");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { onTouchDown = attr; onTouchDownExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(normalResourcePathExists) { sb.AppendFormat (" {0}=\"{1}\"", "normalResourcePath", normalResourcePath); }
		if(highlightedResourcePathExists) { sb.AppendFormat (" {0}=\"{1}\"", "highlightedResourcePath", highlightedResourcePath); }
		if(touchSizeExists) { sb.AppendFormat (" {0}=\"{1}\"", "touchSize", touchSize); }
		if(onTouchUpExists) { sb.AppendFormat (" {0}=\"{1}\"", "onTouchUp", onTouchUp); }
		if(onTouchDownExists) { sb.AppendFormat (" {0}=\"{1}\"", "onTouchDown", onTouchDown); }

	}
	
	public new void gaxb_appendXMLSequences(StringBuilder sb)
	{
		base.gaxb_appendXMLSequences(sb);


	}
	
	public new void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "Button");
		
		if(xmlns != null)
		{
			sb.AppendFormat (" {0}=\"{1}\"", "xmlns", xmlns);
		}
		
		gaxb_appendXMLAttributes(sb);
		
		
		StringBuilder seq = new StringBuilder();
		seq.AppendFormat(" ");
		gaxb_appendXMLSequences(seq);
		
		if(seq.Length == 1)
		{
			sb.AppendFormat (" />");
		}
		else
		{
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Button");
		}
	}
}
