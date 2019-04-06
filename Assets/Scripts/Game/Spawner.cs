using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Side {
  RIGHT,
  BOTTOM,
  TOP,
}
public class Spawner : MonoBehaviour {
  private float offset;
  public Entity enemy;
  public float delay;
  public Side side;
  public IndicatorUI indicator;
  private IndicatorUI indicatorCopy;
  private float timer;
  [HideInInspector]
  public bool active = false;
  [HideInInspector]
  public bool spawned = false;

  public bool hasShield = false;
  public ShieldColors shieldColor = ShieldColors.BEAN;
  public float shieldHealth = 0;

  [HideInInspector] public Wave wave;
  void Start() {
    this.timer = this.delay + 1.5f; //allow time for indicator
    this.indicatorCopy = Instantiate(indicator.gameObject).GetComponent<IndicatorUI>();
    if (this.side == Side.BOTTOM || this.side == Side.TOP) offset = this.transform.position.x;
    else offset = this.transform.position.y;

    indicatorCopy.gameObject.SetActive(false);
    indicatorCopy.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
    indicatorCopy.Init(this.enemy, this.side, offset);
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

      if (enemy.name != "Narwhal") { //hack alert
        enemyCopy.hasShield = this.hasShield;
        enemyCopy.shieldColor = this.shieldColor;
        enemyCopy.shieldHealth = this.shieldHealth;
      }

      spawned = true;
      Destroy(indicatorCopy.gameObject);
      Destroy(this.gameObject);
    }
  }
  public void DrawGizmo(int wave) {
#if (UNITY_EDITOR)
    GUIStyle style = new GUIStyle();
    style.normal.textColor = Color.white;
    style.alignment = TextAnchor.MiddleCenter;
    Handles.Label(transform.position + Vector3.up * 1f + Vector3.left * 0.25f, "Delay: " + this.delay.ToString() + "s\n" + "Wave: " + wave.ToString(), style);

    if (enemy.gameObject != null) {
      if (enemy.gameObject.name == "Enemy Ship") {
        Gizmos.DrawIcon(transform.position, "helmet-icon.png", true);
      }
      if (enemy.gameObject.name == "Shark") {
        Gizmos.DrawIcon(transform.position, "shark-icon.png", true);
      }
      if (enemy.gameObject.name == "JellyFish") {
        Gizmos.DrawIcon(transform.position, "jelly-icon.png", true);
      }
      if (enemy.gameObject.name == "Fish School") {
        Gizmos.DrawIcon(transform.position, "fish-icon.png", true);
      }
      if (enemy.gameObject.name == "PufferFish") {
        Gizmos.DrawIcon(transform.position, "pufferfish-icon.png", true);
      }
      if (enemy.gameObject.name == "Narwhal") {
        Gizmos.DrawIcon(transform.position, "narwhal-icon.png", true);
      }
    }
#endif
  }


}
