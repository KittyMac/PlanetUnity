

//
// Autogenerated by gaxb at 02:06:01 PM on 05/01/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_ImageBase : PlanetUnity_Entity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string resourcePath;
	public bool resourcePathExists;

	public string shader;
	public bool shaderExists;

	public cVector2 anchor;
	public bool anchorExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		if(reader == null && _parent == null)
			return;
			
		base.gaxb_load(reader, _parent);


		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_Image ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Images");
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
		attr = reader.GetAttribute("resourcePath");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { resourcePath = attr; resourcePathExists = true; } 
		
		attr = reader.GetAttribute("shader");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { shader = attr; shaderExists = true; } 
		
		attr = reader.GetAttribute("anchor");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0"; }
		if(attr != null) { anchor = attr; anchorExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(resourcePathExists) { sb.AppendFormat (" {0}=\"{1}\"", "resourcePath", resourcePath); }
		if(shaderExists) { sb.AppendFormat (" {0}=\"{1}\"", "shader", shader); }
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
		
		sb.AppendFormat ("<{0}", "Image");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Image");
		}
	}
}
