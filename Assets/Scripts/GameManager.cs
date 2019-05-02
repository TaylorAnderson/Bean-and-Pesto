using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using deVoid.Utils;
using UnityEngine.SceneManagement;



public class Data {
  public int score;
  public Data(int score) {
    this.score = score;
  }
}
public class GameManager : MonoBehaviour {
  public static GameManager instance;

  public bool paused = false;
  public bool gameEnded = false;
  public int combo = 1;
  public int score = 0;
  public int bestScore = 0;



  private float comboKillTime = 10;
  private float comboKillTimer = 10;

  public ComboText comboText;
  [SerializeField] protected ScoreText scoreText;
  [SerializeField] protected GameObject scorePop;
  [SerializeField] protected GameObject scorePopNegative;
  [SerializeField] protected Canvas canvas;
  [SerializeField] protected GameObject gameOverBundle;

  public EndPauseSignal endPauseSignal;



  void Awake() {
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 60;
    Screen.fullScreen = false;
    Screen.SetResolution(1280, 720, false);
    //Check if instance already exists
    if (instance == null)
      instance = this;

    else if (instance != this)
      Destroy(gameObject);

    DontDestroyOnLoad(gameObject);

    SceneManager.activeSceneChanged += (Scene current, Scene next) => {

      if (next.buildIndex == 1) {
        score = 0;
      }
    };
  }



  // Update is called once per frame
  void Update() {
    if (this.combo > 1) {
      comboKillTimer -= Time.deltaTime;
      //this.comboMeter.fillAmount = MathUtil.Map(this.comboKillTimer, this.comboKillTime, 0, 0, 1);
      if (comboKillTimer <= 0) {
        //this.DecrementCombo();
        comboKillTimer = comboKillTime;
      }
    }



  }

  public IEnumerator PauseForSeconds(float seconds, Action endPauseAction = null) {
    this.paused = true;
    Camera.main.Stop(seconds);

    if (endPauseAction != null) Signals.Get<EndPauseSignal>().AddListener(endPauseAction);
    yield return new WaitForSeconds(seconds);
    Signals.Get<EndPauseSignal>().Dispatch();
    Signals.Get<EndPauseSignal>().RemoveListener(endPauseAction);
    this.paused = false;
  }


  public void AddScore(int score, Vector3 position) {

    this.score += score < 0 ? score : score * this.combo;
    this.scoreText.score = this.score;
    GameObject scorePopCopy = Instantiate(score < 0 ? scorePopNegative : scorePop);

    scorePopCopy.transform.SetParent(canvas.transform);
    scorePopCopy.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
    scorePopCopy.transform.position += Vector3.left * scorePopCopy.GetComponent<RectTransform>().rect.width / 2;
    scorePopCopy.GetComponent<ScorePop>().SetText(score.ToString() + (this.combo > 1 && score > 0 ? " x " + this.combo.ToString() : ""));
  }
  public void IncrementCombo() {
    comboKillTimer = comboKillTime;
    combo++;
    comboText.SetCombo(combo);

  }
  public void DecrementCombo() {
    if (combo == 1) return;
    this.combo = (int)(combo / 2);
    comboText.SetCombo(combo);
  }
  public void ClearCombo() {
    combo = 1;
    comboText.SetCombo(combo);
  }





  public void Save(int score) {
    string path = Application.persistentDataPath + "/";
    File.WriteAllText(path + "times.json", JsonUtility.ToJson(new Data(score), true));
  }
  public Data Load() {
    string path = Application.persistentDataPath + "/" + "times.json";
    try {
      if (File.Exists(path)) {
        return JsonUtility.FromJson<Data>(File.ReadAllText(path));
      }
      return null;

    } catch (System.Exception ex) {
      Debug.Log(ex.Message);
      return null;
    }
  }

  public void EndGame() {

    SfxManager.instance.StopLoopingSound(SoundType.GAME_MUSIC, -1);

    var data = Load();

    if (data == null || this.score > data.score) {
      bestScore = score;
      Save(score);
    }
    else {
      bestScore = data.score;
    }
    GameManager.instance.gameEnded = true;

    StartCoroutine(StartGameOverAfterDelay(1));
  }

  public IEnumerator StartGameOverAfterDelay(float delay) {
    yield return new WaitForSeconds(delay);
    SfxManager.instance.StopAllSounds();
    gameOverBundle.SetActive(true);

  }
}
