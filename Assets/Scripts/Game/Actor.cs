using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Actor : Entity {

  public ShieldColors shieldColor;
  public bool hasShield = false;
  public float shieldHealth = 5;
  protected Dictionary<ShieldColors, string> shieldColorMap = new Dictionary<ShieldColors, string> { 
    { ShieldColors.PESTO, "#82d447" },
    { ShieldColors.BEAN, "#76a7ff" },
    { ShieldColors.ENEMY, "#9176ff" },
    { ShieldColors.NONE, "#000000" } 
  };
}
