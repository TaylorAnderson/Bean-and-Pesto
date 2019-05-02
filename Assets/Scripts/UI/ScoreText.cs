using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour {
  public int score;
  private int currentScore;
  private SuperTextMesh scoreText;
  // Start is called before the first frame update
  void Start() {
    scoreText = GetComponent<SuperTextMesh>();
  }

  // Update is called once per frame
  void Update() {
    if (this.currentScore != score) {
      this.scoreText.transform.localScale = Vector2.Lerp(this.scoreText.transform.localScale, Vector2.one * 1.5f, 0.2f);

      var scoreDiff = Mathf.Min(Mathf.Abs(this.score - this.currentScore), 50);
      this.currentScore += this.currentScore < score ? scoreDiff : -scoreDiff;
    }
    else {
      this.scoreText.transform.localScale = Vector2.Lerp(this.scoreText.transform.localScale, Vector2.one, 0.2f);
    }

    this.scoreText.text = this.currentScore.ToString();
  }
}
