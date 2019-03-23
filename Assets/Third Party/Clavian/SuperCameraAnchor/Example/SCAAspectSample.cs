//Copyright (c) 2016-2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCAAspectSample : MonoBehaviour {
	public AspectUtility aspect;
	[Range(0.5f, 2f)]
	public float aspectRatio = 1.777777778f;
	public float aspectChangeSpeed = 0.5f;
	void Update(){
		if(Input.GetKey(KeyCode.LeftArrow)){
			aspectRatio -= Time.deltaTime * aspectChangeSpeed;
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			aspectRatio += Time.deltaTime * aspectChangeSpeed;
		}
		aspectRatio = Mathf.Clamp(aspectRatio, 0.5f, 2f);
		SetNewAspectRatio();
	}
	void SetNewAspectRatio(){
		aspect._wantedAspectRatio = aspectRatio;
		aspect.SetCamera();
		SuperCameraAnchor.RefreshAll();
	}
}
