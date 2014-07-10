

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


public partial class PUImage : PUImageBase {
	
	public PUImage()
	{
		string attr;

		attr = "0,0";
		if(attr != null) { anchor = attr; anchorExists = true; } 

	}
	
	
	public PUImage(
			string resourcePath,
			string shader,
			cVector2 anchor,
			cColor color,
			cRect bounds ) : this()
	{
		this.resourcePath = resourcePath;
		this.resourcePathExists = true;

		this.shader = shader;
		this.shaderExists = true;

		this.anchor = anchor;
		this.anchorExists = true;

		this.color = color;
		this.colorExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUImage(
			string resourcePath,
			string shader,
			cVector2 anchor,
			cColor color,
			cRect bounds,
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
		this.resourcePath = resourcePath;
		this.resourcePathExists = true;

		this.shader = shader;
		this.shaderExists = true;

		this.anchor = anchor;
		this.anchorExists = true;

		this.color = color;
		this.colorExists = true;

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




public class PUImageBase : PUGameObject {


	private Type planetOverride = Type.GetType("PlanetUnityOverride");




	// XML Attributes
	public string resourcePath;
	public bool resourcePathExists;

	public string shader;
	public bool shaderExists;

	public cVector2 anchor;
	public bool anchorExists;

	public cColor color;
	public bool colorExists;




	
	public void SetResourcePath(string v) { resourcePath = v; resourcePathExists = true; } 
	public void SetShader(string v) { shader = v; shaderExists = true; } 
	public void SetAnchor(cVector2 v) { anchor = v; anchorExists = true; } 
	public void SetColor(cColor v) { color = v; colorExists = true; } 


	public override void gaxb_unload()
	{
		base.gaxb_unload();

	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if(reader == null && _parent == null)
			return;
		
		parent = _parent;
		
		if(this.GetType() == typeof( PUImage ))
		{
			if(parent != null)
			{
				FieldInfo parentField = _parent.GetType().GetField("Image");
				List<object> parentChildren = null;
				
				if(parentField != null)
				{
					parentField.SetValue(_parent, this);
					
					parentField = _parent.GetType().GetField("ImageExists");
					parentField.SetValue(_parent, true);
				}
				else
				{
					parentField = _parent.GetType().GetField("Images");
					
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
		
		attr = reader.GetAttribute("color");
		if(attr != null && planetOverride != null) { attr = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { color = attr; colorExists = true; } 
		

	}
	
	
	
	
	
	
	
	public override void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(resourcePathExists) { sb.AppendFormat (" {0}=\"{1}\"", "resourcePath", resourcePath); }
		if(shaderExists) { sb.AppendFormat (" {0}=\"{1}\"", "shader", shader); }
		if(anchorExists) { sb.AppendFormat (" {0}=\"{1}\"", "anchor", anchor); }
		if(colorExists) { sb.AppendFormat (" {0}=\"{1}\"", "color", color); }

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
