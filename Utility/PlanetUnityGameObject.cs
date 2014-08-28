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

#if UNITY_EDITOR
using UnityEditor;
#endif

public delegate void Task();

public class PlanetUnityOverride {

	private static Mathos.Parser.MathParser mathParser = new Mathos.Parser.MathParser();
	public static float sceneTop;
	public static float sceneBottom;
	public static float sceneLeft;
	public static float sceneRight;
	public static float sceneScale;

	public static int puCameraLayer = 30;
	public static int puEventLayer = 29;

	public static int minFPS = 10;
	public static int maxFPS = 60;

	public static Func<string, string> xmlFromPath = (path) => {
		return PlanetUnityResourceCache.GetTextFile(path);
	};

	public static Func<PUScene, bool> orientationDidChange = (scene) => {
		PlanetUnityGameObject.currentGameObject.ReloadScene();
		return true;
	};

	//public static Func<string, string> processResourcePath = (path) => path;


	public static string processString(object o, string s)
	{
		if (s == null)
			return null;

		s.Replace("@LANGUAGE", PlanetUnityLanguage.LanguageCode());

		if (s.StartsWith ("@eval(")) {

			string evalListString = s.Substring(6, s.Length-7);

			var parts = evalListString.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
			string[] results = new string[12];
			int nresults = 0;

			if (o is GameObject) {
				// Use the size of the display
				mathParser.LocalVariables.Clear ();

				mathParser.LocalVariables.Add ("top", Convert.ToDecimal(0));
				mathParser.LocalVariables.Add ("bottom", Convert.ToDecimal(0));
				mathParser.LocalVariables.Add ("left", Convert.ToDecimal(0));
				mathParser.LocalVariables.Add ("right", Convert.ToDecimal(0));
				mathParser.LocalVariables.Add ("scale", Convert.ToDecimal(1.0f));

				mathParser.LocalVariables.Add ("scaledW", Convert.ToDecimal(Screen.width));
				mathParser.LocalVariables.Add ("scaledH", Convert.ToDecimal(Screen.height));

				mathParser.LocalVariables.Add ("w", Convert.ToDecimal(Screen.width));
				mathParser.LocalVariables.Add ("h", Convert.ToDecimal(Screen.height));
				mathParser.LocalVariables.Add ("lastY", Convert.ToDecimal(0));
				mathParser.LocalVariables.Add ("lastX", Convert.ToDecimal(0));
			}
			else if (o is PUGameObject) {
				PUGameObject entity = (PUGameObject)o;
				mathParser.LocalVariables.Clear ();

				mathParser.LocalVariables.Add ("top", Convert.ToDecimal(sceneTop));
				mathParser.LocalVariables.Add ("bottom", Convert.ToDecimal(sceneBottom));
				mathParser.LocalVariables.Add ("left", Convert.ToDecimal(sceneLeft));
				mathParser.LocalVariables.Add ("right", Convert.ToDecimal(sceneRight));
				mathParser.LocalVariables.Add ("scale", Convert.ToDecimal(sceneScale));

				mathParser.LocalVariables.Add ("scaledW", Convert.ToDecimal(sceneScale*entity.bounds.w));
				mathParser.LocalVariables.Add ("scaledH", Convert.ToDecimal(sceneScale*entity.bounds.h));

				mathParser.LocalVariables.Add ("w", Convert.ToDecimal(entity.bounds.w));
				mathParser.LocalVariables.Add ("h", Convert.ToDecimal(entity.bounds.h));
				mathParser.LocalVariables.Add ("lastY", Convert.ToDecimal(entity.lastY));
				mathParser.LocalVariables.Add ("lastX", Convert.ToDecimal(entity.lastX));
			}

			foreach (string part in parts) {
				results [nresults] = mathParser.Parse (part).ToString ();
				nresults++;
			}

			if(nresults == 4 && o is PUGameObject)
			{
				PUGameObject entity = (PUGameObject)o;
				entity.lastY = float.Parse (results [1]) + float.Parse (results [3]);
				entity.lastX = float.Parse (results [0]) + float.Parse (results [2]);
			}

			return string.Join (",", results, 0, nresults);

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

	public static float desiredFPS;
	public static void RequestFPS(float f) {
		// Called by entities to request a specific fps. PlanetUnity will set the fps dynamically
		// to the highest requested fps.
		if (f > desiredFPS) {
			desiredFPS = f;
		}
	}

	public string xmlPath;
	public Font[] fonts;

	private PUScene scene;

	private bool shouldReloadMainXML = false;

	static public PlanetUnityGameObject currentGameObject = null;

	static public Vector2 MousePosition() {
		Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

		if (currentGameObject == null || currentGameObject.scene == null)
			return pos;

		pos.x /= Screen.width;
		pos.y /= Screen.height;

		pos.x *= PlanetUnityOverride.sceneRight - PlanetUnityOverride.sceneLeft;
		pos.y *= PlanetUnityOverride.sceneBottom - PlanetUnityOverride.sceneTop;

		pos.x += PlanetUnityOverride.sceneLeft;
		pos.y += PlanetUnityOverride.sceneTop;

		return pos;
	}

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

		ReloadScene ();

		#if UNITY_EDITOR
		NotificationCenter.addObserver(this, PlanetUnity.EDITORFILEDIDCHANGE, null, (args,name) => {
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

		RemoveScene ();
	}

	void Update () {
		if (shouldReloadMainXML) {
			shouldReloadMainXML = false;
			#if UNITY_EDITOR
			ReloadScene ();
			#endif
		}

		lock (_queueLock)
		{
			if (TaskQueue.Count > 0)
				TaskQueue.Dequeue()();
		}
	}

	public void RemoveScene () {
		if (scene != null) {
			scene.performOnChildren (val => {
				MethodInfo method = val.GetType ().GetMethod ("gaxb_unload");
				if (method != null) {
					method.Invoke (val, null);
				}
				return true;
			});

			scene.gaxb_unload ();

			Destroy (scene.gameObject);
			scene = null;
		}
	}

	public PUScene Scene() {
		return scene;
	}

	public void ReloadScene () {

		RemoveScene ();

		Stopwatch sw = Stopwatch.StartNew ();

		scene = (PUScene)PlanetUnity.loadXML (PlanetUnityOverride.xmlFromPath (xmlPath), gameObject, null);

		sw.Stop ();

		UnityEngine.Debug.Log ("[" + sw.Elapsed.TotalMilliseconds + "ms] Loading scene " + xmlPath + ".xml");

		//Profile.PrintResults ();
		//Profile.Reset ();
	}

	private Queue<Task> TaskQueue = new Queue<Task>();
	private object _queueLock = new object();

	public void PrivateScheduleTask(Task newTask) {
		lock (_queueLock)
		{
			if (TaskQueue.Count < 100) {
				TaskQueue.Enqueue (newTask);
			}
		}
	}

	public bool PrivateHasTasks()
	{
		return (TaskQueue.Count > 0);
	}

	public static void ScheduleTask(Task newTask)
	{
		if (System.Object.ReferenceEquals(currentGameObject, null)) {
			return;
		}
		currentGameObject.PrivateScheduleTask(newTask);
	}

	public static bool HasTasks()
	{
		if (System.Object.ReferenceEquals(currentGameObject, null)) {
			return false;
		}
		return currentGameObject.PrivateHasTasks ();
	}
	
#if UNITY_EDITOR
	public void PrintAllBounds()
	{
		if (scene == null) {
			UnityEngine.Debug.Log ("You need to press this button while in play mode");
		} else {
			StringBuilder sb = new StringBuilder ();

			scene.performOnChildren( val => {
				PUGameObject oo = val as PUGameObject;
				if (oo != null && oo.title != null) {
					sb.AppendFormat("@{0}_BOUNDS={1}\n", oo.title.ToUpper(), oo.XmlBounds());
				}
				return true;
			});

			UnityEngine.Debug.Log ("Bounds copied to clipboard");
			EditorGUIUtility.systemCopyBuffer = sb.ToString ();
		}
	}
#endif
}

#if UNITY_EDITOR

[CustomEditor(typeof(PlanetUnityGameObject))]
public class PlanetUnityGameObjectEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PlanetUnityGameObject myScript = (PlanetUnityGameObject)target;
		if(GUILayout.Button("Copy All Bounds To Clipboard"))
		{
			myScript.PrintAllBounds();
		}
	}
}

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


