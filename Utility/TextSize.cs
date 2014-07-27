using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

/**
 
 TextSize for Unity3D by thienhaflash (thienhaflash@gmail.com)
 
 Version	: 0.1
 Update		: 18.Jun.2012
 Features	:
	Return perfect size for any TextMesh
 	Cache the size of each character to speed up the size
	Evaluate and cache only when there are requirements
 
 Sample 	:
		
		//declare it locally, so we can have access anywhere from the script
		TextSize ts;
		
		//put this on the Start function
	 	ts = new TextSize(gameObject.GetComponent<TextMesh>());
		
		//anywhere, after you change the text :
		print(ts.width);
		
		//or get the length of an abitrary text (that is not assign to TextMesh)
		print(ts.GetTextWidth("any abitrary text goes here"));

 You are free to use the code or modify it any way you want (even remove this block of comments) but if it's good
 please give it back to the community.
 
 */

public class TextSize {
	private Hashtable dict; //map character -> width

	private TextMesh textMesh;
	private Renderer renderer;

	public TextSize(TextMesh tm){
		textMesh = tm;
		renderer = tm.renderer;
		dict = new Hashtable();
		getSpace();
	}

	private void getSpace(){//the space can not be got alone
		string oldText = textMesh.text;

		textMesh.text = "a";
		float aw = renderer.bounds.size.x;
		textMesh.text = "a a";
		float cw = renderer.bounds.size.x - 2* aw;

		dict.Add(' ', cw);
		dict.Add('a', aw);

		textMesh.text = oldText;
	}

	public float GetTextWidth(string s) {
		char[] charList = s.ToCharArray();
		float w = 0;
		char c;
		string oldText = textMesh.text;

		for (int i=0; i<charList.Length; i++){
			c = charList[i];

			if (dict.ContainsKey(c)){
				w += (float)dict[c];
			} else {
				textMesh.text = ""+c;
				float cw = renderer.bounds.size.x;
				dict.Add(c, cw);
				w += cw;
				//MonoBehaviour.print("char<" + c +"> " + cw);
			}
		}

		textMesh.text = oldText;
		return w;
	}

	public float width { get { return GetTextWidth(textMesh.text); } }
	public float height { get { return renderer.bounds.size.y; } }

	public void FitToWidth(float wantedWidth) {
		string originalString = textMesh.text;

		//originalString = Regex.Replace(originalString, "<[^>]*>", "");

		string[] words = originalString.Split(' ');

		StringBuilder sb = new StringBuilder ();
		StringBuilder sb2 = new StringBuilder ();
		float stringWidth;

		// 1) break on words
		foreach (string word in words) {
			int idx = sb.Length;
			sb.Append (word);

			textMesh.text = sb.ToString();
			stringWidth = renderer.bounds.size.x;

			if (stringWidth > wantedWidth) {
				sb2.Append (sb.ToString ().Substring (0, idx));
				sb2.Append ("\n");

				sb.Length = 0;
				sb.Append (word);
			}
			sb.Append (" ");
		}

		sb2.Append (sb.ToString ());

		// 2) break on chars
		textMesh.text = sb2.ToString();
		stringWidth = renderer.bounds.size.x;
		if (stringWidth > wantedWidth) {
			char[] chars = originalString.ToCharArray();

			textMesh.text = "";
			sb.Length = 0;
			sb2.Length = 0;

			foreach (char word in chars) {
				int idx = sb.Length;
				sb.Append (word);

				textMesh.text = sb.ToString();
				stringWidth = renderer.bounds.size.x;

				if (stringWidth > wantedWidth) {
					sb2.Append (sb.ToString ().Substring (0, idx));
					sb2.Append ("\n");

					sb.Length = 0;
					sb.Append (word);
				}
				sb.Append (" ");
			}

			sb2.Append (sb.ToString ());
			textMesh.text = sb2.ToString();
		}

	}

}