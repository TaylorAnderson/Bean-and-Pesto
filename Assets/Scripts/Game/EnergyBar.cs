using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;
public class EnergyBar : MonoBehaviour {
  public Sprite[] barFrames;
  public SpriteRenderer energyBarSprite;
  [HideInInspector]
  public int overloadFrame = 22;
  public AnimationClip energyBoltOverload;
  public AnimationClip energyBoltOff;
  public SpriteAnim energyBoltSprite;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void SetEnergy(float energyAmt) {
    energyBarSprite.sprite = this.barFrames[Mathf.Clamp(Mathf.RoundToInt(MathUtil.Map(energyAmt, 0, 100, 0, overloadFrame - 1)), 0, barFrames.Length - 1)];
    if (energyAmt == 199 && energyBoltSprite.GetCurrentAnimation() != energyBoltOverload) {
      energyBoltSprite.Play(energyBoltOverload);
    }
    if (energyAmt < 199 && gameObject.activeSelf) {
      energyBoltSprite.Play(energyBoltOff);
    }
  }
}
