

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;


public partial class PUCode : PUCodeBase {
	
	public PUCode()
	{
	}
	
	
	public PUCode(
			string _class,
			cRect bounds ) : this()
	{
		this._class = _class;
		this._classExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUCode(
			string _class,
			cRect bounds,
			bool hidden,
			float lastY,
			float lastX,
			int renderQueueOffset,
			string title,
			string tag,
			string tag1,
			string tag2,
			string tag3,
			string tag4,
			string tag5,
			string tag6 ) : this()
	{
		this._class = _class;
		this._classExists = true;

		this.bounds = bounds;
		this.boundsExists = true;

		this.hidden = hidden;
		this.hiddenExists = true;

		this.lastY = lastY;
		this.lastYExists = true;

		this.lastX = lastX;
		this.lastXExists = true;

		this.renderQueueOffset = renderQueueOffset;
		this.renderQueueOffsetExists = true;

		this.title = title;
		this.titleExists = true;

		this.tag = tag;
		this.tagExists = true;

		this.tag1 = tag1;
		this.tag1Exists = true;

		this.tag2 = tag2;
		this.tag2Exists = true;

		this.tag3 = tag3;
		this.tag3Exists = true;

		this.tag4 = tag4;
		this.tag4Exists = true;

		this.tag5 = tag5;
		this.tag5Exists = true;

		this.tag6 = tag6;
		this.tag6Exists = true;
	}


}




public class PUCodeBase : PUGameObject {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string _class;
	public bool _classExists;




	// XML Sequences
	public List<object> Notifications = new List<object>();
	


	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PUCode ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Code");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("CodeExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Codes");
					
					if(parentField != null)
					{
						parentChildren = (List<object>)(parentField.GetValue(_parent));
					}
					else
					{
						parentField = _parent.GetType().GetField("GameObjects");
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

		MethodInfo mInfo;		foreach(object o in Notifications) { mInfo = o.GetType().GetMethod("gaxb_appendXML"); if(mInfo != null) { mInfo.Invoke (o, new[] { sb }); } else { sb.AppendFormat ("<{0}>{1}</{0}>", "Notification", o); } }
	

	}
	
	public new void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "Code");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Code");
		}
	}
}
