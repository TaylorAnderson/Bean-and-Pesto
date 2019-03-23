using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {
  public Transform bar;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void SetEnergy(float energyAmt) {
    this.bar.localScale = Vector3.up + Vector3.forward + Vector3.right * MathUtil.Map(energyAmt, 0, 100, 0, 1);
  }
}
