using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narwhal : Entity {

  private float chargeSpeed = 0.08f;
  private SniperBullet currentBullet = null;
  private SniperBullet bullet = null;
  public GameObject particle;
  // Start is called before the first frame update
  override public void Start() {
    base.Start();
  }

  // Update is called once per frame
  override public void DoUpdate() {
    base.DoUpdate();
    particle.SetActive(true);
    if (currentBullet == null) {
      this.currentBullet = Instantiate(bullet).GetComponent<SniperBullet>();
      this.currentBullet.power = 0.1f;
      this.currentBullet.transform.localScale = (Vector3)Vector2.one * 0.1f;
      this.currentBullet.GetComponent<Collider2D>().enabled = false;
      this.currentBullet.owner = this.type;
      //currentBullet.gameObject.SetActive(false);
      currentBullet.lifetime = 1;
      currentBullet.transform.position = this.transform.position + Vector3.right * 1.4f + Vector3.down * 0.7f;
    }
    if (currentBullet.power < 3f) currentBullet.power += chargeSpeed * 2;
    else currentBullet.power = 3;
    if (currentBullet.transform.localScale.x < 1.5f) {
      currentBullet.transform.localScale += (Vector3)Vector2.one * chargeSpeed;
    }
    else {
      particle.SetActive(false);
      //currentBullet.gameObject.SetActive(true);
      //this.energy -= currentBullet.power * 5;
      currentBullet.velocity = Vector2.left;
      currentBullet.shrinking = true;
      Camera.main.Kick(Vector2.left);
      this.currentBullet.GetComponent<Collider2D>().enabled = true;

      currentBullet = null;
    }
  }
}
