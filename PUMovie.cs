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
using System.Collections;

public partial class PUMovie : PUMovieBase {

	protected Mesh CreateMesh ()
	{

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[] {
			new Vector3 (bounds.w - bounds.w * anchor.x, bounds.h - bounds.h * anchor.y, 0.0f),
			new Vector3 (bounds.w - bounds.w * anchor.x, -bounds.h * anchor.y, 0.0f),
			new Vector3 (0.0f - bounds.w * anchor.x, bounds.h - bounds.h * anchor.y, 0.0f),
			new Vector3 (0.0f - bounds.w * anchor.x, -bounds.h * anchor.y, 0.0f),
		};

		Vector2[] uv = new Vector2[] {
			new Vector2 (1, 1),
			new Vector2 (1, 0),
			new Vector2 (0, 1),
			new Vector2 (0, 0),
		};

		int[] triangles = new int[] {
			0, 1, 2,
			2, 1, 3,
		};

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();

		return mesh;
	}

	public override void UpdateGeometry()
	{
		base.UpdateGeometry ();

		CreateGeometry ();
	}

	private void CreateGeometry()
	{
		MeshFilter filter = (MeshFilter)gameObject.GetComponent (typeof(MeshFilter));
		filter.sharedMesh = CreateMesh();
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		// Create our specific GameObject, set any defaults
		gameObject = (GameObject) new GameObject("<Movie/>", typeof(MeshRenderer), typeof(MeshFilter));

		base.gaxb_load(reader, _parent, args);

		if (shaderExists == false) {
			if (hasAlpha) {
				shader = "PlanetUnity/AlphaMovie";
			} else {
				shader = "PlanetUnity/Movie";
			}
		}

		if (titleExists) {
			gameObject.name = title;
		}

		CreateGeometry ();


		// Why, oh why are movie textures not supported in iOS?
		#if (UNITY_IOS || UNITY_ANDROID)

		#else
		// Set texture
		MovieTexture tex = Resources.Load (resourcePath) as MovieTexture;
		if (tex != null) {
			tex.filterMode = FilterMode.Bilinear;
			gameObject.renderer.material.mainTexture = tex;
		}
		#endif

		var shaderObj = Shader.Find(fullShaderPath(shader));
		if (colorExists) {
			gameObject.renderer.material.color = new Color (color.r, color.g, color.b, color.a);
		} else {
			gameObject.renderer.material.color = new Color (1, 1, 1, 1);
		}
		gameObject.renderer.material.shader = shaderObj;
		gameObject.renderer.material.renderQueue = scope().getRenderQueue()+renderQueueOffset;

		#if UNITY_IOS || UNITY_ANDROID

		#else
		tex.Play ();
		tex.loop = looping;
		#endif
	}

	public void LoadImageResource(string path)
	{
		#if UNITY_IOS || UNITY_ANDROID

		#else
		MovieTexture tex = Resources.Load (resourcePath) as MovieTexture;
		if (tex != null) {
			tex.filterMode = FilterMode.Bilinear;
			gameObject.renderer.material.mainTexture = tex;
		}
		#endif
	}
}
