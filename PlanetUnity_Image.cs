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

public class PlanetUnity_Image : PlanetUnity_ImageBase {
	private Mesh CreateMesh() {

		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[]
		{
			/*
			new Vector3( bounds.w*0.5f, 	bounds.h*0.5f, 		0),
			new Vector3( bounds.w*0.5f, 	bounds.h*-0.5f,   	0),
			new Vector3( bounds.w*-0.5f,	bounds.h*0.5f, 		0),
			new Vector3( bounds.w*-0.5f,   	bounds.h*-0.5f,   	0),
			*/
			new Vector3( bounds.w, 	bounds.h, 		0),
			new Vector3( bounds.w, 	0.0f,		   	0),
			new Vector3( 0.0f,		bounds.h, 		0),
			new Vector3( 0.0f,   	0.0f,		   	0),
		};

		Vector2[] uv = new Vector2[]
		{
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(0, 0),
		};

		int[] triangles = new int[]
		{
			0, 1, 2,
			2, 1, 3,
		};

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;
	}

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		// Create our specific GameObject, set any defaults
		gameObject = (GameObject) new GameObject("<Image/>", typeof(MeshRenderer), typeof(MeshFilter));

		shader = "Somian/Unlit/Transparent";

		base.gaxb_load(reader, _parent);

		if (titleExists) {
			gameObject.name = title;
		}

		MeshFilter filter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		filter.mesh = CreateMesh();

		// Set texture
		Texture2D tex = (Texture2D) Resources.Load (resourcePath);
		tex.filterMode = FilterMode.Bilinear;


		gameObject.renderer.material.mainTexture = tex;

		var shaderObj = Shader.Find(shader);
		gameObject.renderer.material.color = new Color (1, 1, 1, 1);
		gameObject.renderer.material.shader = shaderObj;
		gameObject.renderer.material.renderQueue = scope().getRenderQueue();
	}
}
