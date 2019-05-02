using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Util {

  public static void Shuffle<T>(List<T> ts) {
    var count = ts.Count;
    var last = count - 1;
    for (var i = 0; i < last; ++i) {
      var r = UnityEngine.Random.Range(i, count);
      var tmp = ts[i];
      ts[i] = ts[r];
      ts[r] = tmp;
    }
  }
}
