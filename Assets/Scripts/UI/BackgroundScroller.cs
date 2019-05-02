using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroller : MonoBehaviour {
  [Autohook]
  private RawImage bg;
  // Use this for initialization
  void Start() {
    bg = GetComponent<RawImage>();
  }

  // Update is called once per frame
  void Update() {
    Rect uvRect = bg.uvRect;
    uvRect.x -= 0.0015f;
    uvRect.y += 0.0015f;
    bg.uvRect = uvRect;
  }
}
