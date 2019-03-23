using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : Enemy {

  private Vector3 startTarget;
  private Vector2 velocity;

  private float travelTimer = 1f;
  private float followTimer = 1f;
  private float waitTimer = 0.5f;
  private float killTimer = 3;
  public GameObject crosshair;
  private GameObject crosshairCopy;
  private bool crosshairSpawned = false;

  override public void Start() {
    base.Start();
    this.type = EntityType.JELLY;
    this.GetComponentInChildren<Bullet>().owner = this.type;
  }
  override public void DoUpdate() {
    travelTimer -= Time.deltaTime;

    if (travelTimer >= 0) {
      Vector2 pointer = this.startTarget - this.transform.position;
      float dist = pointer.magnitude;
      this.velocity += pointer.normalized * dist * 0.008f;

    }
    else {
      if (Ship.ActiveShip != null) {
        followTimer -= Time.deltaTime;
        if (followTimer > 0) {
          if (!crosshairSpawned) {
            crosshairCopy = Instantiate(crosshair);
            crosshairSpawned = true;
          }
          crosshairCopy.transform.position = Ship.ActiveShip.transform.position;
          Vector3 diff = Ship.ActiveShip.transform.position - this.transform.position;
          float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
          transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else {
          sprite.color = Color.red;
          waitTimer -= Time.deltaTime;
          if (waitTimer <= 0) {
            Destroy(crosshairCopy);
            float angleInRadians = (transform.localEulerAngles.z + 90) * Mathf.Deg2Rad;
            this.velocity = Vector2.right * Mathf.Cos(angleInRadians) + Vector2.up * Mathf.Sin(angleInRadians);
            this.velocity = this.velocity.normalized * 1f;
            this.killTimer -= Time.deltaTime;
            if (this.killTimer < 0) {
              Die(false);
            }
          }
        }
      }
    }

    this.velocity *= 0.9f;

    this.transform.position += (Vector3)velocity;

  }

  override public void Spawn(Side side) {
    base.Spawn(side);
    this.transform.position += Vector3.left * 0.5f; //so we center better on the indicator
    this.startTarget = Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.5f, 0));

    if (side == Side.BOTTOM || side == Side.TOP) {
      this.startTarget.x = this.transform.position.x;

    }
    else {
      this.transform.position += Vector3.down * this.sprite.bounds.size.y / 2;
      this.startTarget.y = this.transform.position.y;

    }
  }

  override public void OnDestroy() {
    base.OnDestroy();
    if (crosshairSpawned) Destroy(crosshairCopy);
  }
}
