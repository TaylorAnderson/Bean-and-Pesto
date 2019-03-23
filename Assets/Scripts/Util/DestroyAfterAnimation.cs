using UnityEngine;
using System.Collections;
using PowerTools;

/// Plays an animation and then destroys itself
[RequireComponent(typeof(SpriteAnim))]
public class DestroyAfterAnimation : MonoBehaviour
{
    SpriteAnim m_spriteAnim = null;

    // Use this for initialization
    void Start ()
    {
        // Store the sprite animation component so we don't have to get it again every frame
        m_spriteAnim = GetComponent<SpriteAnim>();

    }

    // Update is called once per frame
    void Update ()
    {
        // If the animation has finished playing, destroy the object
        if ( m_spriteAnim.IsPlaying() == false ) {
          Destroy(gameObject);
        }

    }
}
