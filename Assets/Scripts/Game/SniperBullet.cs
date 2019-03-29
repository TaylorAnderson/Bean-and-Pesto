using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : Bullet {
  public bool shrinking = false;
  public float shrinkMultiplier = 1;
  public override void DoUpdate() {
    base.DoUpdate();
    if (shrinking) {
      if (transform.localScale.y > 0.6f) {
        transform.localScale += Vector3.down * 0.18f * this.shrinkMultiplier;
        transform.localScale += Vector3.right * 0.18f * this.shrinkMultiplier;
      }
    }
  }
}
