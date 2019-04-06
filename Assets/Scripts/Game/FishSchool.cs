using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchool : Enemy {


  [HideInInspector] public float leftmost = 1000;
  [HideInInspector] public float rightmost = -1000;

  public float waveWidth;
  public float waveHeight;
  // Start is called before the first frame update
  public override void Start() {
    for (int i = 0; i < transform.childCount; i++) {
      var child = transform.GetChild(i).transform.position;
      if (child.x < leftmost) leftmost = child.x;
      if (child.x > rightmost) rightmost = child.x;
    }
    for (int j = 0; j < transform.childCount; j++) {
      var fish = transform.GetChild(j).GetComponent<Fish>();
      fish.hasShield = this.hasShield;
      fish.shieldColor = this.shieldColor;
      fish.shieldHealth = this.shieldHealth;
      fish.Init(this);
    }
  }

  override public void TakeHit(Bullet bullet) { }
  override public void Spawn(Side side) {
    this.transform.position += Vector3.up * 0.5f;
  }
}
