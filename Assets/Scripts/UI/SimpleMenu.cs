using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
public class SimpleMenu : MonoBehaviour {
  private CharacterActions input;
  // Start is called before the first frame update
  void Start() {
    input = new CharacterActions();
    input.Select.AddDefaultBinding(Key.Space);
    input.Select.AddDefaultBinding(Key.Z);
    input.Select.AddDefaultBinding(Mouse.LeftButton);
    input.Select.AddDefaultBinding(InputControlType.Action1);

  }

  // Update is called once per frame
  void Update() {
    if (input.Select.WasPressed) {
      SfxManager.instance.StopAllSounds();
      SfxManager.instance.PlaySound(SoundType.SELECT);
      SceneManager.LoadSceneAsync(1);
    }
  }
}
