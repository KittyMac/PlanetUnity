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

using System.Xml;
using System;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public partial class PUObject : PUObjectBase {

	private int renderQeueuCount = 0;
	public int getRenderQueue()
	{
		renderQeueuCount += 10;
		return renderQeueuCount;
	}

	public void clearRenderQueue()
	{
		renderQeueuCount = 0;
	}

	public void reclaimRenderQueues()
	{
		int maxRenderQueue = 0;

		performOnChildren (val => {
			PUGameObject oo = val as PUGameObject;
			if(oo.gameObject != null && oo.gameObject.renderer && oo.gameObject.renderer.material) {
				int r = oo.gameObject.renderer.material.renderQueue - oo.renderQueueOffset;
				if( r > maxRenderQueue){
					maxRenderQueue = r;
				}
			}
			return true;
		});

		renderQeueuCount = maxRenderQueue;

		/*
		Renderer[] renderers = GameObject.FindObjectsOfType(typeof(Renderer)) as Renderer[];

		int maxRenderQueue = 0;
		foreach (Renderer renderer in renderers) {
			if (renderer.material != null && renderer.material.renderQueue > maxRenderQueue) {
				maxRenderQueue = renderer.material.renderQueue;
			}
		}
		renderQeueuCount = maxRenderQueue;*/
	}

	public bool performOnChildren(Func<object, bool> block)
	{
		for (int i = children.Count - 1; i >= 0; i--) {
			object child = children[i];
			
			if (!block (child)) {
				return false;
			}

			MethodInfo method = child.GetType().GetMethod ("performOnChildren");
			if (method != null) {
				bool shouldContinue = Convert.ToBoolean(method.Invoke (child, new[] { block }));
				if (!shouldContinue) {
					return false;
				}
			}
		}

		return true;
	}

	public bool performOnChildrenForward(Func<object, bool> block)
	{
		for (int i = 0; i < children.Count; i++) {
			object child = children[i];

			if (!block (child)) {
				return false;
			}

			MethodInfo method = child.GetType().GetMethod ("performOnChildrenForward");
			if (method != null) {
				bool shouldContinue = Convert.ToBoolean(method.Invoke (child, new[] { block }));
				if (!shouldContinue) {
					return false;
				}
			}
		}

		return true;
	}

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		base.gaxb_load(reader, _parent, args);

		if (args != null && args.ContainsKey ("baseRenderQueue")) {
			renderQeueuCount = (int)args ["baseRenderQueue"];
		}
	}

	public override void gaxb_unload()
	{
		NotificationCenter.removeObserver (this);
	}

	public PUObject scope()
	{
		if (isScopeContainer ())
			return this;
		if (parent == null)
			return this;
		if ((parent is PUObject) == false)
			return this;
		return (parent as PUObject).scope();
	}

	public virtual bool isScopeContainer()
	{
		return false;
	}
}
