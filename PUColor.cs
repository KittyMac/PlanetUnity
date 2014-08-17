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
using System.Collections;

public partial class PUColor : PUColorBase
{

	public static void CreateGradient (GameObject gameObject, PUGameObject puGameObject, cRect bounds, cVector2 anchor, Color a, Color b, string shader)
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
		mesh.colors = new Color[4] {Color.white, Color.white, Color.white, Color.white};
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();

		MeshFilter filter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		filter.mesh = mesh;

		if (shader == null) {
			shader = "PlanetUnity/Color";
		}

		var shaderObj = Shader.Find (puGameObject.fullShaderPath(shader));
		Material mat = new Material (shaderObj);
		gameObject.renderer.material = mat;
		gameObject.renderer.material.color = a;
	}

	public static void CreateGradient (GameObject gameObject, PUGameObject puGameObject, string meshName, cRect bounds, cVector2 anchor, Color a, Color b, string shader)
	{
		Transform model = Resources.Load<GameObject> (meshName).transform;
		Mesh mesh = null;
		foreach (Transform t in model.transform) {
			MeshFilter loadedFilter = (MeshFilter)t.GetComponent (typeof(MeshFilter));
			if (loadedFilter != null) {
				mesh = loadedFilter.mesh;
				break;
			}
		}


		mesh = (Mesh)Mesh.Instantiate(mesh);

		/*
		// Scale the mesh by the width and the height...
		Vector3[] vertices = new Vector3[mesh.vertexCount];
		for(int i = 0; i < mesh.vertexCount; i++){
			Vector3 v = mesh.vertices [i];
			v.x *= bounds.w;
			v.y *= bounds.h;
			vertices [i] = v;
		}
		mesh.vertices = vertices;*/


		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) {
			vertices [i].x *= bounds.w;
			vertices [i].y *= bounds.h;
			i++;
		}
		mesh.vertices = vertices;

		mesh.RecalculateBounds ();

		MeshFilter filter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		filter.mesh = mesh;

		if (shader == null) {
			shader = "PlanetUnity/Color";
		}

		var shaderObj = Shader.Find (puGameObject.fullShaderPath(shader));
		Material mat = new Material (shaderObj);
		gameObject.renderer.material = mat;
		gameObject.renderer.material.color = a;
	}

	public override void gaxb_load (XmlReader reader, object _parent, Hashtable args)
	{
		if(gameObject == null)
			gameObject = (GameObject)new GameObject ("<Color/>", typeof(MeshRenderer), typeof(MeshFilter));

		base.gaxb_load (reader, _parent, args);

		if (titleExists) {
			gameObject.name = title;
		}

		Color c = Color.white;

		if (colorExists) {
			c = new Color (color.r, color.g, color.b, color.a);
		}

		if (anchorExists == false) {
			anchor = new cVector2 (0, 0);
		}

		if (mesh != null) {
			PUColor.CreateGradient (gameObject, this, mesh, bounds, anchor, c, c, shader);
		} else {
			PUColor.CreateGradient (gameObject, this, bounds, anchor, c, c, shader);
		}

		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}
}
