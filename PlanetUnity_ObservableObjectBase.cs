

//
// Autogenerated by gaxb at 09:59:08 PM on 05/01/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_ObservableObjectBase : IPlanetUnity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");


	public object parent;
	public string xmlns;


	// XML Attributes
	public string title;
	public bool titleExists;

	public string tag;
	public bool tagExists;

	public string tag1;
	public bool tag1Exists;

	public string tag2;
	public bool tag2Exists;

	public string tag3;
	public bool tag3Exists;

	public string tag4;
	public bool tag4Exists;

	public string tag5;
	public bool tag5Exists;

	public string tag6;
	public bool tag6Exists;




	// XML Sequences
	public List<object> children = new List<object>();
	


	public void gaxb_load(XmlReader reader, object _parent)
	{	

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_ObservableObject ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("ObservableObjects");
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
		attr = reader.GetAttribute("title");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { title = attr; titleExists = true; } 
		
		attr = reader.GetAttribute("tag");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag = attr; tagExists = true; } 
		
		attr = reader.GetAttribute("tag1");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag1 = attr; tag1Exists = true; } 
		
		attr = reader.GetAttribute("tag2");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag2 = attr; tag2Exists = true; } 
		
		attr = reader.GetAttribute("tag3");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag3 = attr; tag3Exists = true; } 
		
		attr = reader.GetAttribute("tag4");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag4 = attr; tag4Exists = true; } 
		
		attr = reader.GetAttribute("tag5");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag5 = attr; tag5Exists = true; } 
		
		attr = reader.GetAttribute("tag6");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { tag6 = attr; tag6Exists = true; } 
		

	}
	
	
	
	
	
	
	
	public void gaxb_appendXMLAttributes(StringBuilder sb)
	{

		if(titleExists) { sb.AppendFormat (" {0}=\"{1}\"", "title", title); }
		if(tagExists) { sb.AppendFormat (" {0}=\"{1}\"", "tag", tag); }
		if(tag1Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag1", tag1); }
		if(tag2Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag2", tag2); }
		if(tag3Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag3", tag3); }
		if(tag4Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag4", tag4); }
		if(tag5Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag5", tag5); }
		if(tag6Exists) { sb.AppendFormat (" {0}=\"{1}\"", "tag6", tag6); }

	}
	
	public void gaxb_appendXMLSequences(StringBuilder sb)
	{

		foreach(object o in children) { MethodInfo mInfo = o.GetType().GetMethod("gaxb_appendXML"); if(mInfo != null) { mInfo.Invoke (o, new[] { sb }); } else { sb.AppendFormat ("<{0}>{1}</{0}>", "any", o); } }
	

	}
	
	public void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "ObservableObject");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "ObservableObject");
		}
	}
}
