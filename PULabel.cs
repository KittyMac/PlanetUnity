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
	public CCText text;
	public CCFont ccfont;

	public new void gaxb_load(XmlReader reader, object _parent)
	{
		base.gaxb_load(reader, _parent);

		if (titleExists == false) {
			gameObject.name = "\""+value+"\"";
		}

		text = gameObject.AddComponent(typeof(CCText)) as CCText;

		ccfont = Resources.Load ("bmfonts/"+font, typeof(CCFont)) as CCFont;

		if (ccfont == null) {
			UnityEngine.Debug.Log ("Unable to find bmfont " + font);
			return;
		}

		text.Font = ccfont;
		text.Text = value;

		var tex = (Texture) Resources.Load ("bmfonts/"+font, typeof(Texture));
		text.renderer.material.mainTexture = tex;


		if (shaderExists == false) {
			shader = "Custom/Unlit/NoDepth";
		}

		var shaderObj = Shader.Find(shader);

		text.renderer.material.color = new Color (1, 1, 1, 1);
		text.renderer.material.shader = shaderObj;
		text.renderer.material.renderQueue = scope().getRenderQueue()+renderQueueOffset;

		float pxScale = ccfont.pixelScale;

		text.Width = bounds.w * pxScale;
		text.LineHeight = 0.8f;
		text.Bounding = CCText.BoundingMode.Margin;

		if(this.alignment == PlanetUnity.LabelAlignment.left)
			text.Alignment = CCText.AlignmentMode.Left;
		if(this.alignment == PlanetUnity.LabelAlignment.center)
			text.Alignment = CCText.AlignmentMode.Center;
		if(this.alignment == PlanetUnity.LabelAlignment.right)
			text.Alignment = CCText.AlignmentMode.Right;
		if(this.alignment == PlanetUnity.LabelAlignment.justify)
			text.Alignment = CCText.AlignmentMode.Justify;

		text.VerticalAnchor = CCText.VerticalAnchorMode.Middle;
		text.HorizontalAnchor = CCText.HorizontalAnchorMode.Center;

		gameObject.transform.localPosition += new Vector3(bounds.w/2, bounds.h/2, 0);
		gameObject.transform.localScale = new Vector3 (1.0f/pxScale, 1.0f/pxScale, 1.0f);
	}
}
