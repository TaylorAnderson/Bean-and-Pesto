using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class MusicStarter : MonoBehaviour {
  public SoundType musicToStart;
  public float volume = 1;
  // Start is called before the first frame update
  void Start() {
    Signals.Get<SfxManagerInitSignal>().AddListener(Init);
  }

  public void Init() {
    SfxManager.instance.PlaySound(musicToStart, volume, true);
  }

  // Update is called once per frame
  void Update() {

  }
}
