

//
// Autogenerated by gaxb at 01:12:52 PM on 04/30/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_SubscribeBase : IPlanetUnity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");


	public object parent;
	public string xmlns;


	// XML Attributes
	public string name;
	public bool nameExists;





	public void gaxb_load(XmlReader reader, object _parent)
	{


		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_Subscribe ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Subscribes");
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
		attr = reader.GetAttribute("name");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { name = attr; nameExists = true; } 
		

	}
	
	
	
	
	
	
	
	public void gaxb_appendXMLAttributes(StringBuilder sb)
	{

		if(nameExists) { sb.AppendFormat (" {0}=\"{1}\"", "name", name); }

	}
	
	public void gaxb_appendXMLSequences(StringBuilder sb)
	{


	}
	
	public void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "Subscribe");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Subscribe");
		}
	}
}
