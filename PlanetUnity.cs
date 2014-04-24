

//
// Autogenerated by gaxb at 12:57:27 PM on 04/24/14
//

using System.Xml;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

interface IPlanetUnity
{
	void gaxb_load(XmlReader reader, object _parent);
	void gaxb_appendXML(StringBuilder sb);
}

public class PlanetUnity {






	static public object loadXML(string xmlString, object parentObject)
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
						Type entityClass = Type.GetType (xmlNamespace + "_" + reader.Name, true);

						object entityObject = (Activator.CreateInstance (entityClass));						
						
						MethodInfo method = entityClass.GetMethod ("gaxb_load");
						method.Invoke (entityObject, new[] { reader, rootEntity });
						
						if (reader.IsEmptyElement == false) {
							rootEntity = entityObject;
						}

						if (rootEntity == null) {
							rootEntity = entityObject;
						}
						
						if(returnEntity == null) {
							returnEntity = entityObject;
						}
					}
					catch(TypeLoadException e) {
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
						Type entityClass = Type.GetType (xmlNamespace + "_" + reader.Name, true);
						
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
					}catch(TypeLoadException e) { }
					break;
				}
			}
		}

		return returnEntity;
	}
}
