using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
  public float z;
  private float travelSpeed;
  public Sprite[] sprites;
  public float speedMultiplier = 1;

  public float rightmost;
  public Gradient gradient;
  private float leftmost;

  public SpriteRenderer spriteRenderer;

  // Start is called before the first frame update
  void Start() {


  }

  public void Init(bool startRight) {
    spriteRenderer = GetComponent<SpriteRenderer>();
    leftmost = -20;
    var pos = transform.position;
    if (startRight) pos.x = rightmost;
    else pos.x = Random.Range(leftmost, rightmost);

    transform.position = pos;

    travelSpeed = MathUtil.Map(z, 1, 3, 0.1f, 0.025f);
    spriteRenderer.sortingOrder = Mathf.RoundToInt(10 - z);
    //Debug.Log(Mathf.FloorToInt(2-z));
    spriteRenderer.sprite = this.sprites[Mathf.RoundToInt(2 - z)];


    var randomY = Random.value;
    var randomYTranslated = Camera.main.ViewportToWorldPoint(new Vector3(0, randomY, 1)).y;
    transform.position += Vector3.up * randomYTranslated;
  }

  // Update is called once per frame
  void Update() {
    transform.position += Vector3.left * travelSpeed * speedMultiplier;
    this.transform.localScale = Vector3.right * MathUtil.Map(travelSpeed * speedMultiplier, 0.025f, 0.5f, 1, 3f) + Vector3.up + Vector3.forward;
    this.spriteRenderer.color = gradient.Evaluate(MathUtil.Map(this.travelSpeed * speedMultiplier, 0.025f, 0.5f, 0, 1));

    if (transform.position.x < leftmost) {
      Init(true);
    }
  }

}
