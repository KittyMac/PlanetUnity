

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


public partial class PULabelButton : PULabelButtonBase {
	
	public PULabelButton()
	{
		string attr;

		attr = "1,0,0,1";
		if(attr != null) { touchColor = attr; touchColorExists = true; } 

	}
	
	
	public PULabelButton(
			cVector2 touchSize,
			cColor touchColor,
			string onTouchUp,
			string onTouchDown,
			string value,
			cRect bounds ) : this()
	{
		this.touchSize = touchSize;
		this.touchSizeExists = true;

		this.touchColor = touchColor;
		this.touchColorExists = true;

		this.onTouchUp = onTouchUp;
		this.onTouchUpExists = true;

		this.onTouchDown = onTouchDown;
		this.onTouchDownExists = true;

		this.value = value;
		this.valueExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PULabelButton(
			cVector2 touchSize,
			cColor touchColor,
			string onTouchUp,
			string onTouchDown,
			string shader,
			string font,
			int fontSize,
			PlanetUnity.LabelAlignment alignment,
			cColor textColor,
			string value,
			cVector2 shadowOffset,
			cColor shadowColor,
			cRect bounds,
			cVector3 rotation,
			bool hidden,
			float lastY,
			float lastX,
			int renderQueueOffset,
			bool clipDepth,
			bool clipStencil,
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

		this.touchColor = touchColor;
		this.touchColorExists = true;

		this.onTouchUp = onTouchUp;
		this.onTouchUpExists = true;

		this.onTouchDown = onTouchDown;
		this.onTouchDownExists = true;

		this.shader = shader;
		this.shaderExists = true;

		this.font = font;
		this.fontExists = true;

		this.fontSize = fontSize;
		this.fontSizeExists = true;

		this.alignment = alignment;
		this.alignmentExists = true;

		this.textColor = textColor;
		this.textColorExists = true;

		this.value = value;
		this.valueExists = true;

		this.shadowOffset = shadowOffset;
		this.shadowOffsetExists = true;

		this.shadowColor = shadowColor;
		this.shadowColorExists = true;

		this.bounds = bounds;
		this.boundsExists = true;

		this.rotation = rotation;
		this.rotationExists = true;

		this.hidden = hidden;
		this.hiddenExists = true;

		this.lastY = lastY;
		this.lastYExists = true;

		this.lastX = lastX;
		this.lastXExists = true;

		this.renderQueueOffset = renderQueueOffset;
		this.renderQueueOffsetExists = true;

		this.clipDepth = clipDepth;
		this.clipDepthExists = true;

		this.clipStencil = clipStencil;
		this.clipStencilExists = true;

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




public class PULabelButtonBase : PULabel {


	private static Type planetOverride = Type.GetType("PlanetUnityOverride");
	private static MethodInfo processStringMethod = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static);




	// XML Attributes
	public cVector2 touchSize;
	public bool touchSizeExists;

	public cColor touchColor;
	public bool touchColorExists;

	public string onTouchUp;
	public bool onTouchUpExists;

	public string onTouchDown;
	public bool onTouchDownExists;




	
	public void SetTouchSize(cVector2 v) { touchSize = v; touchSizeExists = true; } 
	public void SetTouchColor(cColor v) { touchColor = v; touchColorExists = true; } 
	public void SetOnTouchUp(string v) { onTouchUp = v; onTouchUpExists = true; } 
	public void SetOnTouchDown(string v) { onTouchDown = v; onTouchDownExists = true; } 


	public override void gaxb_unload()
	{
		base.gaxb_unload();

	}
	
	public void gaxb_addToParent()
	{
		if(parent != null)
		{
			FieldInfo parentField = parent.GetType().GetField("LabelButton");
			List<object> parentChildren = null;
			
			if(parentField != null)
			{
				parentField.SetValue(parent, this);
				
				parentField = parent.GetType().GetField("LabelButtonExists");
				parentField.SetValue(parent, true);
			}
			else
			{
				parentField = parent.GetType().GetField("LabelButtons");
				
				if(parentField != null)
				{
					parentChildren = (List<object>)(parentField.GetValue(parent));
				}
				else
				{
					parentField = parent.GetType().GetField("Labels");
					if(parentField != null)
					{
						parentChildren = (List<object>)(parentField.GetValue(parent));
					}
				}
				if(parentChildren == null)
				{
					FieldInfo childrenField = parent.GetType().GetField("children");
					if(childrenField != null)
					{
						parentChildren = (List<object>)childrenField.GetValue(parent);
					}
				}
				if(parentChildren != null)
				{
					parentChildren.Add(this);
				}
				
			}
		}
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PULabelButton ))
		{
			gaxb_addToParent();
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("touchSize");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { touchSize = attr; touchSizeExists = true; } 
		
		attr = reader.GetAttribute("touchColor");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "1,0,0,1"; }
		if(attr != null) { touchColor = attr; touchColorExists = true; } 
		
		attr = reader.GetAttribute("onTouchUp");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { onTouchUp = attr; onTouchUpExists = true; } 
		
		attr = reader.GetAttribute("onTouchDown");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { onTouchDown = attr; onTouchDownExists = true; } 
		

	}
	
	
	
	
	
	
	
	public override void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(touchSizeExists) { sb.AppendFormat (" {0}=\"{1}\"", "touchSize", touchSize); }
		if(touchColorExists) { sb.AppendFormat (" {0}=\"{1}\"", "touchColor", touchColor); }
		if(onTouchUpExists) { sb.AppendFormat (" {0}=\"{1}\"", "onTouchUp", onTouchUp); }
		if(onTouchDownExists) { sb.AppendFormat (" {0}=\"{1}\"", "onTouchDown", onTouchDown); }

	}
	
	public override void gaxb_appendXMLSequences(StringBuilder sb)
	{
		base.gaxb_appendXMLSequences(sb);


	}
	
	public override void gaxb_appendXML(StringBuilder sb)
	{
		if(sb.Length == 0)
		{
			sb.AppendFormat ("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		}
		
		sb.AppendFormat ("<{0}", "LabelButton");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "LabelButton");
		}
	}
}
