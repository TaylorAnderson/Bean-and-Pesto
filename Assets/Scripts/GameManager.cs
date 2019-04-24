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
  public int wave = 0;
  public bool paused = false;
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
  private List<Enemy> currentEnemiesInWave = new List<Enemy>();
  private List<Wave> waves = new List<Wave>();
  public EndPauseSignal endPauseSignal;

  private bool switchingWaves = false;

  private bool isEndless = true;

  public GameObject[] waveObjects;

  private Wave _currentWave = null;

  public Wave CurrentWave {
    get {
      if (!isEndless) {
        for (int i = 0; i < waves.Count; i++) {
          if (waves[i].wave == this.wave) {
            return waves[i];
          }
        }
        return null;
      }
      else {
        return _currentWave;
      }

    }
  }

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

    if (isEndless) {
      StartCoroutine(StartWave());
    }
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

    if (this.CurrentWave && this.CurrentWave.finished && !switchingWaves) {
      this.StartCoroutine(StartWave());
      switchingWaves = true;
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

  public void RegisterEnemy(Enemy enemy) {
    this.currentEnemiesInWave.Add(enemy);
  }

  public void DeregisterEnemy(Enemy enemy) {
    this.currentEnemiesInWave.Remove(enemy);


    if (this.currentEnemiesInWave.Count == 0) {
      if (CurrentWave && CurrentWave.AllEnemiesSpawned()) this.CurrentWave.finished = true;
    }
  }

  public void RegisterWave(Wave wave) {
    waves.Add(wave);
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

  IEnumerator StartWave() {
    yield return new WaitForSeconds(0.5f);

    this.wave++;
    if (this.wave > this.waves.Count - 1 && !this.isEndless) {
      EndGame();
    }
    else {

      if (isEndless) {
        var wave = GenerateWave();
        if (wave) {
          wave.Init();
          this._currentWave = wave;
          wave.SetSpawnersActive();
          this.switchingWaves = false;
        }

      }
      else {
        CurrentWave.SetSpawnersActive();
      }

    }
  }

  public Wave GenerateWave() {
    if (this.waveObjects.Length == 0) return null;
    return Instantiate(this.waveObjects[UnityEngine.Random.Range(0, this.waveObjects.Length - 1)]).GetComponent<Wave>();
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

    var data = Load();
    print(data.score);
    print(this.score);
    if (data == null || this.score > data.score) {
      bestScore = score;
      Save(score);
    }
    else {
      bestScore = data.score;
    }

    SceneManager.LoadSceneAsync(2);
  }
}
