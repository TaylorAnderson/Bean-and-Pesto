using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUI : MonoBehaviour {
  public IndicatorSprite[] sprites;
  public RectTransform indicatorArrow;
  private Camera cam;

  private RectTransform rectTransform;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }


  public void Init(Entity enemyToIndicate, Side sideToIndicate, float offset) {
    rectTransform = GetComponent<RectTransform>();
    cam = Camera.main;
    for (int i = 0; i < sprites.Length; i++) {
      if (sprites[i].enemyObject.gameObject == enemyToIndicate.gameObject) {
        sprites[i].gameObject.SetActive(true);
      };
    }

    var anchor = new Vector2();
    var position = new Vector2();
    var pivot = new Vector2();
    if (sideToIndicate == Side.TOP) {
      indicatorArrow.localEulerAngles = Vector3.forward * 180;
      anchor.y = 1;
      pivot.y = 1;
      position.x = Camera.main.WorldToScreenPoint(Vector3.right * offset).x;
      position.y = -20f;
    }
    if (sideToIndicate == Side.BOTTOM) {
      anchor.y = 0;
      pivot.y = 0;
      position.x = Camera.main.WorldToScreenPoint(Vector3.right * offset).x;
      position.y = 20f;
    }
    if (sideToIndicate == Side.RIGHT) {
      indicatorArrow.localEulerAngles = Vector3.forward * 90;
      anchor.x = 1;
      pivot.x = 1;
      position.y = Camera.main.WorldToScreenPoint(Vector3.up * offset).y;
      position.x = -10f - indicatorArrow.rect.width;

    }


    this.rectTransform.anchorMax = this.rectTransform.anchorMin = anchor;

    this.rectTransform.pivot = pivot;

    this.rectTransform.anchoredPosition = position;


  }
}
