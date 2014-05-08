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

using UnityEngine;
using System.Xml;

public class PUGameObject : PUGameObjectBase {
	public GameObject gameObject;

	public new void gaxb_unload()
	{
		base.gaxb_unload ();
	}

	public void setParentGameObject(GameObject p)
	{
		gameObject.transform.parent = p.transform;
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if (gameObject == null) {
			gameObject = new GameObject ("<Entity />");

			if (titleExists) {
				gameObject.name = title;
			}
		}

		if (_parent is GameObject) {
			setParentGameObject (_parent as GameObject);
		}
		else if (_parent is PUGameObject) {
			PUGameObject parentEntity = (PUGameObject)_parent;

			setParentGameObject (parentEntity.gameObject);

			if (boundsExists) {
				bounds.y = (parentEntity.bounds.h - bounds.y) - bounds.h;
				gameObject.transform.localPosition = new Vector3 (bounds.x, bounds.y, 0.0f);
			}
		}

		if (reader != null) {
			gameObject.layer = 31;
		}

		if (hidden) {
			gameObject.SetActive (false);
		}
	}
}
