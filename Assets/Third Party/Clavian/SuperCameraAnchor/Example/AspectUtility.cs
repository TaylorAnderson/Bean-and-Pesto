/*
This code is a modified and trimmed-down version of the code from 
http://wiki.unity3d.com/index.php?title=AspectRatioEnforcer 
and was originally written by Eric Haines.

It is only intended for demonstration purposes, and the full code is available for free online.
*/
using UnityEngine;
 
public class AspectUtility : MonoBehaviour {
	public float _wantedAspectRatio = 1.7777778f;
	public Camera cam;
	public Camera backgroundCam;
 
	public void SetCamera () {
		float currentAspectRatio = (float)Screen.width / Screen.height;
		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(_wantedAspectRatio * 100) / 100.0f) {
			cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			return;
		}
		// Pillarbox
		if (currentAspectRatio > _wantedAspectRatio) {
			float inset = 1.0f - _wantedAspectRatio/currentAspectRatio;
			cam.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		}
		// Letterbox
		else {
			float inset = 1.0f - currentAspectRatio/_wantedAspectRatio;
			cam.rect = new Rect(0.0f, inset/2, 1.0f, 1.0f-inset);
		}
	}
}