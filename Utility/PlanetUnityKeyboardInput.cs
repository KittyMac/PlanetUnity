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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;


public class PlanetUnityKeyboardInput : MonoBehaviour
{
	static string lastMobileKeyboardText = "";

	#if UNITY_IPHONE || UNITY_ANDROID
	static TouchScreenKeyboard keyboard = null;
	#else 
	public enum TouchScreenKeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}
	#endif

	public static void OpenKeyboard(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, bool hideInput)
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		if( !Application.isEditor )
		{
			keyboard = TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert);
			TouchScreenKeyboard.hideInput = hideInput;
			lastMobileKeyboardText = "";
		}
		#endif
	}

	public static void CloseKeyboard()
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		if(keyboard != null)
		{
			keyboard.active = false;
			keyboard = null;
		}
		#endif
	}


	void Update () {
		foreach (Char c in Input.inputString) {

			if (c == "\b"[0]) {
				if (guiText.text.Length != 0)
					guiText.text = guiText.text.Substring(0, guiText.text.Length - 1);
				NotificationCenter.postNotification (null, PlanetUnity.USERCHARINPUT, NotificationCenter.Args("char", c, "string", guiText.text));
			}
			else if (c == "\n"[0] || c == "\r"[0]) {
				NotificationCenter.postNotification (null, PlanetUnity.USERCHARINPUT, NotificationCenter.Args("char", c, "string", guiText.text));
				NotificationCenter.postNotification (null, PlanetUnity.USERSTRINGINPUT, NotificationCenter.Args("string", guiText.text));
				guiText.text = "";
			}
			else {
				guiText.text += c;
				NotificationCenter.postNotification (null, PlanetUnity.USERCHARINPUT, NotificationCenter.Args("char", c, "string", guiText.text));
			}
		}


		#if UNITY_IPHONE || UNITY_ANDROID
		// Also, support mobile seamlessly...
		if(keyboard != null)
		{
			if(keyboard.text.Equals(lastMobileKeyboardText) == false)
			{
				NotificationCenter.postNotification (null, PlanetUnity.USERCHARINPUT, NotificationCenter.Args("string", keyboard.text));
				lastMobileKeyboardText = keyboard.text;
			}
			if(keyboard.done)
			{
				NotificationCenter.postNotification (null, PlanetUnity.USERSTRINGINPUT, NotificationCenter.Args("string", keyboard.text));
				CloseKeyboard();
			}
			else if(keyboard.wasCanceled || keyboard.active == false)
			{
				NotificationCenter.postNotification (null, PlanetUnity.USERINPUTCANCELLED, NotificationCenter.Args("string", keyboard.text));
				CloseKeyboard();
			}
		}
		#endif
	}
}
