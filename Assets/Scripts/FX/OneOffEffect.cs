using UnityEngine;
using System.Collections;
using PowerTools;

/// Plays an animation and then destroys itself
[RequireComponent(typeof(SpriteAnim))]
public class OneOffEffect : MonoBehaviour {
  public SpriteAnim m_spriteAnim = null;

  public SoundType soundToPlay;
  public float volume = 1;



  // Use this for initialization
  void Start() {
    m_spriteAnim = GetComponent<SpriteAnim>();
    GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;

    SfxManager.instance.PlaySound(soundToPlay, volume);

  }

  // Update is called once per frame
  void Update() {
    // If the animation has finished playing, destroy the object
    if (m_spriteAnim.IsPlaying() == false) {
      Destroy(gameObject);
    }

  }
}
