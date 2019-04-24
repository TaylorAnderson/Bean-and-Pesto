using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapFX : MonoBehaviour {

  public Transform circle;
  new public ParticleSystem particleSystem;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void Init() {
    StartCoroutine(Auto.ScaleTo(circle, Vector3.one, 0.15f, EaseType.BackOut));
    this.GetComponent<Collider2D>().enabled = true;
    //particleSystem.Play();
  }
  public void Kill() {
    circle.localScale = (Vector3)Vector2.zero + Vector3.forward;
    this.GetComponent<Collider2D>().enabled = false;
    //particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    var bullet = other.GetComponent<Bullet>();
    if (bullet != null && bullet.team == BulletTeam.ENEMIES) {
      SfxManager.instance.PlaySound(SoundType.SHIP_PARRY);
      StartCoroutine(GameManager.instance.PauseForSeconds(0.1f));
      var newVec = (bullet.transform.position - transform.position).normalized * 0.5f;
      bullet.velocity = bullet.velocity * -1;//newVec;
      bullet.team = BulletTeam.PLAYER;
      bullet.lifetime += 2;
      bullet.power++;
    }
  }
}
