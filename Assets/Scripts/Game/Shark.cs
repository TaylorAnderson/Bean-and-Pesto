using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Enemy {
  private float speed = 0.15f;
  public GameObject circleTrail;
  public float circleTrailInterval;
  private float circleTrailIntervalTimer;
  private float pauseMove = 0;

  private float lifetime = 3;
  // Start is called before the first frame update
  void Awake() {
    this.offset = new Vector2(0.85f, -0.75f);
    this.type = EntityType.SHARK;
    this.GetComponentInChildren<Bullet>().owner = this.type;

  }

  // Update is called once per frame
  public override void DoUpdate() {
    lifetime -= Time.deltaTime;
    if (lifetime <= 0) Die(false);
    this.pauseMove -= Time.deltaTime;
    if (this.pauseMove <= 0) {
      this.transform.position += Vector3.left * speed;
    }
    this.circleTrailIntervalTimer += Time.deltaTime;
    if (this.circleTrailIntervalTimer > this.circleTrailInterval) {
      this.circleTrailIntervalTimer = 0;
      GameObject circleCopy = Instantiate(circleTrail);
      circleCopy.transform.position = this.transform.position + Vector3.right * 0.75f + Vector3.down * 0.73f;
    }
  }
}
