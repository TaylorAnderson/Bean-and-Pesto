using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestoBullet : Bullet {

  private SpriteRenderer outlineSprite;
  override public void Start() {
    base.Start();
    outlineSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    StartCoroutine(DisplayUtil.FlashWhiteForSeconds(this.GetComponent<SpriteRenderer>(), 0.1f));
    StartCoroutine(DisplayUtil.FlashWhiteForSeconds(this.transform.GetChild(0).GetComponent<SpriteRenderer>(), 0.1f));
    StartCoroutine(ShowOutlineForSeconds(0.1f));
    this.transform.localScale -= Vector3.right * 0.3f;
    this.transform.localScale += Vector3.up * 0.5f;

  }
  override public void Update() {
    base.Update();
    if (this.transform.localScale.x < 0.75f) this.transform.localScale += Vector3.right * 0.05f;
    if (this.transform.localScale.y > 0.75f) this.transform.localScale -= Vector3.up * 0.05f;
  }

  IEnumerator ShowOutlineForSeconds(float seconds) {
    Color color = this.outlineSprite.color;
    color.a = 1;
    this.outlineSprite.color = color;
    yield return new WaitForSeconds(seconds);
    color.a = 0.5f;
    this.outlineSprite.color = color;
  }
}
