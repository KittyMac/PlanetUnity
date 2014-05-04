

//
// Autogenerated by gaxb at 03:58:19 PM on 05/04/14
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PlanetUnity_ControllerBase : PlanetUnity_Entity {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string _class;
	public bool _classExists;




	// XML Sequences
	public List<object> Subscribes = new List<object>();
	


	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PlanetUnity_Controller ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Controller");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("ControllerExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Controllers");
					
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
		attr = reader.GetAttribute("class");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { _class = attr; _classExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(_classExists) { sb.AppendFormat (" {0}=\"{1}\"", "_class", _class); }

	}
	
	public new void gaxb_appendXMLSequences(StringBuilder sb)
	{
		base.gaxb_appendXMLSequences(sb);

		MethodInfo mInfo;		foreach(object o in Subscribes) { mInfo = o.GetType().GetMethod("gaxb_appendXML"); if(mInfo != null) { mInfo.Invoke (o, new[] { sb }); } else { sb.AppendFormat ("<{0}>{1}</{0}>", "Subscribe", o); } }
	

	}
	
	public new void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "Controller");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Controller");
		}
	}
}
