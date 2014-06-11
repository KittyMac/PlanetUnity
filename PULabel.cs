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

public partial class PULabel : PULabelBase {

	public TextSize ts;
	public TextMesh textMesh;

	public override Vector3 maskOffset() 
	{
		// Fix positioning manually to match the label
		if (this.alignment == PlanetUnity.LabelAlignment.center) {
			return new Vector3 (-bounds.w / 2, -bounds.h, 0);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.right) {
			return new Vector3 (-bounds.w, -bounds.h, 0);
		}

		return Vector3.zero;
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (titleExists == false) {
			gameObject.name = "\""+value+"\"";
		}
		
		gameObject.AddComponent("TextMesh");

		textMesh = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;
		ts = new TextSize(gameObject.GetComponent<TextMesh>());

		textMesh.font = PlanetUnityGameObject.FindFontNamed(font);
		textMesh.text = value;

		textMesh.color = new Color (textColor.r, textColor.g, textColor.b, textColor.a);

		// magic numbers explained here: http://answers.unity3d.com/questions/55433/textmesh-charactersize-vs-fontsize.html
		textMesh.characterSize = fontSize*10.0f/(fontSize*2);
		textMesh.fontSize = (fontSize*2);

		textMesh.lineSpacing = 0.86f;

		if (this.alignment == PlanetUnity.LabelAlignment.left) {
			textMesh.alignment = TextAlignment.Left;
			textMesh.anchor = TextAnchor.UpperLeft;
			gameObject.transform.localPosition += new Vector3(0, bounds.h, 0);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.center) {
			textMesh.alignment = TextAlignment.Center;
			textMesh.anchor = TextAnchor.UpperCenter;
			gameObject.transform.localPosition += new Vector3(bounds.w/2, bounds.h, 0);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.right) {
			textMesh.alignment = TextAlignment.Right;
			textMesh.anchor = TextAnchor.UpperRight;
			gameObject.transform.localPosition += new Vector3(bounds.w, bounds.h, 0);
		}
		
		if (shaderExists == false) {
			shader = "PlanetUnity/Label";
		}

		MeshRenderer meshRendererComponent = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		var shaderObj = Shader.Find (fullShaderPath (shader));
		Material mat = new Material (shaderObj);
		mat.mainTexture = textMesh.font.material.mainTexture;
		mat.mainTexture.filterMode = FilterMode.Bilinear;
		meshRendererComponent.materials = new Material[] { mat };

		ts.FitToWidth (bounds.w);

		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}

	public void LoadTextString(string value)
	{
		textMesh.text = value;

		ts.FitToWidth (bounds.w);
	}
}
