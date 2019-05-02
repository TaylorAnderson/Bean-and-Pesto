using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpeedController : MonoBehaviour {
  public float starSpeedMultiplier;
  public float acceleration;
  public StarGenerator starGenerator;
  // Start is called before the first frame update
  void OnEnable() {
    starGenerator.TweenStarsToSpeedMultiplier(starSpeedMultiplier, acceleration);
  }

}
