/* Copyright (c) 2012 Small Planet Digital, LLC
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
 * (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PUTableCell {

	public PUGameObject puGameObject = null;
	public object cellData = null;

	public virtual string XmlPath() {
		return "(subclass needs to define an XmlPath)";
	}

	public virtual void LoadIntoPUGameObject(PUGameObject parent, object data, int baseRenderQueue) {

		cellData = data;

		TextAsset stringData = Resources.Load(XmlPath ()) as TextAsset;
		puGameObject = (PUGameObject)PlanetUnity.loadXML(stringData.text, parent, NotificationCenter.Args("baseRenderQueue", baseRenderQueue));

		// Attach all of the PlanetUnity objects
		try {
			FieldInfo field = this.GetType ().GetField ("scene");
			if (field != null)
			{
				field.SetValue (this, puGameObject);
			}

			puGameObject.peformOnChildren(val =>
				{
					PUGameObject oo = val as PUGameObject;
					if(oo != null && oo.title != null)
					{
						field = this.GetType ().GetField (oo.title);
						if (field != null)
						{
							field.SetValue (this, oo);
						}
					}
				});
		}
		catch(Exception e) {
			UnityEngine.Debug.Log ("TableCell error: " + e);
		}

		try {
			// Attach all of the named GameObjects
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields) {
				if (field.FieldType == typeof(GameObject)) {

					GameObject[] pAllObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

					foreach (GameObject pObject in pAllObjects) {
						if(pObject.name.Equals(field.Name)) {
							field.SetValue(this, pObject);
						}
					}
				}
			}
		}
		catch(Exception e) {
			UnityEngine.Debug.Log ("TableCell error: " + e);
		}
	}

}

public partial class PUTable : PUTableBase {

	List<object> allObjects = null;
	List<PUTableCell> allCells = new List<PUTableCell>();


	public void SetObjectList(List<object> objects) {
		allObjects = new List<object> (objects);
	}

	public void ReloadTable() {
		GameObject content = contentGameObject();

		// TODO: run through all objects, create cells, put them in my content game object


		// 0) Remove all previous content
		for (int i = gameObject.transform.childCount - 1; i > 0; i--)
		{
			GameObject.Destroy(gameObject.transform.GetChild(i).gameObject);
		}
		for (int i = content.transform.childCount - 1; i > 0; i--)
		{
			GameObject.Destroy(content.transform.GetChild(i).gameObject);
		}

		allCells.Clear ();

		if (allObjects == null) {
			Debug.Log ("Unable to reload table contents because no data objects were specified");
			return;
		}

		// 1) Run through allObjects; instantiate a cell object based on said object class
		float y = 0;
		float maxWidth = 1;

		if (clipDepth) {
			PUGameObject.depthMaskCounter = 1;
		}

		int baseRenderQueue = scope ().getRenderQueue ();

		foreach (object row in allObjects) {
			string className = row.GetType ().Name + "TableCell";

			Type cellType = Type.GetType (className, true);

			PUTableCell cell = (Activator.CreateInstance (cellType)) as PUTableCell;
			cell.LoadIntoPUGameObject (this, row, baseRenderQueue);

			allCells.Add (cell);

			GameObject cellGO = cell.puGameObject.gameObject;
			cellGO.transform.localPosition += new Vector3(0,y,0);

			y -= cell.puGameObject.bounds.h;

			if (cell.puGameObject.bounds.w > maxWidth) {
				maxWidth = cell.puGameObject.bounds.w;
			}

			foreach (Transform trans in cellGO.GetComponentsInChildren<Transform>(true)) {
				trans.gameObject.layer = 31;
			}
		}

		if (clipDepth) {
			PUGameObject.depthMaskCounter = 0;
		}

		this.SetContentSize(new cVector2(maxWidth, Mathf.Abs(y)));
		this.gaxb_loadComplete ();
	}

}
