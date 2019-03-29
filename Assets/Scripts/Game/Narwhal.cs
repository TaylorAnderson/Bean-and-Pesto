using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narwhal : Enemy {

  private float chargeSpeed = 0.01f;
  private SniperBullet currentBullet = null;
  public SniperBullet bullet = null;
  public GameObject particle;
  private float bulletDelayTimer = 0;
  private float bulletDelay = 3;
  public Sprite angledSprite;
  private Sprite normalSprite;
  // Start is called before the first frame update
  override public void Start() {
    base.Start();
    this.normalSprite = this.sprite.sprite;
    UpdateOutline(true);
    
  }
  void UpdateOutline(bool outline) {
      MaterialPropertyBlock mpb = new MaterialPropertyBlock();
      this.sprite.GetPropertyBlock(mpb);
      mpb.SetFloat("_Outline", outline ? 1f : 0);
      mpb.SetColor("_OutlineColor", Color.white);
      mpb.SetFloat("_OutlineSize", 1);
      this.sprite.SetPropertyBlock(mpb);
  }
  // Update is called once per frame
   override public void Update() {
    base.Update();
    if (currentBullet == null) {
      Debug.Log("current bullet is null");
      this.bulletDelayTimer -= Time.deltaTime;
      if (bulletDelayTimer <= 0) {
        particle.SetActive(true);
        this.currentBullet = Instantiate(bullet).GetComponent<SniperBullet>();
        this.currentBullet.power = 0.1f;
        this.currentBullet.transform.localScale = (Vector3)Vector2.one * 0.1f;
        this.currentBullet.GetComponent<Collider2D>().enabled = false;
        this.currentBullet.owner = this.type;
        
        currentBullet.transform.position = this.transform.position + Vector3.left*this.sprite.bounds.extents.x;
        this.bulletDelayTimer = bulletDelay;
      }
    }
    else {
      currentBullet.lifetime = 1;
      if (currentBullet.power < 3f) currentBullet.power += chargeSpeed * 2;
      else currentBullet.power = 3;

      if (currentBullet.transform.localScale.x < 1.5f) {
        currentBullet.transform.localScale += (Vector3)Vector2.one * chargeSpeed;
        Debug.Log(currentBullet.transform.localScale);

        if (currentBullet.transform.localScale.x > 1f) {
          this.sprite.sprite = angledSprite;
        }
      }
      else {
        this.sprite.sprite = normalSprite;
        
        particle.SetActive(false);
        currentBullet.velocity = Vector2.left*0.4f;
        currentBullet.shrinkMultiplier = 0.4f;
        currentBullet.shrinking = true;
        Camera.main.Kick(Vector2.left);
        
        this.currentBullet.GetComponent<Collider2D>().enabled = true;
        currentBullet = null;
      }
    }
    

  }
}
