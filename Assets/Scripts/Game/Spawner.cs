using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using deVoid.Utils;
public enum Side {
  RIGHT,
  BOTTOM,
  TOP,
}
public class Spawner : MonoBehaviour {

  public Entity enemy;
  public float delay;
  public Side side;
  public IndicatorUI indicator;

  private IndicatorUI indicatorCopy;
  private float timer;
  private float offset;

  [HideInInspector]
  public bool active = false;
  [HideInInspector]
  public bool spawned = false;
  [HideInInspector]
  public EntityType enemyType;

  public bool hasShield = false;
  public ShieldColors shieldColor = ShieldColors.BEAN;
  public float shieldHealth = 0;

  [HideInInspector] public Wave wave;
  void Start() {
    this.enemyType = enemy.type;
    this.timer = this.delay + 1.5f; //allow time for indicator
    this.indicatorCopy = Instantiate(indicator.gameObject).GetComponent<IndicatorUI>();
    if (this.side == Side.BOTTOM || this.side == Side.TOP) offset = this.transform.position.x;
    else offset = this.transform.position.y;

    indicatorCopy.gameObject.SetActive(false);
    indicatorCopy.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
    indicatorCopy.Init(this.enemy, this.side, offset);
    Signals.Get<SpawningSignal>().Dispatch(enemy.GetComponent<Enemy>().type);
  }

  //called by wave manager


  // Update is called once per frame
  void Update() {
    if (!active) return;
    timer -= Time.deltaTime;
    if (timer < 1.5f) {
      indicatorCopy.gameObject.SetActive(true);
    }

    if (timer < 0) {
      Enemy enemyCopy = Instantiate(enemy.gameObject).GetComponent<Enemy>();
      var worldSpawnPoint = Camera.main.ScreenToWorldPoint(this.indicatorCopy.transform.position);
      enemyCopy.transform.position = new Vector3(worldSpawnPoint.x, worldSpawnPoint.y, 1);
      enemyCopy.Spawn(this.side);

      if (enemy.GetComponent<Enemy>().type != EntityType.NARWHAL) { //hack alert
        enemyCopy.hasShield = this.hasShield;
        enemyCopy.shieldColor = this.shieldColor;
        enemyCopy.shieldHealth = this.shieldHealth;
      }

      spawned = true;
      this.active = false;
      Destroy(indicatorCopy.gameObject);
    }
  }
  public void DrawGizmo(int wave) {
#if (UNITY_EDITOR)
    GUIStyle style = new GUIStyle();
    style.normal.textColor = Color.white;
    style.alignment = TextAnchor.MiddleCenter;
    Handles.Label(transform.position + Vector3.up * 1f + Vector3.left * 0.25f, "Delay: " + this.delay.ToString() + "s\n" + "Wave: " + wave.ToString(), style);

    if (enemy.gameObject != null) {
      switch (enemyType) {
        case EntityType.SHIP:
          Gizmos.DrawIcon(transform.position, "helmet-icon.png", true);
          break;
        case EntityType.SHARK:
          Gizmos.DrawIcon(transform.position, "shark-icon.png", true);
          break;
        case EntityType.JELLY:
          Gizmos.DrawIcon(transform.position, "jelly-icon.png", true);
          break;
        case EntityType.FISH_SCHOOL:
          Gizmos.DrawIcon(transform.position, "fish-icon.png", true);
          break;
        case EntityType.PUFFER:
          Gizmos.DrawIcon(transform.position, "pufferfish-icon.png", true);
          break;
        case EntityType.NARWHAL:
          Gizmos.DrawIcon(transform.position, "narwhal-icon.png", true);
          break;
      }
    }
#endif
  }


}
