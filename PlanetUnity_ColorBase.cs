

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_ColorBase : PlanetUnity_Entity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public cColor color;
	public bool colorExists;

	public cVector2 anchor;
	public bool anchorExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_Color ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Color");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("ColorExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Colors");
					
					if(parentField != null)
					{
						parentChildren = (List<object>)(parentField.GetValue(_parent));
					}
					else
					{
						parentField = _parent.GetType().GetField("Entitys");
						if(parentField != null)
						{
							parentChildren = (List<object>)(parentField.GetValue(_parent));
						}
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
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("color");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { color = attr; colorExists = true; } 
		
		attr = reader.GetAttribute("anchor");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0"; }
		if(attr != null) { anchor = attr; anchorExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(colorExists) { sb.AppendFormat (" {0}=\"{1}\"", "color", color); }
		if(anchorExists) { sb.AppendFormat (" {0}=\"{1}\"", "anchor", anchor); }

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
		
		sb.AppendFormat ("<{0}", "Color");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Color");
		}
	}
}
