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
using System;
using System.Globalization;

public class cColor {

	public float r, g, b, a;

	public cColor(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator cColor(string value)
	{
		if (value.StartsWith ("#")) {
			int argb = Int32.Parse(value.Substring(1), NumberStyles.HexNumber);
			return new cColor (
				(float)((argb & 0xFF000000) >> 24) / 255.0f,
				(float)((argb & 0x00FF0000) >> 16) / 255.0f,
				(float)((argb & 0x0000FF00) >> 8) / 255.0f,
				(float)((argb & 0x000000FF) >> 0) / 255.0f);
		}

		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		return new cColor(
			float.Parse(elements[0], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[1], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[2], System.Globalization.CultureInfo.InvariantCulture),
			float.Parse(elements[3], System.Globalization.CultureInfo.InvariantCulture));
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1},{2},{3}", r, g, b, a);
	}
}
