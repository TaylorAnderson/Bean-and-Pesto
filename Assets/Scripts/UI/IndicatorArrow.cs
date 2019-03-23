using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorArrow : MonoBehaviour {
  private RectTransform rectTransform;
  private float counter = 0;

  private float speed = 20;
  private float distance = 1.5f;
  public RectTransform image;
  // Start is called before the first frame update
  void Start() {
    rectTransform = GetComponent<RectTransform>();
  }

  // Update is called once per frame
  void Update() {
    counter += Time.deltaTime;
    image.anchoredPosition += Vector2.down * Mathf.Sin(counter * speed) * distance;
  }
}
