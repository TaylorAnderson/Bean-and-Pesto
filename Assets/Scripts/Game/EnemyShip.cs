using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
  Shooting,
  Moving
}
public class EnemyShip : Enemy {
  public Bullet bullet;
  public Transform bulletPos;
  private float[] positions = new float[4];
  private int currentIndex = 0;
  private float bulletEmitTimer = 0;
  private float bulletDelay = 0.1f;
  private float bulletsFired = 0;
  private float travelTime = 1f;
  private float travelTimer = 0;
  private Vector2 velocity;
  private bool comingIn = true;
  private float currentTarget;
  void Awake() {
    this.type = EntityType.SHIP;
    offset = new Vector2(0.75f, -0.65f);
  }
  public override void DoUpdate() {
    Vector2 pointer = (Vector3.right * transform.position.x + Vector3.up * this.currentTarget) - this.transform.position;
    float dist = pointer.magnitude;
    this.velocity += pointer.normalized * dist * 0.008f;
    this.velocity *= 0.9f;
    travelTimer += Time.deltaTime;


    if (!comingIn) {
      if (travelTimer > travelTime) {
        bulletEmitTimer += Time.deltaTime;
        if (bulletEmitTimer > bulletDelay) {
          Bullet bullet = Instantiate(this.bullet);
          bullet.owner = this.type;
          bullet.transform.position = bulletPos.position;
          bullet.angleOffset = 180;
          bulletEmitTimer = 0;
          bulletsFired++;
          bullet.velocity = Vector2.left * 0.6f;
        }
        if (bulletsFired >= 3) {
          this.currentIndex++;
          this.currentIndex %= this.positions.Length;
          this.currentTarget = this.positions[this.currentIndex];
          this.travelTimer = 0;

          this.bulletsFired = 0;
          this.bulletEmitTimer = 0;
        }
      }
    }
    else {
      if (this.travelTimer > this.travelTime) {
        comingIn = false;
        this.travelTimer = 0;
        this.currentTarget = this.positions[this.currentIndex];
      }
    }

    this.transform.position += (Vector3)velocity;
  }

  public override void TakeHit(Bullet bullet) {
    base.TakeHit(bullet);
    //this.velocity += bullet.velocity / 10;
  }

  public override void Spawn(Side side) {
    base.Spawn(side);
    this.transform.position += Vector3.left * 0.5f; //so we center better on the indicator
    this.currentTarget = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.5f, 0)).y;

    var topmost = Camera.main.ViewportToWorldPoint(Vector3.one).y - 2;
    var bottommost = Camera.main.ViewportToWorldPoint(Vector3.zero).y + sprite.bounds.size.y + 2;
    var mid = Camera.main.ViewportToWorldPoint(Vector3.one * 0.5f).y + sprite.bounds.extents.y;

    if (side == Side.BOTTOM || side == Side.TOP) {

      if (side == Side.BOTTOM) {
        positions[0] = mid - 2;
        positions[1] = topmost;
        positions[2] = mid + 2;
        positions[3] = bottommost;
      }
      else {
        positions[0] = mid + 2;
        positions[1] = bottommost;
        positions[2] = mid - 2;
        positions[3] = topmost;
      }
    }
    else {
    }
  }
}
