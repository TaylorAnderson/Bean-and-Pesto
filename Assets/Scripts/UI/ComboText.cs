using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;
public class ComboText : MonoBehaviour {
  public int score;
  private int currentScore;
  private SuperTextMesh comboText;
  public SpriteAnim animator;
  public AnimationClip comboAnim;

  private int combo = 1;
  // Start is called before the first frame update
  void Start() {
    comboText = GetComponentInChildren<SuperTextMesh>();
  }
  void Update() {

  }

  public void OnComboChanged() {

  }

  public void SetCombo(int combo) {
    this.comboText.transform.localScale = (Vector3)Vector2.one * MathUtil.Map(GameManager.instance.combo, 1, 100, 1, 2) + Vector3.forward;
    if (this.combo < combo)
      animator.Play(comboAnim);

    this.combo = combo;
    this.comboText.text = combo.ToString() + "x";

  }


}
