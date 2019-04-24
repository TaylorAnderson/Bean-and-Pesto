using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narwhal : Enemy {

  private float chargeSpeed = 0.01f;
  private SniperBullet currentBullet = null;
  public SniperBullet bullet = null;
  public GameObject particle;
  private float bulletDelayTimer = 0;
  private float bulletDelay = 1;
  public Sprite angledSprite;
  private Sprite normalSprite;

  private float startDelay = 2;

  private int chargePlayerToken;
  // Start is called before the first frame update
  override public void Start() {
    base.Start();
    this.normalSprite = this.sprite.sprite;
    this.particle.SetActive(false);
    outline.buffer = 2;
    outline.Regenerate();

  }
  // Update is called once per frame
  override public void Update() {
    base.Update();
    startDelay -= Time.deltaTime;
    this.sprite.color = Color.white;
    if (startDelay <= 0) {
      if (currentBullet == null) {

        this.bulletDelayTimer -= Time.deltaTime;
        if (bulletDelayTimer <= 0) {
          this.sprite.sprite = this.angledSprite;
          particle.SetActive(true);
          this.chargePlayerToken = SfxManager.instance.PlaySound(SoundType.BEAN_CHARGE, 1, true);
          this.currentBullet = Instantiate(bullet).GetComponent<SniperBullet>();
          this.currentBullet.power = 0.1f;
          this.currentBullet.transform.localScale = (Vector3)Vector2.one * 0.1f;
          this.currentBullet.GetComponent<Collider2D>().enabled = false;
          this.currentBullet.owner = this.type;
          currentBullet.transform.position = this.transform.position + Vector3.left * this.sprite.bounds.extents.x;
          this.bulletDelayTimer = bulletDelay;
        }
      }
      else {
        currentBullet.lifetime = 1;
        if (currentBullet.power < 3f) currentBullet.power += chargeSpeed * 2;
        else currentBullet.power = 3;

        if (currentBullet.transform.localScale.x < 1.5f) {
          currentBullet.transform.localScale += (Vector3)Vector2.one * chargeSpeed;


          if (currentBullet.transform.localScale.x > 1.25f) {
            this.sprite.color = Color.red;
          }
        }
        else {

          this.sprite.sprite = normalSprite;

          SfxManager.instance.StopLoopingSound(SoundType.BEAN_CHARGE, chargePlayerToken);
          SfxManager.instance.PlaySound(SoundType.BEAN_SHOOT);

          particle.SetActive(false);
          currentBullet.velocity = Vector2.left * 0.4f;
          currentBullet.shrinkMultiplier = 0.4f;
          currentBullet.shrinking = true;
          Camera.main.Kick(Vector2.left);

          this.currentBullet.GetComponent<Collider2D>().enabled = true;
          currentBullet = null;
        }
      }
    }


  }

  public override void Spawn(Side side) {
    base.Spawn(side);
    this.transform.position += Vector3.left * 0.5f; //so we center better on the indicator
    var dest = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.5f, 0));
    dest.z = 1;
    StartCoroutine(Auto.MoveTo(this.transform, dest, this.startDelay, Ease.BackOut));
  }

  public override void Die(bool killedByPlayer) {
    this.particle.SetActive(false);
    SfxManager.instance.StopLoopingSound(SoundType.BEAN_CHARGE, this.chargePlayerToken);
    if (this.currentBullet) {
      Destroy(this.currentBullet.gameObject);
    }
    base.Die(killedByPlayer);
  }
}
