using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

  void OnEnable() {
    SpawnManager.instance.StartGame();
  }
}
