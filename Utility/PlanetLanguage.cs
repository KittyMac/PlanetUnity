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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;


public class PlanetLanguage
{
	// TODO: This should be updated with more commonly used language codes, since Unity does not allow us to 
	// get a directory listing of stuff in the Resources folder.  Having extras in here will not hurt anything.
	static string[] languageCodes = new string[] { "en", "de", "es", "ja", "ru", "zh" };
	static Dictionary<string,Dictionary<string,string>> allLanguages = new Dictionary<string,Dictionary<string,string>>();

	static public void reload()
	{
		allLanguages.Clear ();

		foreach (string code in languageCodes) {
			#if UNITY_EDITOR
			string basePath = Path.GetFullPath("Assets/Resources");
			string stringsFile = System.IO.File.ReadAllText(basePath+"/languages/"+code+"/Localizable.strings");
			#else
			TextAsset stringData = Resources.Load("languages/"+code+"/Localizable") as TextAsset;
			string stringsFile = stringData.text;
			#endif

			Dictionary<string,string> currentLanguage = new Dictionary<string,string> ();
			MatchCollection matches = Regex.Matches(stringsFile, "\"([^\"]+)\"\\s*=\\s*\"([^\"]+)\"");
			foreach (Match match in matches)
			{
				currentLanguage.Add (match.Groups [1].Value, match.Groups [2].Value);
			}
			allLanguages.Add (code, currentLanguage);
		}
	}

	static public string translate(string key, string code)
	{
		Dictionary<string,string> languageDict;
		if(!allLanguages.TryGetValue(code, out languageDict))
		{
			if(code.Equals("en"))
				return key;
			return translate(key, "en");
		}

		string value = languageDict[key];
		if(value == null)
		{
			if(code.Equals("en"))
				return key;
			return translate(key, "en");
		}

		return value;
	}

	static public string translate(string key)
	{
		return translate (key, Application.systemLanguage.ToString());
	}
}
