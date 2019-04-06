using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : MonoBehaviour {
  public int score;
  private int currentScore;
  private SuperTextMesh comboText;
  // Start is called before the first frame update
  void Start() {
    comboText = GetComponent<SuperTextMesh>();
  }


}
