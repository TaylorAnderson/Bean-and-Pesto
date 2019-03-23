using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
  public float z;
  private float travelSpeed;
  public Sprite[] sprites;

  public float rightmost;
  private float leftmost;

  // Start is called before the first frame update
  void Start() {

  }

  public void Init(bool startRight) {

    leftmost = -20;
    var pos = transform.position;
    if (startRight) pos.x = rightmost;
    else pos.x = Random.Range(leftmost, rightmost);

    transform.position = pos;

    travelSpeed = MathUtil.Map(z, 1, 3, 0.1f, 0.025f);
    var spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.sortingOrder = Mathf.RoundToInt(10 - z);
    //Debug.Log(Mathf.FloorToInt(2-z));
    spriteRenderer.sprite = this.sprites[Mathf.RoundToInt(2 - z)];
    //this.transform.localScale = Vector3.right * (3-z) + Vector3.up + Vector3.forward;

    var randomY = Random.value;
    var randomYTranslated = Camera.main.ViewportToWorldPoint(new Vector3(0, randomY, 1)).y;
    transform.position += Vector3.up * randomYTranslated;
  }

  // Update is called once per frame
  void Update() {
    transform.position += Vector3.left * travelSpeed;
    if (transform.position.x < leftmost) {
      Init(true);
    }
  }

}
