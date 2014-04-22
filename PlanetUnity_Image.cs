
using UnityEngine;
using System.Xml;
using System;

public class PlanetUnity_Image : PlanetUnity_ImageBase {
	private Mesh CreateMesh() {

		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[]
		{
			new Vector3( bounds.w*0.5f, 	bounds.h*0.5f, 		0),
			new Vector3( bounds.w*0.5f, 	bounds.h*-0.5f,   	0),
			new Vector3( bounds.w*-0.5f,	bounds.h*0.5f, 		0),
			new Vector3( bounds.w*-0.5f,   	bounds.h*-0.5f,   	0),
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

		// Set position
		gameObject.transform.position = new Vector3(bounds.x+bounds.w/2, bounds.y+bounds.h/2, 1);
		gameObject.transform.parent = gameObject.transform;

	}
}
