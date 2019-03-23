using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePop : MonoBehaviour {
  private float upVel = 10f;
  public SuperTextMesh whiteText;
  public SuperTextMesh blueText;
  private float textFlipper;
  private float lifetime = 1;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    lifetime -= Time.deltaTime;
    if (lifetime < 0) Destroy(gameObject);
    textFlipper += Time.deltaTime;
    if (textFlipper > 0.05f) {
      textFlipper = 0;
      if (whiteText.gameObject.activeSelf) {
        whiteText.gameObject.SetActive(false);
        blueText.gameObject.SetActive(true);
      }
      else {
        whiteText.gameObject.SetActive(true);
        blueText.gameObject.SetActive(false);
      }
    }
    this.transform.position += Vector3.up * upVel;
    upVel -= 0.5f;
    if (upVel < 0) upVel = 0;
  }
  public void SetText(string text) {
    this.whiteText.text = this.blueText.text = text;
  }
}
