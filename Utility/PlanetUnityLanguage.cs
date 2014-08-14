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


public class PlanetUnityLanguage
{
	private static Dictionary<string,Dictionary<string,string>> allLanguages = new Dictionary<string,Dictionary<string,string>>();
	private static Dictionary<string, string> languageToCode = null;
	private static string languageCode = null;

	static public string LanguageCode()
	{
		if(languageCode == null) {
			languageCode = Application.systemLanguage.ToString ();
		}
		return languageCode;
	}

	static public void CreateLanguageCodeMapping()
	{
		if(languageToCode == null)
		{
			languageToCode = new Dictionary<string, string>();

			languageToCode.Add(SystemLanguage.English.ToString(), "en");
			languageToCode.Add(SystemLanguage.Afrikaans.ToString(), "af");
			languageToCode.Add(SystemLanguage.Arabic.ToString(), "ar");
			languageToCode.Add(SystemLanguage.Basque.ToString(), "eu");
			languageToCode.Add(SystemLanguage.Belarusian.ToString(), "be");
			languageToCode.Add(SystemLanguage.Bulgarian.ToString(), "bg");
			languageToCode.Add(SystemLanguage.Catalan.ToString(), "ca");
			languageToCode.Add(SystemLanguage.Chinese.ToString(), "zh");
			languageToCode.Add(SystemLanguage.Czech.ToString(), "cs");
			languageToCode.Add(SystemLanguage.Danish.ToString(), "da");
			languageToCode.Add(SystemLanguage.Dutch.ToString(), "nl");
			languageToCode.Add(SystemLanguage.Estonian.ToString(), "et");
			languageToCode.Add(SystemLanguage.Faroese.ToString(), "fo");
			languageToCode.Add(SystemLanguage.Finnish.ToString(), "fi");
			languageToCode.Add(SystemLanguage.French.ToString(), "fr");
			languageToCode.Add(SystemLanguage.German.ToString(), "de");
			languageToCode.Add(SystemLanguage.Greek.ToString(), "el");
			languageToCode.Add(SystemLanguage.Hebrew.ToString(), "he");
			languageToCode.Add(SystemLanguage.Icelandic.ToString(), "is");
			languageToCode.Add(SystemLanguage.Indonesian.ToString(), "id");
			languageToCode.Add(SystemLanguage.Italian.ToString(), "it");
			languageToCode.Add(SystemLanguage.Japanese.ToString(), "ja");
			languageToCode.Add(SystemLanguage.Korean.ToString(), "ko");
			languageToCode.Add(SystemLanguage.Latvian.ToString(), "lv");
			languageToCode.Add(SystemLanguage.Lithuanian.ToString(), "lt");
			languageToCode.Add(SystemLanguage.Norwegian.ToString(), "no");
			languageToCode.Add(SystemLanguage.Polish.ToString(), "pl");
			languageToCode.Add(SystemLanguage.Portuguese.ToString(), "pt");
			languageToCode.Add(SystemLanguage.Romanian.ToString(), "ro");
			languageToCode.Add(SystemLanguage.Russian.ToString(), "ru");
			languageToCode.Add(SystemLanguage.SerboCroatian.ToString(), "hr");
			languageToCode.Add(SystemLanguage.Slovak.ToString(), "sk");
			languageToCode.Add(SystemLanguage.Slovenian.ToString(), "sl");
			languageToCode.Add(SystemLanguage.Spanish.ToString(), "es");
			languageToCode.Add(SystemLanguage.Swedish.ToString(), "sv");
			languageToCode.Add(SystemLanguage.Thai.ToString(), "th");
			languageToCode.Add(SystemLanguage.Turkish.ToString(), "tr");
			languageToCode.Add(SystemLanguage.Ukrainian.ToString(), "uk");
			languageToCode.Add(SystemLanguage.Vietnamese.ToString(), "vi");
			languageToCode.Add(SystemLanguage.Hungarian.ToString(), "hu");
			languageToCode.Add(SystemLanguage.Unknown.ToString(), "en");
		}
	}

	static public void ReloadAllLanguages()
	{
		allLanguages.Clear ();
	}

	static public void VerifyLanguageCode(string code)
	{
		Dictionary<string,string> languageDict;

		CreateLanguageCodeMapping ();

		try {
			code = languageToCode [code];
		} catch {}

		if (!allLanguages.TryGetValue (code, out languageDict)) {
			string stringsFile = PlanetUnityResourceCache.GetTextFile ("languages/" + code + "/Localizable.strings");
			if (stringsFile == null) {
				// default to English in cases where
				stringsFile = PlanetUnityResourceCache.GetTextFile ("languages/en/Localizable.strings");
			}

			Dictionary<string,string> currentLanguage = new Dictionary<string,string> ();
			MatchCollection matches = Regex.Matches (stringsFile, "\"([^\"]+)\"\\s*=\\s*\"([^\"]+)\"");
			foreach (Match match in matches) {
				currentLanguage.Add (match.Groups [1].Value, match.Groups [2].Value);
			}
			allLanguages.Add (code, currentLanguage);

			Debug.Log ("Loading language strings for " + code);
		}
	}

	static public string Translate(string key, string code)
	{
		VerifyLanguageCode (code);

		Dictionary<string,string> languageDict;
		if(!allLanguages.TryGetValue(code, out languageDict))
		{
			if(code.Equals("en"))
				return key;
			return Translate(key, "en");
		}

		try{
			string value = languageDict[key];
			if(value == null)
			{
				if(code.Equals("en"))
					return key;
				return Translate(key, "en");
			}
			return value;
		}catch{
			Debug.Log ("Language key " + key + " does not exist");
		}

		return key;
	}

	static public string Translate(string key)
	{
		if (languageCode == null) {

		}
		return Translate (key, languageCode);
	}
}
