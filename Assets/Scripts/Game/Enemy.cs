using System.Collections;
using System.Collections.Generic;
using deVoid.Utils;
using UnityEngine;
public class Enemy : Actor {
  public float health = 6;
  public SpriteRenderer sprite;
  public GameObject explosion;

  private SpriteOutline outline;
  protected Vector2 offset;
  // Start is called before the first frame update
  public override void Start () {
    base.Start ();

    GameManager.instance.RegisterEnemy (this);
    outline = this.sprite.GetComponent<SpriteOutline> ();
    outline.size = 3;
    outline.enabled = hasShield;
    ColorUtility.TryParseHtmlString (this.shieldColorMap[this.shieldColor], out outline.color);
  }
  public virtual void TakeHit (Bullet bullet) {
    if (GameManager.instance.paused) return;

    if (!this.hasShield || this.shieldHealth <= 0) DisplayUtil.FlashWhite (this, this.sprite, bullet.power / 40);
    
    StartCoroutine (GameManager.instance.PauseForSeconds (bullet.power / 50, () => {
      Camera.main.Kick (bullet.velocity * (bullet.power / 2));

      if (hasShield && shieldHealth >= 0) {
        if (bullet.color == shieldColor) {
          float tempShieldHealth = this.shieldHealth;
          this.shieldHealth -= bullet.power;
          bullet.power -= tempShieldHealth;
          
          if (this.shieldHealth <= 0) {
            this.outline.Clear();
            this.outline.enabled = false;
            this.hasShield = false;
          }
          else {
            DisplayUtil.FlashOutlineWhite(this, this.outline, bullet.power/40);
          }
        } else {
          var offsetVector = (bullet.transform.position - transform.position).normalized * bullet.velocity.magnitude;
          bullet.velocity = offsetVector;
        }

      } else {
        float tempHealth = this.health;
        this.health -= bullet.power;
        bullet.power -= tempHealth;
      }

      Signals.Get<HitByBulletSignal> ().Dispatch (this.type, bullet.owner, this != null && this.health <= 0);

      if (bullet != null && bullet.power <= 0) {
        bullet.Die ();
      }
      if (this != null && this.health <= 0) {
        this.Die (true);
      }
    }));
  }
  public virtual void Die (bool killedByPlayer) {
    if (killedByPlayer) {
      Camera.main.Shake ();
      GameObject explosionCopy = Instantiate (explosion);
      explosionCopy.transform.position = transform.position + (Vector3) offset;

      if (GameManager.instance != null) {
        GameManager.instance.AddScore (100, this.transform.position + (Vector3) offset + Vector3.left);
        GameManager.instance.IncrementCombo ();
      }
    }
    Destroy (gameObject);
  }
  public virtual void Spawn (Side side) {
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

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.GetComponent<Bullet> () != null && other.GetComponent<Bullet> ().team == BulletTeam.PLAYER && !GameManager.instance.paused) {
      TakeHit (other.GetComponent<Bullet> ());
    }
  }

  public virtual void OnDestroy () {
    GameManager.instance.DeregisterEnemy (this);
  }
}