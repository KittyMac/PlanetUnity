

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

public class PULabelBase : PUGameObject {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string shader;
	public bool shaderExists;

	public string font;
	public bool fontExists;

	public int fontSize;
	public bool fontSizeExists;

	public PlanetUnity.LabelAlignment alignment;
	public bool alignmentExists;

	public cColor textColor;
	public bool textColorExists;

	public string value;
	public bool valueExists;

	public bool clips;
	public bool clipsExists;





	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PULabel ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Label");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("LabelExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Labels");
					
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
		attr = reader.GetAttribute("shader");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { shader = attr; shaderExists = true; } 
		
		attr = reader.GetAttribute("font");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { font = attr; fontExists = true; } 
		
		attr = reader.GetAttribute("fontSize");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "12"; }
		if(attr != null) { fontSize = int.Parse(attr); fontSizeExists = true; } 
		
		attr = reader.GetAttribute("alignment");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "center"; }
		if(attr != null) { alignment = (PlanetUnity.LabelAlignment)System.Enum.Parse(typeof(PlanetUnity.LabelAlignment), attr); alignmentExists = true; } 
		
		attr = reader.GetAttribute("textColor");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0,0,1"; }
		if(attr != null) { textColor = attr; textColorExists = true; } 
		
		attr = reader.GetAttribute("value");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { value = attr; valueExists = true; } 
		
		attr = reader.GetAttribute("clips");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "true"; }
		if(attr != null) { clips = bool.Parse(attr); clipsExists = true; } 
		

	}
	
	
	
	
	
	
	
	public new void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(shaderExists) { sb.AppendFormat (" {0}=\"{1}\"", "shader", shader); }
		if(fontExists) { sb.AppendFormat (" {0}=\"{1}\"", "font", font); }
		if(fontSizeExists) { sb.AppendFormat (" {0}=\"{1}\"", "fontSize", fontSize); }
		if(alignmentExists) { sb.AppendFormat (" {0}=\"{1}\"", "alignment", (int)alignment); }
		if(textColorExists) { sb.AppendFormat (" {0}=\"{1}\"", "textColor", textColor); }
		if(valueExists) { sb.AppendFormat (" {0}=\"{1}\"", "value", value); }
		if(clipsExists) { sb.AppendFormat (" {0}=\"{1}\"", "clips", clips.ToString().ToLower()); }

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
		
		sb.AppendFormat ("<{0}", "Label");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Label");
		}
	}
}
