using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour {
  public GameObject star;
  private float travelSpeed;
  private float z;

  private List<GameObject> objects = new List<GameObject>();
  // Start is called before the first frame update
  void Start() {

    for (int i = 0; i < 50; i++) {
      var starCopy = Instantiate(star);
      var script = starCopy.GetComponent<Star>();

      starCopy.transform.position = transform.position;

      starCopy.transform.parent = this.transform.parent;
      script.rightmost = transform.position.x;
      script.z = GetRandomWeightedIndex(new float[] { 1, 2, 3 });
      script.Init(false);
    }
  }

  public int GetRandomWeightedIndex(float[] weights) {
    if (weights == null || weights.Length == 0) return -1;
    float w;
    float total = 0;
    for (int i = 0; i < weights.Length; i++) {
      w = weights[i];
      if (float.IsPositiveInfinity(w)) return i;
      else if (w >= 0f && !float.IsNaN(w)) total += weights[i];
    }

    float r = Random.value;
    float s = 0f;

    for (int i = 0; i < weights.Length; i++) {
      w = weights[i];
      if (float.IsNaN(w) || w <= 0f) continue;

      s += w / total;
      if (s >= r) return i;
    }

    return -1;
  }


}
