using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medal : MonoBehaviour {
  public int bronzeCutoff;
  public int silverCutoff;
  public int goldCutoff;

  public Sprite bronzeMedal;
  public Sprite silverMedal;
  public Sprite goldMedal;

  private Image img;
  // Start is called before the first frame update
  void Start() {
    this.img = GetComponent<Image>();

  }

  // Update is called once per frame
  void Update() {

  }

  public void SetScore(int score){
    if (score < bronzeCutoff) gameObject.SetActive(false);
    else if (score < silverCutoff) img.sprite = bronzeMedal;
    else if (score < goldCutoff) img.sprite = silverMedal;
    else img.sprite = goldMedal;
  }
}
