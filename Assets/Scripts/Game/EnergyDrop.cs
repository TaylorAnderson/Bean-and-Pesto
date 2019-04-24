using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : MonoBehaviour {

  public ShieldColors shieldColor;
  [Autohook]
  public SpriteRenderer sprite;

  private float toggleTime = 0.07f;
  private float toggleTimer = 0;

  private Color originalColor;

  [HideInInspector]
  public Vector2 velocity;


  public bool followingActiveShip = false;

  private float lifetime = 4;




  // Start is called before the first frame update
  void Start() {
    ColorUtility.TryParseHtmlString("#ffef56", out originalColor);
  }

  // Update is called once per frame
  void Update() {


    if (followingActiveShip) {
      var activeShip = Ship.ActiveShip.disabled ? Ship.ActiveShip.otherShip : Ship.ActiveShip;
      Vector2 rawFollowVector = activeShip.transform.position - transform.position;
      Vector2 followVector = rawFollowVector * 0.2f;
      this.velocity = followVector;
    }
    else {
      velocity *= 0.95f;
    }

    lifetime -= Time.deltaTime;
    toggleTimer += Time.deltaTime;
    if (toggleTimer > toggleTime) {
      if (lifetime > 1) {
        sprite.color = sprite.color == Color.white ? originalColor : Color.white;
      }
      else sprite.enabled = !sprite.enabled;

      toggleTimer = 0;
    }

    if (lifetime <= 0) {
      Destroy(this.gameObject);

    }


    transform.position += (Vector3)this.velocity;
  }


}


