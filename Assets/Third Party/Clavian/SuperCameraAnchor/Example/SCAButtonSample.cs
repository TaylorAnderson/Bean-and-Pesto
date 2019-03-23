using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCAButtonSample : MonoBehaviour {

	public bool isMouseOver;
	public bool isMouseDrag;
	public Vector3 startingScale;
	public Vector3 hoverScale;
	public Vector3 clickScale;
	public float changeRate = 1f;

	void Start(){
		startingScale = transform.localScale;
	}

	void Update () {
		Vector3 goalScale = isMouseOver ? isMouseDrag ? clickScale : hoverScale : startingScale;
		transform.localScale = Vector3.MoveTowards(transform.localScale, goalScale, changeRate);
		isMouseOver = false;
		isMouseDrag = false;
	}
	void OnMouseOver(){
		isMouseOver = true;
	}
	void OnMouseDrag(){
		isMouseDrag = true;
	}
}
