
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

			if (o is PlanetUnity_Entity) {
				PlanetUnity_Entity entity = (PlanetUnity_Entity)o;
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

			if(results.Count == 4 && o is PlanetUnity_Entity)
			{
				PlanetUnity_Entity entity = (PlanetUnity_Entity)o;
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

	private PlanetUnity_Scene scene;

	private static FileSystemWatcher watcher;
	private bool shouldReloadMainXML = false;

	// Use this for initialization
	void Start () {
		ReloadScene ();

		#if UNITY_EDITOR
		beginWatchingFile ();
		#endif
	}

	void OnDestroy () {
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

	void RemoveScene () {
		if (scene != null) {
			Destroy (scene.gameObject);
			scene = null;
		}
	}

	void ReloadScene () {

		RemoveScene ();

		Stopwatch sw = Stopwatch.StartNew();

		// In the editor, pull directly from the file system otherwise we get a cached version
		#if UNITY_EDITOR
		string basePath = Path.GetFullPath("Assets/Resources");
		string xmlString = System.IO.File.ReadAllText(basePath+"/"+xmlPath+".xml");
		scene = (PlanetUnity_Scene)PlanetUnity.loadXML(xmlString, null);
		#else
		TextAsset stringData = Resources.Load(xmlPath) as TextAsset;
		scene = (PlanetUnity_Scene)PlanetUnity.loadXML(stringData.text, null);
		#endif

		sw.Stop();

		UnityEngine.Debug.Log("["+sw.Elapsed.TotalMilliseconds+"ms] Loading scene "+xmlPath+".xml");
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
