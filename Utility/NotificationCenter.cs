using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class NotificationObserver
{
	public string name;
	public string methodName;
	public object observer;

	public void callObserver()
	{
		MethodInfo method = observer.GetType().GetMethod (methodName);
		if (method != null) {
			method.Invoke (observer, null);
		}
	}
}

public class NotificationCenter
{
	private static Dictionary<object, List<NotificationObserver>> observersByScope = new Dictionary<object, List<NotificationObserver>> ();


	public static void addObserver(object observer, string methodName, string name, object scope)
	{
		if (observer == null || name == null || scope == null) {
			UnityEngine.Debug.Log ("Warning: NotificationCenter.addObserver() called with null arguments");
			return;
		}

		NotificationObserver obv = new NotificationObserver ();
		obv.name = name;
		obv.methodName = methodName;
		obv.observer = observer;

		List<NotificationObserver> list;
		if (!observersByScope.TryGetValue(scope, out list))
		{
			list = new List<NotificationObserver>();
			observersByScope.Add(scope, list);
		}
		list.Add(obv);
	}

	public static void postNotification(object scope, string name)
	{
		List<NotificationObserver> list;
		if (observersByScope.TryGetValue(scope, out list))
		{
			foreach (NotificationObserver o in list) {
				if (o.name.Equals (name)) {
					o.callObserver ();
				}
			}
		}
	}

	public static void removeObserver(object obv)
	{
		foreach (List<NotificationObserver> list in observersByScope.Values) {
			list.RemoveAll(x => x.observer == obv);
		}
	}

	public static void removeAllObservers()
	{
		observersByScope.Clear ();
	}
}
