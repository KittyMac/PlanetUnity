

//
// Autogenerated by gaxb ( https://github.com/SmallPlanet/gaxb )
//

using System.Xml;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;

interface IPlanetUnity
{
	void gaxb_load(XmlReader reader, object _parent, Hashtable args);
	void gaxb_appendXML(StringBuilder sb);
}

public class PlanetUnity {
	public int baseRenderQueue = 0;

	public enum LabelAlignment {
		left,
		center,
		right,
	};

	public enum ScrollDirection {
		horizontal,
		vertical,
		both,
	};

	public const string USERSTRINGINPUT = "UserStringInput";
	public const string USERCHARINPUT = "UserCharInput";
	public const string USERINPUTCANCELLED = "UserInputCancelled";
	public const string EVENTWITHUNREGISTEREDCOLLIDER = "EventWithUnregisteredCollider";
	public const string EVENTWITHNOCOLLIDER = "EventWithNoCollider";
	public const string EDITORFILEDIDCHANGE = "EditorFileDidChange";



	static public string ConvertClassName(string xmlNamespace, string name)
	{
		return Regex.Replace(xmlNamespace, "[^A-Z]", "")+name;
	}
	
	static public string writeXML(object root) {
		StringBuilder sb = new StringBuilder ();
		MethodInfo mInfo = root.GetType().GetMethod("gaxb_appendXML");
		if(mInfo != null) {
			mInfo.Invoke (root, new[] { sb });
		}
		return sb.ToString();
	}

	static public object loadXML(string xmlString, object parentObject, Hashtable args)
	{
		object rootEntity = parentObject;
		object returnEntity = null;
		string xmlNamespace;

		// Create an XmlReader
		using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(xmlString)))
		{
			// Parse the file and display each of the nodes.
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					xmlNamespace = Path.GetFileName (reader.NamespaceURI);
					try
					{
						Type entityClass = Type.GetType (ConvertClassName(xmlNamespace, reader.Name), true);

						object entityObject = (Activator.CreateInstance (entityClass));						
						
						MethodInfo method = entityClass.GetMethod ("gaxb_load");
						method.Invoke (entityObject, new[] { reader, rootEntity, args });
						
						if (reader.IsEmptyElement == false) {
							rootEntity = entityObject;
						} else {
							method = entityClass.GetMethod ("gaxb_loadComplete");
							if(method != null) { method.Invoke (entityObject, null); }
						}

						if (rootEntity == null) {
							rootEntity = entityObject;
						}
						
						if(returnEntity == null) {
							returnEntity = entityObject;
						}
					}
					catch(TypeLoadException) {
						if (rootEntity != null) {
							// If we get here, this is not a unique object but perhaps a field on the parent...
							string valueName = reader.Name;
							if(rootEntity.GetType ().GetField (valueName) != null)
							{
								reader.Read();
								if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
								{
									rootEntity.GetType ().GetField (valueName).SetValue (rootEntity, reader.Value);
									rootEntity.GetType ().GetField (valueName+"Exists").SetValue (rootEntity, true);
								}
							}
							else
							{
								reader.Read();
								if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue)) {
									List<object> parentChildren = (List<object>)(rootEntity.GetType ().GetField (valueName + "s").GetValue (rootEntity));
									if (parentChildren != null) {
										parentChildren.Add (reader.Value);
									}
								}
							}
						}
					}

					break;
				case XmlNodeType.Text:
					break;
				case XmlNodeType.XmlDeclaration:
				case XmlNodeType.ProcessingInstruction:
					break;
				case XmlNodeType.Comment:
					break;
				case XmlNodeType.EndElement:
					try{
						xmlNamespace = Path.GetFileName (reader.NamespaceURI);
						Type entityClass = Type.GetType (ConvertClassName(xmlNamespace, reader.Name), true);
						
						MethodInfo method = entityClass.GetMethod ("gaxb_loadComplete");
						if(method != null) { method.Invoke (rootEntity, null); }

						if(entityClass != null)
						{
							object parent = rootEntity.GetType().GetField("parent").GetValue(rootEntity);
							if(parent != null)
							{
								rootEntity = parent;
							}
						}
					}catch(TypeLoadException) { }
					break;
				}
			}
		}

		return returnEntity;
	}
}
