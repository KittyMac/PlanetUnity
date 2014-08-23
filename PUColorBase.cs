

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


public partial class PUColor : PUColorBase {
	
	public PUColor()
	{
		string attr;

		attr = "0,0";
		if(attr != null) { anchor = attr; anchorExists = true; } 

	}
	
	
	public PUColor(
			string shader,
			cColor color,
			string mesh,
			cVector2 anchor,
			cRect bounds ) : this()
	{
		this.shader = shader;
		this.shaderExists = true;

		this.color = color;
		this.colorExists = true;

		this.mesh = mesh;
		this.meshExists = true;

		this.anchor = anchor;
		this.anchorExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUColor(
			string shader,
			cColor color,
			string mesh,
			cVector2 anchor,
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
		this.shader = shader;
		this.shaderExists = true;

		this.color = color;
		this.colorExists = true;

		this.mesh = mesh;
		this.meshExists = true;

		this.anchor = anchor;
		this.anchorExists = true;

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




public class PUColorBase : PUGameObject {


	private static Type planetOverride = Type.GetType("PlanetUnityOverride");
	private static MethodInfo processStringMethod = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static);




	// XML Attributes
	public string shader;
	public bool shaderExists;

	public cColor color;
	public bool colorExists;

	public string mesh;
	public bool meshExists;

	public cVector2 anchor;
	public bool anchorExists;




	
	public void SetShader(string v) { shader = v; shaderExists = true; } 
	public void SetColor(cColor v) { color = v; colorExists = true; } 
	public void SetMesh(string v) { mesh = v; meshExists = true; } 
	public void SetAnchor(cVector2 v) { anchor = v; anchorExists = true; } 


	public override void gaxb_unload()
	{
		base.gaxb_unload();

	}
	
	public void gaxb_addToParent()
	{
		if(parent != null)
		{
			FieldInfo parentField = parent.GetType().GetField("Color");
			List<object> parentChildren = null;
			
			if(parentField != null)
			{
				parentField.SetValue(parent, this);
				
				parentField = parent.GetType().GetField("ColorExists");
				parentField.SetValue(parent, true);
			}
			else
			{
				parentField = parent.GetType().GetField("Colors");
				
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
		
		if(this.GetType() == typeof( PUColor ))
		{
			gaxb_addToParent();
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("shader");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { shader = attr; shaderExists = true; } 
		
		attr = reader.GetAttribute("color");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { color = attr; colorExists = true; } 
		
		attr = reader.GetAttribute("mesh");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { mesh = attr; meshExists = true; } 
		
		attr = reader.GetAttribute("anchor");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "0,0"; }
		if(attr != null) { anchor = attr; anchorExists = true; } 
		

	}
	
	
	
	
	
	
	
	public override void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(shaderExists) { sb.AppendFormat (" {0}=\"{1}\"", "shader", shader); }
		if(colorExists) { sb.AppendFormat (" {0}=\"{1}\"", "color", color); }
		if(meshExists) { sb.AppendFormat (" {0}=\"{1}\"", "mesh", mesh); }
		if(anchorExists) { sb.AppendFormat (" {0}=\"{1}\"", "anchor", anchor); }

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
		
		sb.AppendFormat ("<{0}", "Color");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Color");
		}
	}
}
