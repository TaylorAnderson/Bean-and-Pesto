using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType {
  BEAN,
  PESTO,
  SHIP,
  SHARK,
  JELLY,
  FISH,
  PUFFER,
  NARWHAL,
  UNASSIGNED
}
public class Entity : MonoBehaviour {
  public EntityType type = EntityType.UNASSIGNED;
  public virtual void Start() {
  }
  public virtual void Update() {
    if (!GameManager.instance.paused) {
      DoUpdate();
    }
  }

  public virtual void DoUpdate() {
    //override this
  }

  void OnPauseGame() {

  }
}
