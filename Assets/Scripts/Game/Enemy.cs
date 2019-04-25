using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;
public class Enemy : Actor {
  public float health = 6;
  public SpriteRenderer sprite;
  public GameObject explosion;
  public GameObject energyDrop;

  public int maxEnergyDrops = 3;
  public int minEnergyDrops = 6;

  public bool playHitSound = true;

  protected SpriteOutline outline;
  protected float killShakeMultipler = 1f;
  protected float hitStopMultiplier = 1f;
  protected Vector2 offset;

  public int score = 100;

  // Start is called before the first frame update
  public override void Start() {
    base.Start();

    GameManager.instance.RegisterEnemy(this);
    outline = this.sprite.GetComponent<SpriteOutline>();
    outline.size = 3;
    outline.enabled = hasShield;

    if (!outline.enabled) outline.Clear();
    ColorUtility.TryParseHtmlString(ShieldColorMap.map[this.shieldColor], out outline.color);
    outline.Regenerate();
  }
  public virtual void TakeHit(Bullet bullet) {
    if (GameManager.instance.paused) return;
    var shieldActive = this.hasShield && this.shieldHealth > 0;
    if (!shieldActive) {
      DisplayUtil.FlashWhite(this, this.sprite, bullet.power / 40);
      if (this.playHitSound) { SfxManager.instance.PlaySound(SoundType.ENEMY_HURT); }
    }


    StartCoroutine(GameManager.instance.PauseForSeconds(shieldActive ? 0.005f : 0.01f * hitStopMultiplier, () => {
      Camera.main.Kick(bullet.velocity * (bullet.power / 4));

      if (hasShield && shieldHealth >= 0) {
        if (bullet.color == shieldColor) {
          float tempShieldHealth = this.shieldHealth;
          this.shieldHealth -= bullet.power;
          bullet.power -= tempShieldHealth;

          if (this.shieldHealth <= 0) {
            this.outline.Clear();
            this.outline.enabled = false;
            this.hasShield = false;
            Camera.main.Shake(0.6f);
            GameObject explosionCopy = Instantiate(explosion);

            SfxManager.instance.PlaySound(SoundType.SHIELD_DESTROYED, 1.2f);
            explosionCopy.transform.localScale -= Vector3.one * 0.25f;
            explosionCopy.transform.position = transform.position + (Vector3)offset;
          }
          else {
            DisplayUtil.FlashOutlineWhite(this, this.outline, bullet.power);
          }
        }
        else {
          SfxManager.instance.PlaySound(SoundType.SHIELD_DEFLECT);
          var offsetVector = (bullet.transform.position - transform.position).normalized * bullet.velocity.magnitude;
          bullet.velocity = offsetVector;
        }

      }
      else {
        float tempHealth = this.health;
        this.health -= bullet.power;
        bullet.power -= tempHealth;
      }

      Signals.Get<HitByBulletSignal>().Dispatch(new AttackData(this.type, bullet.owner, this != null && this.health <= 0));

      if (bullet != null && bullet.power <= 0) {
        bullet.Die();
      }
      if (this != null && this.health <= 0) {
        this.Die(true);
      }
    }));
  }
  public virtual void Die(bool killedByPlayer) {


    if (killedByPlayer) {

      Camera.main.Shake(this.killShakeMultipler);
      GameObject explosionCopy = Instantiate(explosion);
      explosionCopy.transform.position = transform.position + (Vector3)offset;

      SfxManager.instance.PlaySound(SoundType.EXPLOSION, this is Fish ? 0.3f : 0.6f);


      var dropAmt = Random.Range(this.minEnergyDrops, this.maxEnergyDrops);

      ShieldColors shieldColor = Random.value > 0.5f ? ShieldColors.BEAN : ShieldColors.PESTO;
      for (int i = 0; i < dropAmt; i++) {
        var energyDropScript = Instantiate(this.energyDrop).GetComponent<EnergyDrop>();
        energyDropScript.transform.position = this.transform.position + (Vector3)this.offset;
        float angle = (float)(i * 360 / dropAmt) * Mathf.Deg2Rad;
        energyDropScript.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * 0.1f;
        energyDropScript.shieldColor = shieldColor;
      }

      if (GameManager.instance != null) {
        GameManager.instance.AddScore(this.score, this.transform.position + (Vector3)offset);
        GameManager.instance.IncrementCombo();
      }
    }
    Destroy(gameObject);
  }
  public virtual void Spawn(Side side) {
    if (side == Side.BOTTOM) {
      this.transform.position += Vector3.down * sprite.bounds.size.y;
    }
    if (side == Side.TOP) {
      this.transform.position += Vector3.up * sprite.bounds.size.y;
    }
    if (side == Side.RIGHT) {
      this.transform.position += Vector3.right * sprite.bounds.size.x;
      this.transform.position += Vector3.up * sprite.bounds.size.y;
    }

  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponent<Bullet>() != null && other.GetComponent<Bullet>().team == BulletTeam.PLAYER && !GameManager.instance.paused) {
      TakeHit(other.GetComponent<Bullet>());
    }
  }

  public virtual void OnDestroy() {
    GameManager.instance.DeregisterEnemy(this);
  }
}
