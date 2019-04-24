using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class PufferFish : Enemy {

  public GameObject spike;
  public SpriteRenderer body;
  public SpriteRenderer fin;

  public AnimationClip faceAnim;
  public AnimationClip spikeAnim;
  public SpriteAnim faceAnimator;
  public SpriteAnim spikeAnimator;

  private Vector3 currentTarget;
  private Vector2 velocity;
  private float travelTimer = 1;

  private float spikeInterval = 1;
  private float spikeTimer = 0;
  // Start is called before the first frame update
  override public void Start() {
    base.Start();
  }

  // Update is called once per frame
  override public void DoUpdate() {
    base.DoUpdate();
    spikeTimer += Time.deltaTime;
    if (spikeTimer > spikeInterval) {
      SfxManager.instance.PlaySound(SoundType.PUFFER_SPIKE);
      SendSpikes(10);
      spikeTimer = 0;
    }

    fin.transform.position = Vector2.right * (body.transform.position.x + body.bounds.extents.x + fin.bounds.extents.x - 0.1f) + Vector2.up * fin.transform.position.y;
    if (travelTimer > 0) {
      Vector2 pointer = this.currentTarget - this.transform.position;
      float dist = pointer.magnitude;
      this.velocity += pointer.normalized * dist * 0.008f;
      this.velocity *= 0.9f;
      travelTimer -= Time.deltaTime;
      this.transform.position += (Vector3)velocity;

    }
    else {
      transform.position += Vector3.up * Mathf.Sin(Time.time * 4) * 0.03f;
    }

  }

  override public void TakeHit(Bullet bullet) {
    base.TakeHit(bullet);

    if (body.transform.localScale.x < 0.4f) {
      if (health > 3) health = 3f;
    }
    else {

      body.transform.localScale -= (Vector3)Vector2.one * 0.05f;

      SendSpikes(4);

    }


  }

  private void SendSpikes(int amt) {
    faceAnimator.Play(faceAnim);
    spikeAnimator.Play(spikeAnim);
    for (int i = 0; i < amt; i++) {
      var spikeCopy = Instantiate(spike);
      spikeCopy.transform.position = transform.position;
      var randAngle = Random.Range(0, 360);
      var spikeBullet = spikeCopy.GetComponent<Bullet>();
      spikeBullet.velocity = new Vector2(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)).normalized * 0.25f;
      spikeBullet.angleOffset = 180;
    }
  }

  override public void Spawn(Side side) {

    this.transform.position += Vector3.up * body.bounds.extents.y / 2;
    this.currentTarget = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.5f, 0));

    if (side == Side.BOTTOM || side == Side.TOP) {
      this.currentTarget.x = this.transform.position.x;
    }
    else {
      this.currentTarget.y = this.transform.position.y;
    }
  }
}
