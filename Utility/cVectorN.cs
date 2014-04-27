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
using System.Collections.Generic;
using System.Text;

public class cVectorN {

	float[] vector;
	int length;

	public cVectorN(float[] v)
	{
		length = v.Length;

		// v1.1.6: We set length really high, because we no longer export trailing zeros
		if(length < 100)
			length = 100;
			
		vector = new float[length];

		// First, calc the length
		for (int i = 0; i < v.Length; i++) {
			vector [i] = v [i];
		}
		for (int i = v.Length; i < length; i++) {
			vector [i] = 0;
		}
	}

	public static implicit operator cVectorN(string value)
	{
		var elements = value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		float[] v = new float[elements.Length];
		int i = 0;

		foreach (string s in elements) {
			v [i] = float.Parse (s, System.Globalization.CultureInfo.InvariantCulture);
			i++;
		}

		return new cVectorN(v);
	}

	public override string ToString ()
	{
		// Should we continue?
		int lastZeroIndex = length;
		for(int i = length-1; i >= 0; i--)
		{
			lastZeroIndex = i;
			if(vector[i] != 0)
			{
				break;
			}
		}

		StringBuilder sb = new StringBuilder ();

		for(int i = 0; i <= lastZeroIndex; i++)
		{
			sb.AppendFormat ("{0},", vector[i].ToString ("0.###"));
		}

		if(sb.Length > 0)
		{
			sb.Remove (sb.Length - 1, 1);
		}

		return sb.ToString ();
	}
}
