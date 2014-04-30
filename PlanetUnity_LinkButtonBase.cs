

//
// Autogenerated by gaxb at 01:12:52 PM on 04/30/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_LinkButtonBase : PlanetUnity_Entity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string font;
	public bool fontExists;

	public int size;
	public bool sizeExists;

	public cColor textColor;
	public bool textColorExists;

	public string value;
	public bool valueExists;

	public cVector2 touchSize;
	public bool touchSizeExists;

	public string onTouchUp;
	public bool onTouchUpExists;

	public string onTouchDown;
	public bool onTouchDownExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);


		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_LinkButton ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("LinkButtons");
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
		attr = reader.GetAttribute("font");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { font = attr; fontExists = true; } 
		
		attr = reader.GetAttribute("size");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { size = int.Parse(attr); sizeExists = true; } 
		
		attr = reader.GetAttribute("textColor");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "1,1,1,1"; }
		if(attr != null) { textColor = attr; textColorExists = true; } 
		
		attr = reader.GetAttribute("value");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { value = attr; valueExists = true; } 
		
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

		if(fontExists) { sb.AppendFormat (" {0}=\"{1}\"", "font", font); }
		if(sizeExists) { sb.AppendFormat (" {0}=\"{1}\"", "size", size); }
		if(textColorExists) { sb.AppendFormat (" {0}=\"{1}\"", "textColor", textColor); }
		if(valueExists) { sb.AppendFormat (" {0}=\"{1}\"", "value", value); }
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
		
		sb.AppendFormat ("<{0}", "LinkButton");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "LinkButton");
		}
	}
}
