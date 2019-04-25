using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wave : MonoBehaviour {
  public int wave = 0;
  private List<Spawner> spawners = new List<Spawner>();

  [HideInInspector]
  private float lifetime = 8;

  public bool finished = false;

  private bool infinite;
  // Start is called before the first frame update
  void Start() {
    Init();
    infinite = lifetime == -1;
  }

  public void Init() {
    GameManager.instance.RegisterWave(this);
    for (int i = 0; i < transform.childCount; i++) {
      var child = transform.GetChild(i);
      var spawner = child.GetComponent<Spawner>();
      spawners.Add(spawner);
      spawner.wave = this;
      if (this.wave == 0) SetSpawnersActive();
    }
  }

  // Update is called once per frame
  void Update() {
    this.lifetime -= Time.deltaTime;
    this.finished = this.lifetime <= 0 && !infinite;
  }
  public void SetSpawnersActive() {
    for (int i = 0; i < spawners.Count; i++) {
      spawners[i].active = true;
    }
  }
  public bool AllEnemiesSpawned() {
    bool allSpawned = true;
    for (int i = 0; i < spawners.Count; i++) {
      if (!spawners[i].spawned && spawners[i].active) allSpawned = false;
    }
    return allSpawned;
  }
  private void OnDrawGizmos() {
#if (UNITY_EDITOR)
    for (int i = 0; i < transform.childCount; i++) {
      Spawner spawner = transform.GetChild(i).GetComponent<Spawner>();
      spawner.DrawGizmo(this.wave);
    }
#endif
  }
}
