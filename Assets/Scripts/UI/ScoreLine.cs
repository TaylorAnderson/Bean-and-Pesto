using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLine : MonoBehaviour
{
    public int score;
    public bool isBestScore;
    public SuperTextMesh scoreText;
    public Medal medal;
    // Start is called before the first frame update
    void Start()
    {
      if (isBestScore) {
        SetScore(GameManager.instance.bestScore);
      }
      else {
        SetScore(GameManager.instance.score);
      }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score) {
      scoreText.text = score.ToString();
      medal.SetScore(score);
    }
}
