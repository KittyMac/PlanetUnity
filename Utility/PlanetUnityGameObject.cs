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
		}

		return s;
	}

}


public class PlanetUnityGameObject : MonoBehaviour {

	public string xmlPath;

	private PUScene scene;

	private static FileSystemWatcher watcher;
	private bool shouldReloadMainXML = false;

	// Use this for initialization
	void Start () {

		NotificationCenter.addObserver (this, "ReloadScene", "PlanetUnityReloadScene", null);

		ReloadScene ();

		#if UNITY_EDITOR
		beginWatchingFile ();
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

		try {
			Stopwatch sw = Stopwatch.StartNew();

			// In the editor, pull directly from the file system otherwise we get a cached version
			#if UNITY_EDITOR
			string basePath = Path.GetFullPath("Assets/Resources");
			string xmlString = System.IO.File.ReadAllText(basePath+"/"+xmlPath+".xml");
			scene = (PUScene)PlanetUnity.loadXML(xmlString, gameObject);
			#else
			TextAsset stringData = Resources.Load(xmlPath) as TextAsset;
			scene = (PUScene)PlanetUnity.loadXML(stringData.text, gameObject);
			#endif

			sw.Stop();

			UnityEngine.Debug.Log("["+sw.Elapsed.TotalMilliseconds+"ms] Loading scene "+xmlPath+".xml");
		}
		catch(Exception) {
			UnityEngine.Debug.Log ("Unable to load Planet Unity XML " + xmlPath);
		}
	}

	// ************************ EDITOR ONLY *******************************
	#if UNITY_EDITOR
	private void beginWatchingFile()
	{
		string basePath = Path.GetFullPath("Assets/Resources");

		watcher = new FileSystemWatcher ();
		watcher.Path = basePath+"/"+Path.GetDirectoryName(xmlPath);
		watcher.Filter = Path.GetFileName(xmlPath)+".xml";
		watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName 
			| NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite 
			| NotifyFilters.Size;
		watcher.Changed += new FileSystemEventHandler (OnAssetFileWatcherChanged);
		watcher.EnableRaisingEvents = true;
	}

	void OnAssetFileWatcherChanged(object sender, FileSystemEventArgs e)
	{
		shouldReloadMainXML = true;
	}
	#endif
}
