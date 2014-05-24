

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;


public partial class PUColorButton : PUColorButtonBase {
	
	public PUColorButton()
	{
	}
	
	
	public PUColorButton(
			cVector2 touchSize,
			string onTouchUp,
			string onTouchDown,
			cColor color,
			cRect bounds ) : this()
	{
		this.touchSize = touchSize;
		this.touchSizeExists = true;

		this.onTouchUp = onTouchUp;
		this.onTouchUpExists = true;

		this.onTouchDown = onTouchDown;
		this.onTouchDownExists = true;

		this.color = color;
		this.colorExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUColorButton(
			cVector2 touchSize,
			string onTouchUp,
			string onTouchDown,
			string shader,
			cColor color,
			cVector2 anchor,
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
		this.touchSize = touchSize;
		this.touchSizeExists = true;

		this.onTouchUp = onTouchUp;
		this.onTouchUpExists = true;

		this.onTouchDown = onTouchDown;
		this.onTouchDownExists = true;

		this.shader = shader;
		this.shaderExists = true;

		this.color = color;
		this.colorExists = true;

		this.anchor = anchor;
		this.anchorExists = true;

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




public class PUColorButtonBase : PUColor {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public cVector2 touchSize;
	public bool touchSizeExists;

	public string onTouchUp;
	public bool onTouchUpExists;

	public string onTouchDown;
	public bool onTouchDownExists;




	
	public void SetTouchSize(cVector2 v) { touchSize = v; touchSizeExists = true; } 
	public void SetOnTouchUp(string v) { onTouchUp = v; onTouchUpExists = true; } 
	public void SetOnTouchDown(string v) { onTouchDown = v; onTouchDownExists = true; } 

	

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PUColorButton ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("ColorButton");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("ColorButtonExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("ColorButtons");
					
					if(parentField != null)
					{
						parentChildren = (List<object>)(parentField.GetValue(_parent));
					}
					else
					{
						parentField = _parent.GetType().GetField("Colors");
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
		
		sb.AppendFormat ("<{0}", "ColorButton");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "ColorButton");
		}
	}
}
