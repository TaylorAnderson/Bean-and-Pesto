using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class MusicStarter : MonoBehaviour {
  public SoundType musicToStart;
  public bool loop = true;
  public float volume = 1;
  // Start is called before the first frame update
  void OnEnable() {
    StartCoroutine(Init());
  }

  public IEnumerator Init() {
    yield return new WaitForSeconds(0.1f);
    SfxManager.instance.PlaySound(musicToStart, volume, loop);
  }

  // Update is called once per frame
  void Update() {

  }
}
