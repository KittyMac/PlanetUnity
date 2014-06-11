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
using UnityEditor;
using System.Xml;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

public class PlanetUnityOverride {

	private static Mathos.Parser.MathParser mathParser = new Mathos.Parser.MathParser();
	public static float sceneTop;
	public static float sceneBottom;
	public static float sceneScale;

	public static string processString(object o, string s)
	{
		s.Replace("@LANGUAGE", PlanetUnityLanguage.LanguageCode());

		if (s.StartsWith ("@eval(")) {

			string evalListString = s.Substring(6, s.Length-7);

			var parts = evalListString.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
			List<string> results = new List<string> ();

			if (o is PUGameObject) {
				PUGameObject entity = (PUGameObject)o;
				mathParser.LocalVariables.Clear ();

				mathParser.LocalVariables.Add ("top", Convert.ToDecimal(sceneTop));
				mathParser.LocalVariables.Add ("bottom", Convert.ToDecimal(sceneBottom));
				mathParser.LocalVariables.Add ("scale", Convert.ToDecimal(sceneScale));

				mathParser.LocalVariables.Add ("w", Convert.ToDecimal(entity.bounds.w));
				mathParser.LocalVariables.Add ("h", Convert.ToDecimal(entity.bounds.h));
				mathParser.LocalVariables.Add ("lastY", Convert.ToDecimal(entity.lastY));
				mathParser.LocalVariables.Add ("lastX", Convert.ToDecimal(entity.lastX));
			}

			foreach (string part in parts) {
				results.Add (mathParser.Parse(part).ToString());
			}

			if(results.Count == 4 && o is PUGameObject)
			{
				PUGameObject entity = (PUGameObject)o;
				entity.lastY = float.Parse (results [1]) + float.Parse (results [3]);
				entity.lastX = float.Parse (results [0]) + float.Parse (results [2]);
			}

			return string.Join(",", results.ToArray());

		} else if(s.StartsWith("@")) {

			string localizedString = PlanetUnityLanguage.Translate(s);
			if(localizedString.Equals(s) == false)
			{
				return localizedString;
			}
		}
		
		return s;
	}

}


public class PlanetUnityGameObject : MonoBehaviour {

	public string xmlPath;
	public Font[] fonts;

	private PUScene scene;

	private static FileSystemWatcher watcher;
	private bool shouldReloadMainXML = false;

	static private PlanetUnityGameObject currentGameObject = null;

	static public Font FindFontNamed(string name) {
		if (!currentGameObject)
			return null;
		foreach (Font font in currentGameObject.fonts) {
			if (font.name.Equals (name)) {
				return font;
			}
		}
		return Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
	}

	// Use this for initialization
	void Start () {

		currentGameObject = this;

		NotificationCenter.addObserver (this, "PlanetUnityReloadScene", null, "ReloadScene");

		ReloadScene ();

		#if UNITY_EDITOR
		NotificationCenter.addObserver(this, PlanetUnity.EDITORFILEDIDCHANGE, null, args => {
			string assetPath = args ["path"].ToString();
			if( assetPath.Contains(xmlPath+".xml") ||
				assetPath.EndsWith(".strings"))
			{
				PlanetUnityLanguage.ReloadAllLanguages();
				ReloadScene ();
			}
		});
		#endif
	}

	void OnDestroy () {

		NotificationCenter.removeObserver (this);

		scene.peformOnChildren(val =>
			{
				MethodInfo method = val.GetType().GetMethod ("gaxb_unload");
				if (method != null) { method.Invoke (val, null); }
			});
	}

	void Update () {
		if (shouldReloadMainXML) {
			shouldReloadMainXML = false;
			ReloadScene ();
		}
	}

	public void RemoveScene () {
		if (scene != null) {
			Destroy (scene.gameObject);
			scene = null;
		}
	}

	public void ReloadScene () {

		RemoveScene ();

		Stopwatch sw = Stopwatch.StartNew();

		TextAsset stringData = Resources.Load(xmlPath) as TextAsset;
		scene = (PUScene)PlanetUnity.loadXML(stringData.text, gameObject, null);

		sw.Stop();

		UnityEngine.Debug.Log("["+sw.Elapsed.TotalMilliseconds+"ms] Loading scene "+xmlPath+".xml");
	}
}

#if UNITY_EDITOR

public class CustomPostprocessor : AssetPostprocessor
{
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		foreach(string asset in importedAssets)
		{
			NotificationCenter.postNotification(null, PlanetUnity.EDITORFILEDIDCHANGE, NotificationCenter.Args("path", asset));
		}
	}
}

#endif


