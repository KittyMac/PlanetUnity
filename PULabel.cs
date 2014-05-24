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

public partial class PULabel : PULabelBase {

	public TextSize ts;
	public TextMesh textMesh;

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

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
			if (clips) {
				shader = "Custom/Unlit/FontWithDepthMask";
			} else {
				shader = "Custom/Unlit/Font";
			}
		}

		MeshRenderer meshRendererComponent = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		var shaderObj = Shader.Find (shader);
		Material mat = new Material (shaderObj);
		mat.mainTexture = textMesh.font.material.mainTexture;
		mat.mainTexture.filterMode = FilterMode.Bilinear;
		meshRendererComponent.materials = new Material[] { mat };

		ts.FitToWidth (bounds.w);

		if (clips) {
			// We need to create a Color to render the DepthMask shader to do depth-based culling
			PUColor depthMask1 = new PUColor("Custom/Unlit/DepthMask", new cColor(0,0,0,1), new cVector2 (0, 1), bounds);
			depthMask1.SetTitle ("Depth Mask 1");
			depthMask1.loadIntoPUGameObject (this);

			// Fix positioning manually to match the label
			if (this.alignment == PlanetUnity.LabelAlignment.center) {
				depthMask1.gameObject.transform.localPosition += new Vector3 (-bounds.w / 2, 0, 0);
			}
			if (this.alignment == PlanetUnity.LabelAlignment.right) {
				depthMask1.gameObject.transform.localPosition += new Vector3 (-bounds.w, 0, 0);
			}

			gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;

			PUColor depthMask2 = new PUColor("Custom/Unlit/DepthMaskClear", new cColor(0,0,0,1), new cVector2 (0, 1), bounds);
			depthMask2.SetTitle ("Depth Mask 2");
			depthMask2.loadIntoPUGameObject (this);

			// Fix positioning manually to match the label
			if (this.alignment == PlanetUnity.LabelAlignment.center) {
				depthMask2.gameObject.transform.localPosition += new Vector3 (-bounds.w / 2, 0, 0);
			}
			if (this.alignment == PlanetUnity.LabelAlignment.right) {
				depthMask2.gameObject.transform.localPosition += new Vector3 (-bounds.w, 0, 0);
			}
		}
	}
}
