
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Security.Permissions;

public class FollowPath : MonoBehaviour {

	public string pathPoints;
	public bool loop;
	public float rate;

	public GameObject target;

	private float anim = 0;
	private List<Vector3> points;


	float catmullRomSpline(float x, float v0,float v1, float v2,float v3)
	{
		/* Coefficients for Matrix M */
		const float M11 =	 0.0f;
		const float M12 =	 1.0f;
		const float M13 =	 0.0f;
		const float M14 =	 0.0f;
		const float M21 =	-0.5f;
		const float M22 =	 0.0f;
		const float M23 =	 0.5f;
		const float M24 =	 0.0f;
		const float M31 =	 1.0f;
		const float M32 =	-2.5f;
		const float M33 =	 2.0f;
		const float M34 =	-0.5f;
		const float M41 =	-0.5f;
		const float M42 =	 1.5f;
		const float M43 =	-1.5f;
		const float M44 =	 0.5f;

		float c1,c2,c3,c4;

		c1 =  	      M12*v1;
		c2 = M21*v0          + M23*v2;
		c3 = M31*v0 + M32*v1 + M33*v2 + M34*v3;
		c4 = M41*v0 + M42*v1 + M43*v2 + M44*v3;

		return(((c4*x + c3)*x +c2)*x + c1);
	}

	void Start() {
		Reset(false);
	}

	public void Reset(bool useCurrentPosition) {
		// Parse the pathPoints string.  it is comma-delimited list of floats, we need to separate into vec3 list
		points = new List<Vector3>();

		var elements = pathPoints.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

		if (useCurrentPosition) {
			points.Add (gameObject.transform.localPosition);
		}

		float x = 0, y = 0, z = 0;
		int idx = 0;
		foreach (string floatString in elements) {
			float t = float.Parse(floatString, System.Globalization.CultureInfo.InvariantCulture);

			if(idx == 0)
				x = t;
			if(idx == 1)
				y = t;
			if(idx == 2)
			{
				z = t;

				points.Add (new Vector3 (x, y, z));

				idx = -1;
			}

			idx++;
		}

		anim = 0;
	}

	void Update() {

		anim += Time.deltaTime;

		if (loop == false) {
			if (anim > rate) {
				anim = rate - 0.01f;
			}
		} else {
			while (anim > rate) {
				anim -= rate;
			}
		}

		// Find the correct position for the camera...
		int idx = (int)Math.Floor ((anim/rate) * points.Count);
		float idxPart = (float)((anim/rate) * points.Count) - idx;

		int prevIndex = idx;
		int nextIndex = idx+1;
		int prevIndex2 = prevIndex-1;
		int nextIndex2 = nextIndex+1;

		if (nextIndex2 >= points.Count) {
			if (loop) {
				nextIndex2 -= points.Count;
			} else {
				nextIndex2 = points.Count - 1;
			}
		}
		if (prevIndex2 < 0) {
			if (loop) {
				prevIndex2 += points.Count;
			} else {
				prevIndex2 = 0;
			}
		}

		if (nextIndex >= points.Count) {
			if (loop) {
				nextIndex -= points.Count;
			} else {
				nextIndex = points.Count - 1;
			}
		}
		if (prevIndex < 0) {
			if (loop) {
				prevIndex += points.Count;
			} else {
				prevIndex = 0;
			}
		}

		float posX = catmullRomSpline (idxPart, points [prevIndex2].x, points [prevIndex].x, points [nextIndex].x, points [nextIndex2].x);
		float posY = catmullRomSpline (idxPart, points [prevIndex2].y, points [prevIndex].y, points [nextIndex].y, points [nextIndex2].y);
		float posZ = catmullRomSpline (idxPart, points [prevIndex2].z, points [prevIndex].z, points [nextIndex].z, points [nextIndex2].z);

		gameObject.transform.position = new Vector3 (posX, posY, posZ);

		if (target != null) {
			gameObject.transform.LookAt (target.transform.position);
		}
	}
}
