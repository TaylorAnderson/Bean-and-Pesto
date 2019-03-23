using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DisplayUtil {
  public static void FlashWhite(MonoBehaviour script, SpriteRenderer sprite, float timeInSeconds) {
    script.StartCoroutine(FlashWhiteForSeconds(sprite, timeInSeconds));
  }
  public static IEnumerator FlashWhiteForSeconds(SpriteRenderer sprite, float seconds) {
    Shader shaderGUItext = Shader.Find("GUI/Text Shader");
    Shader normalShader = Shader.Find("Sprites/Default");
    sprite.material.shader = shaderGUItext;
    sprite.color = Color.white;
    yield return new WaitForSeconds(seconds);
    sprite.material.shader = normalShader;
    sprite.color = Color.white;

  }
}
