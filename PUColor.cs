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
using System;

public class PUColor : PUColorBase
{

	public static void CreateGradient (GameObject gameObject, cRect bounds, cVector2 anchor, Color a, Color b)
	{

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[] {
			new Vector3 (bounds.w - bounds.w * anchor.x, bounds.h - bounds.h * anchor.y, 0.0f),
			new Vector3 (bounds.w - bounds.w * anchor.x, -bounds.h * anchor.y, 0.0f),
			new Vector3 (0.0f - bounds.w * anchor.x, bounds.h - bounds.h * anchor.y, 0.0f),
			new Vector3 (0.0f - bounds.w * anchor.x, -bounds.h * anchor.y, 0.0f),
		};
			
		int[] triangles = new int[] {
			0, 1, 2,
			2, 1, 3,
		};

		mesh.vertices = vertices;
		mesh.colors = new Color[4] {a, b, a, b};
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();

		MeshFilter filter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		filter.mesh = mesh;

		var shaderObj = Shader.Find ("Custom/Unlit/SolidColor");
		Material mat = new Material (shaderObj);
		gameObject.renderer.material = mat;
		gameObject.renderer.material.color = Color.white;

	}

	public new void gaxb_load (XmlReader reader, object _parent)
	{
		if(gameObject == null)
			gameObject = (GameObject)new GameObject ("<Color/>", typeof(MeshRenderer), typeof(MeshFilter));

		base.gaxb_load (reader, _parent);

		if (reader == null && _parent == null)
			return;

		if (titleExists) {
			gameObject.name = title;
		}

		Color c = new Color (color.r, color.g, color.b, color.a);
		PUColor.CreateGradient (gameObject, bounds, anchor, c, c);

		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}
}
