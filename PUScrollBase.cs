

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;


public partial class PUScroll : PUScrollBase {
	
	public PUScroll()
	{
		string attr;

		attr = "0,0";
		if(attr != null) { contentSize = attr; contentSizeExists = true; } 
		attr = "true";
		if(attr != null) { bounces = bool.Parse(attr); bouncesExists = true; } 
		attr = "true";
		if(attr != null) { scrollEnabled = bool.Parse(attr); scrollEnabledExists = true; } 
		attr = "false";
		if(attr != null) { directionalLockEnabled = bool.Parse(attr); directionalLockEnabledExists = true; } 

	}
	
	
	public PUScroll(
			cVector2 contentSize,
			bool bounces,
			bool pagingEnabled,
			bool scrollEnabled,
			PlanetUnity.ScrollDirection scrollDirection,
			bool directionalLockEnabled,
			cRect bounds ) : this()
	{
		this.contentSize = contentSize;
		this.contentSizeExists = true;

		this.bounces = bounces;
		this.bouncesExists = true;

		this.pagingEnabled = pagingEnabled;
		this.pagingEnabledExists = true;

		this.scrollEnabled = scrollEnabled;
		this.scrollEnabledExists = true;

		this.scrollDirection = scrollDirection;
		this.scrollDirectionExists = true;

		this.directionalLockEnabled = directionalLockEnabled;
		this.directionalLockEnabledExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUScroll(
			cVector2 contentSize,
			bool bounces,
			bool pagingEnabled,
			bool scrollEnabled,
			PlanetUnity.ScrollDirection scrollDirection,
			bool directionalLockEnabled,
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
		this.contentSize = contentSize;
		this.contentSizeExists = true;

		this.bounces = bounces;
		this.bouncesExists = true;

		this.pagingEnabled = pagingEnabled;
		this.pagingEnabledExists = true;

		this.scrollEnabled = scrollEnabled;
		this.scrollEnabledExists = true;

		this.scrollDirection = scrollDirection;
		this.scrollDirectionExists = true;

		this.directionalLockEnabled = directionalLockEnabled;
		this.directionalLockEnabledExists = true;

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




public class PUScrollBase : PUGameObject {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public cVector2 contentSize;
	public bool contentSizeExists;

	public bool bounces;
	public bool bouncesExists;

	public bool pagingEnabled;
	public bool pagingEnabledExists;

	public bool scrollEnabled;
	public bool scrollEnabledExists;

	public PlanetUnity.ScrollDirection scrollDirection;
	public bool scrollDirectionExists;

	public bool directionalLockEnabled;
	public bool directionalLockEnabledExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PUScroll ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Scroll");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("ScrollExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Scrolls");
					
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
		attr = reader.GetAttribute("contentSize");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0"; }
		if(attr != null) { contentSize = attr; contentSizeExists = true; } 
		
		attr = reader.GetAttribute("bounces");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "true"; }
		if(attr != null) { bounces = bool.Parse(attr); bouncesExists = true; } 
		
		attr = reader.GetAttribute("pagingEnabled");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { pagingEnabled = bool.Parse(attr); pagingEnabledExists = true; } 
		
		attr = reader.GetAttribute("scrollEnabled");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "true"; }
		if(attr != null) { scrollEnabled = bool.Parse(attr); scrollEnabledExists = true; } 
		
		attr = reader.GetAttribute("scrollDirection");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { scrollDirection = (PlanetUnity.ScrollDirection)System.Enum.Parse(typeof(PlanetUnity.ScrollDirection), attr); scrollDirectionExists = true; } 
		
		attr = reader.GetAttribute("directionalLockEnabled");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "false"; }
		if(attr != null) { directionalLockEnabled = bool.Parse(attr); directionalLockEnabledExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(contentSizeExists) { sb.AppendFormat (" {0}=\"{1}\"", "contentSize", contentSize); }
		if(bouncesExists) { sb.AppendFormat (" {0}=\"{1}\"", "bounces", bounces.ToString().ToLower()); }
		if(pagingEnabledExists) { sb.AppendFormat (" {0}=\"{1}\"", "pagingEnabled", pagingEnabled.ToString().ToLower()); }
		if(scrollEnabledExists) { sb.AppendFormat (" {0}=\"{1}\"", "scrollEnabled", scrollEnabled.ToString().ToLower()); }
		if(scrollDirectionExists) { sb.AppendFormat (" {0}=\"{1}\"", "scrollDirection", (int)scrollDirection); }
		if(directionalLockEnabledExists) { sb.AppendFormat (" {0}=\"{1}\"", "directionalLockEnabled", directionalLockEnabled.ToString().ToLower()); }

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
		
		sb.AppendFormat ("<{0}", "Scroll");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Scroll");
		}
	}
}
