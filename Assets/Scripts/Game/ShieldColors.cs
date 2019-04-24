using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldColors {
  PESTO = 0x82d447,
  BEAN = 0x76a7ff,
  ENEMY = 0x9176ff,
  NONE = 0,
}

class ShieldColorMap {
  public static Dictionary<ShieldColors, string> map = new Dictionary<ShieldColors, string> {
    { ShieldColors.PESTO, "#82d447" },
    { ShieldColors.BEAN, "#76a7ff" },
    { ShieldColors.ENEMY, "#9176ff" },
    { ShieldColors.NONE, "#000000" }
  };

}

