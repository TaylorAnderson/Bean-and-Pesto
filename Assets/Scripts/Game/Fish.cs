using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Enemy {

  [HideInInspector] public FishSchool school;


  private float waveOffset;

  private float waveWidth;
  private float waveHeight;


  public bool debug;

  override public void Start() {
    base.Start();
    this.type = EntityType.FISH;
  }

  // Update is called once per frame
  override public void DoUpdate() {
    base.DoUpdate();
    var sinVal = Mathf.Sin((Time.time + waveOffset) * waveWidth) * waveHeight / 2;
    var vel = Vector3.up * sinVal + Vector3.left * 0.08f;
    transform.position += vel;


    float speed = 0.6f;

    float t = (Mathf.Sin((Time.time + waveOffset) * speed * Mathf.PI * 2.0f) + 1.0f) / 2.0f;

    transform.eulerAngles = Vector3.forward * (Mathf.Lerp(5, -5, t));
  }

  public void Init(FishSchool school) {
    waveOffset = MathUtil.Map(this.transform.position.x, school.leftmost, school.rightmost, 0.25f, 0);
    waveWidth = school.waveWidth;
    waveHeight = school.waveHeight;
    this.killShakeMultipler = 0f;
    this.hitStopMultiplier = 0f;
  }


}
