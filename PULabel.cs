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

public class PULabel : PULabelBase {

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
			shader = "Custom/Unlit/Font";
		}

		MeshRenderer meshRendererComponent = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		var shaderObj = Shader.Find (shader);
		Material mat = new Material (shaderObj);
		mat.mainTexture = textMesh.font.material.mainTexture;
		mat.mainTexture.filterMode = FilterMode.Bilinear;
		meshRendererComponent.materials = new Material[] { mat };

		ts.FitToWidth (bounds.w);
		
		// We need to create a Color to render the DepthMask shader to do depth-based culling
		PUColor depthMask1 = new PUColor ();
		depthMask1.title = "Depth Mask 1";
		depthMask1.titleExists = true;
		depthMask1.anchor = new cVector2 (0, 1);
		depthMask1.anchorExists = true;
		depthMask1.bounds = bounds;
		depthMask1.shader = "Custom/Unlit/DepthMask";
		depthMask1.gaxb_load (null, null);
		depthMask1.gameObject.transform.parent = gameObject.transform;

		depthMask1.gameObject.transform.localPosition = Vector3.zero;
		if (this.alignment == PlanetUnity.LabelAlignment.center) {
			depthMask1.gameObject.transform.localPosition += new Vector3(-bounds.w/2, 0, 0);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.right) {
			depthMask1.gameObject.transform.localPosition += new Vector3(-bounds.w, 0, 0);
		}
		depthMask1.gameObject.transform.localRotation = Quaternion.identity;
		depthMask1.gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;


		gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;

		PUColor depthMask2 = new PUColor ();
		depthMask2.title = "Depth Mask 2";
		depthMask2.titleExists = true;
		depthMask2.anchor = new cVector2 (0, 1);
		depthMask2.anchorExists = true;
		depthMask2.bounds = bounds;
		depthMask2.shader = "Custom/Unlit/DepthMaskClear";
		depthMask2.gaxb_load (null, null);
		depthMask2.gameObject.transform.parent = gameObject.transform;

		depthMask2.gameObject.transform.localPosition = Vector3.zero;
		if (this.alignment == PlanetUnity.LabelAlignment.center) {
			depthMask2.gameObject.transform.localPosition += new Vector3(-bounds.w/2, 0, 0);
		}
		if (this.alignment == PlanetUnity.LabelAlignment.right) {
			depthMask2.gameObject.transform.localPosition += new Vector3(-bounds.w, 0, 0);
		}
		depthMask2.gameObject.transform.localRotation = Quaternion.identity;
		depthMask2.gameObject.renderer.material.renderQueue = scope ().getRenderQueue () + renderQueueOffset;
	}
}
