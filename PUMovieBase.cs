

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


public partial class PUMovie : PUMovieBase {
	
	public PUMovie()
	{
		string attr;

		attr = "0,0";
		if(attr != null) { anchor = attr; anchorExists = true; } 

	}
	
	
	public PUMovie(
			bool hasAlpha,
			bool looping,
			string resourcePath,
			string shader,
			cVector2 anchor,
			cColor color,
			cRect bounds ) : this()
	{
		this.hasAlpha = hasAlpha;
		this.hasAlphaExists = true;

		this.looping = looping;
		this.loopingExists = true;

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

	
	
	public PUMovie(
			bool hasAlpha,
			bool looping,
			string resourcePath,
			string shader,
			cVector2 anchor,
			cColor color,
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
		this.hasAlpha = hasAlpha;
		this.hasAlphaExists = true;

		this.looping = looping;
		this.loopingExists = true;

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




public class PUMovieBase : PUGameObject {


	private static Type planetOverride = Type.GetType("PlanetUnityOverride");
	private static MethodInfo processStringMethod = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static);




	// XML Attributes
	public bool hasAlpha;
	public bool hasAlphaExists;

	public bool looping;
	public bool loopingExists;

	public string resourcePath;
	public bool resourcePathExists;

	public string shader;
	public bool shaderExists;

	public cVector2 anchor;
	public bool anchorExists;

	public cColor color;
	public bool colorExists;




	
	public void SetHasAlpha(bool v) { hasAlpha = v; hasAlphaExists = true; } 
	public void SetLooping(bool v) { looping = v; loopingExists = true; } 
	public void SetResourcePath(string v) { resourcePath = v; resourcePathExists = true; } 
	public void SetShader(string v) { shader = v; shaderExists = true; } 
	public void SetAnchor(cVector2 v) { anchor = v; anchorExists = true; } 
	public void SetColor(cColor v) { color = v; colorExists = true; } 


	public override void gaxb_unload()
	{
		base.gaxb_unload();

	}
	
	public void gaxb_addToParent()
	{
		if(parent != null)
		{
			FieldInfo parentField = parent.GetType().GetField("Movie");
			List<object> parentChildren = null;
			
			if(parentField != null)
			{
				parentField.SetValue(parent, this);
				
				parentField = parent.GetType().GetField("MovieExists");
				parentField.SetValue(parent, true);
			}
			else
			{
				parentField = parent.GetType().GetField("Movies");
				
				if(parentField != null)
				{
					parentChildren = (List<object>)(parentField.GetValue(parent));
				}
				else
				{
					parentField = parent.GetType().GetField("GameObjects");
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
		
		if(this.GetType() == typeof( PUMovie ))
		{
			gaxb_addToParent();
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("hasAlpha");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { hasAlpha = bool.Parse(attr); hasAlphaExists = true; } 
		
		attr = reader.GetAttribute("looping");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { looping = bool.Parse(attr); loopingExists = true; } 
		
		attr = reader.GetAttribute("resourcePath");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { resourcePath = attr; resourcePathExists = true; } 
		
		attr = reader.GetAttribute("shader");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { shader = attr; shaderExists = true; } 
		
		attr = reader.GetAttribute("anchor");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0"; }
		if(attr != null) { anchor = attr; anchorExists = true; } 
		
		attr = reader.GetAttribute("color");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { color = attr; colorExists = true; } 
		

	}
	
	
	
	
	
	
	
	public override void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(hasAlphaExists) { sb.AppendFormat (" {0}=\"{1}\"", "hasAlpha", hasAlpha.ToString().ToLower()); }
		if(loopingExists) { sb.AppendFormat (" {0}=\"{1}\"", "looping", looping.ToString().ToLower()); }
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
		
		sb.AppendFormat ("<{0}", "Movie");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Movie");
		}
	}
}