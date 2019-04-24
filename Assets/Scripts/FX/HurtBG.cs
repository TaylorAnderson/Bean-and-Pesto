using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBG : MonoBehaviour {
  [Autohook]
  public SpriteRenderer sprite;

  private Color color = Color.white;
  // Start is called before the first frame update
  void Start() {
    color.a = 0;
  }

  void Update() {
    color.a -= 0.01f;
    this.sprite.color = color;
  }

  public void Show() {
    color.a = 1;
  }
}
