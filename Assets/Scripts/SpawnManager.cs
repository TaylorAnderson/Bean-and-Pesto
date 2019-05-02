using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyIntro {
  public EntityType enemy;
  public GameObject wave;
}
public class SpawnManager : MonoBehaviour {
  public GameObject[] waveObjects;
  public List<Wave> wavePool;
  public EnemyIntro[] enemyIntrosArray;

  public static SpawnManager instance;
  private List<Enemy> currentEnemiesInWave = new List<Enemy>();
  private List<Wave> waves = new List<Wave>();

  private bool switchingWaves = false;
  private bool isEndless = true;
  [HideInInspector]
  public Wave currentWave = null;

  private int turnsBetweenIntros = 2;
  private int turnsBeforeIntro = 0;
  private int currentIntro = 0;


  private Dictionary<EntityType, Wave> enemyIntros = new Dictionary<EntityType, Wave>();
  private List<EntityType> enemiesToIntroduce = new List<EntityType> {
    EntityType.SHIP,
    EntityType.PUFFER,
    EntityType.FISH_SCHOOL,
    EntityType.JELLY,
    EntityType.NARWHAL,
    EntityType.SHARK
  };

  // Start is called before the first frame update
  void Awake() {
    if (instance == null) {
      SpawnManager.instance = this;
    }



    Util.Shuffle(enemiesToIntroduce);
    for (int j = 0; j < this.waveObjects.Length; j++) {
      this.wavePool.Add(this.waveObjects[j].GetComponent<Wave>());
    }

    for (int i = 0; i < this.enemyIntrosArray.Length; i++) {
      this.enemyIntros[this.enemyIntrosArray[i].enemy] = enemyIntrosArray[i].wave.GetComponent<Wave>();
    }
  }

  // Update is called once per frame
  void Update() {
    if (this.currentWave && this.currentWave.finished && !switchingWaves) {
      this.StartCoroutine(StartWave());
      switchingWaves = true;
    }
  }

  public void RegisterEnemy(Enemy enemy) {
    this.currentEnemiesInWave.Add(enemy);
  }

  public void DeregisterEnemy(Enemy enemy) {
    this.currentEnemiesInWave.Remove(enemy);

    if (this.currentEnemiesInWave.Count == 0) {
      if (currentWave && currentWave.AllEnemiesSpawned()) this.currentWave.finished = true;
    }
  }

  public void RegisterWave(Wave wave) {
    waves.Add(wave);
  }

  public void StartGame() {
    StartCoroutine(StartWave());
  }

  IEnumerator StartWave() {
    yield return new WaitForSeconds(0.5f);
    var wave = GenerateWave();
    if (wave) {
      wave.Init();
      this.currentWave = wave;

      this.switchingWaves = false;
    }
  }
  public Wave GenerateWave() {

    if (this.waveObjects.Length == 0) return null;
    Wave wave = GetWave();
    if (wave == null) {
      wave = enemyIntros[enemiesToIntroduce[currentIntro]];
      currentIntro++;
    }

    return Instantiate(wave.gameObject).GetComponent<Wave>();
  }

  public Wave GetWave() {
    List<Wave> wavesAllowed = new List<Wave>();
    List<EntityType> enemiesAllowed = this.enemiesToIntroduce.GetRange(0, currentIntro);
    for (int i = 0; i < wavePool.Count; i++) {
      if (wavePool[i].HasEnemies(enemiesAllowed)) {
        wavesAllowed.Add(wavePool[i]);
      }
    }
    if (wavesAllowed.Count == 0) {
      return null;
    }
    return wavesAllowed[Random.Range(0, wavesAllowed.Count - 1)];
  }


}
