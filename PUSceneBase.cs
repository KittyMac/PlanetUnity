

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


public partial class PUScene : PUSceneBase {
	
	public PUScene()
	{
		string attr;

		attr = "true";
		if(attr != null) { adjustCamera = bool.Parse(attr); adjustCameraExists = true; } 

	}
	
	
	public PUScene(
			bool adjustCamera,
			int fps,
			cRect bounds ) : this()
	{
		this.adjustCamera = adjustCamera;
		this.adjustCameraExists = true;

		this.fps = fps;
		this.fpsExists = true;

		this.bounds = bounds;
		this.boundsExists = true;
	}

	
	
	public PUScene(
			bool adjustCamera,
			int fps,
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
		this.adjustCamera = adjustCamera;
		this.adjustCameraExists = true;

		this.fps = fps;
		this.fpsExists = true;

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




public class PUSceneBase : PUGameObject {


	private static Type planetOverride = Type.GetType("PlanetUnityOverride");
	private static MethodInfo processStringMethod = planetOverride.GetMethod("processString", BindingFlags.Public | BindingFlags.Static);




	// XML Attributes
	public bool adjustCamera;
	public bool adjustCameraExists;

	public int fps;
	public bool fpsExists;




	
	public void SetAdjustCamera(bool v) { adjustCamera = v; adjustCameraExists = true; } 
	public void SetFps(int v) { fps = v; fpsExists = true; } 


	public override void gaxb_unload()
	{
		base.gaxb_unload();

	}
	
	public void gaxb_addToParent()
	{
		if(parent != null)
		{
			FieldInfo parentField = parent.GetType().GetField("Scene");
			List<object> parentChildren = null;
			
			if(parentField != null)
			{
				parentField.SetValue(parent, this);
				
				parentField = parent.GetType().GetField("SceneExists");
				parentField.SetValue(parent, true);
			}
			else
			{
				parentField = parent.GetType().GetField("Scenes");
				
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
		
		if(this.GetType() == typeof( PUScene ))
		{
			gaxb_addToParent();
		}
		
		xmlns = reader.GetAttribute("xmlns");
		

		string attr;
		attr = reader.GetAttribute("adjustCamera");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr == null) { attr = "true"; }
		if(attr != null) { adjustCamera = bool.Parse(attr); adjustCameraExists = true; } 
		
		attr = reader.GetAttribute("fps");
		if(attr != null && planetOverride != null) { attr = processStringMethod.Invoke(null, new [] {_parent, attr}).ToString(); }
		if(attr != null) { fps = int.Parse(attr); fpsExists = true; } 
		

	}
	
	
	
	
	
	
	
	public override void gaxb_appendXMLAttributes(StringBuilder sb)
	{
		base.gaxb_appendXMLAttributes(sb);

		if(adjustCameraExists) { sb.AppendFormat (" {0}=\"{1}\"", "adjustCamera", adjustCamera.ToString().ToLower()); }
		if(fpsExists) { sb.AppendFormat (" {0}=\"{1}\"", "fps", fps); }

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
		
		sb.AppendFormat ("<{0}", "Scene");
		
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
			sb.AppendFormat (">{0}</{1}>", seq.ToString(), "Scene");
		}
	}
}
