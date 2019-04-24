using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class BeanShip : Ship {
  private SniperBullet currentBullet = null;
  private float chargeSpeed = 0.08f;

  public GameObject particle;

  private int chargePlayerToken = -1;

  public override void Start() {
    base.Start();
    this.type = EntityType.BEAN;
    particle.SetActive(false);
    bulletSpeed = 1;

  }


  override public void Switch() {
    this.particle.SetActive(false);
    SfxManager.instance.StopLoopingSound(SoundType.BEAN_CHARGE, this.chargePlayerToken);
    if (this.currentBullet) {
      Destroy(this.currentBullet.gameObject);
    }
  }


  override public void Shoot(PlayerAction shootAction) {
    if (shootAction.IsPressed && active) {
      particle.SetActive(true);
      if (currentBullet == null) {
        this.chargePlayerToken = SfxManager.instance.PlaySound(SoundType.BEAN_CHARGE, 1, true);

        this.currentBullet = Instantiate(bullet).GetComponent<SniperBullet>();
        this.currentBullet.power = 0.1f;
        this.currentBullet.transform.localScale = (Vector3)Vector2.one * 0.1f;
        this.currentBullet.GetComponent<Collider2D>().enabled = false;
        this.currentBullet.owner = this.type;
        //currentBullet.gameObject.SetActive(false);
      }
      if (currentBullet.power < 3f) currentBullet.power += chargeSpeed * 2;
      else currentBullet.power = 3;
      if (currentBullet.transform.localScale.x < 1.5f) {
        currentBullet.transform.localScale += (Vector3)Vector2.one * chargeSpeed;
      }
      currentBullet.lifetime = 1;
      currentBullet.transform.position = this.transform.position + Vector3.right * 1.4f + Vector3.down * 0.7f;
    }
    else {
      particle.SetActive(false);
    }
    if (shootAction.WasReleased) {
      SfxManager.instance.StopLoopingSound(SoundType.BEAN_CHARGE, this.chargePlayerToken);
      //currentBullet.gameObject.SetActive(true);
      //this.energy -= currentBullet.power * 5;
      SfxManager.instance.PlaySound(SoundType.BEAN_SHOOT, MathUtil.Map(currentBullet.power, 0, 3, 0, 1));
      currentBullet.velocity = Vector2.right * this.bulletSpeed;
      currentBullet.shrinking = true;
      this.velocity += Vector2.left * currentBullet.power * Time.deltaTime;
      Camera.main.Kick(Vector2.left);
      this.currentBullet.GetComponent<Collider2D>().enabled = true;

      currentBullet = null;
    }
  }
}
