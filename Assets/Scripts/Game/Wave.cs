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
  public void Init() {
    spawners.Clear();
    SpawnManager.instance.RegisterWave(this);
    for (int i = 0; i < transform.childCount; i++) {
      var child = transform.GetChild(i);
      var spawner = child.GetComponent<Spawner>();
      spawners.Add(spawner);
      Debug.Log(spawners.Count);
      Debug.Log(spawner.gameObject.name);
      spawner.wave = this;
    }

    Debug.Log(gameObject.name + " spawners: " + spawners.Count);

  }

  // Update is called once per frame
  void Update() {
    this.lifetime -= Time.deltaTime;
    this.finished = this.lifetime <= 0 && !infinite;
  }
  public bool AllEnemiesSpawned() {
    bool allSpawned = true;
    for (int i = 0; i < spawners.Count; i++) {
      Debug.Log("spawner spawned " + spawners[i].spawned);
      if (!spawners[i].spawned && spawners[i].active) allSpawned = false;
    }
    return allSpawned;
  }
  public bool HasEnemy(EntityType enemyType) {

    for (int i = 0; i < this.spawners.Count; i++) {
      if (spawners[i].enemyType == enemyType) return true;
    }
    return false;
  }
  public bool HasEnemies(List<EntityType> enemyTypes) {
    for (int i = 0; i < enemyTypes.Count; i++) {
      if (HasEnemy(enemyTypes[i])) {
        return true;
      }
    }
    return false;
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
