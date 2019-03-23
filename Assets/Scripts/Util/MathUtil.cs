using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil {
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public static float Map(float x, float fromMin, float fromMax, float toMin, float toMax) {
    return toMin + ((x - fromMin) / (fromMax - fromMin)) * (toMax - toMin);
  }

  public static float Vector2Angle(Vector2 v) {
    return Mathf.Atan2(v.y, v.x);
  }
}
