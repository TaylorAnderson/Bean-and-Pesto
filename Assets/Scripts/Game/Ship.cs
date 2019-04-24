using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;
using deVoid.Utils;
using StateMachineUtil;
public enum ShipState {
  ACTIVE,
  INACTIVE,
  SWITCHING
}
public class Ship : Entity {
  public CharacterActions input = new CharacterActions();
  public GameObject bullet;
  public Transform bulletOrigin;
  public bool active = false;
  [HideInInspector]
  public Ship otherShip;
  public static Ship ActiveShip;

  public SpriteRenderer shipSprite;
  public SpriteRenderer characterSprite;
  public Sprite shootCharacterSprite;
  public EnergyBar energyBar;
  public float energy = 100;

  public SwapFX swapFX;

  public GameObject reviveFX;

  protected Vector2 velocity = new Vector2();
  protected float bulletSpeed = 0.5f;

  private StateMachine<ShipState> fsm = new StateMachine<ShipState>(ShipState.ACTIVE);

  private Sprite defaultCharacterSprite;
  public bool disabled = false;
  private Color darkColor = new Color(0.6f, 0.6f, 0.6f);
  private Color lightColor = new Color(1f, 1f, 1f);

  private float switchTimer = 0;
  new private Collider2D collider;
  private float switchCooldownTime = 1;
  private float switchCooldownTimer;


  private bool invincible = false;
  private float invincibleFlickerTimer = 0;
  private float invincibleTime = 2;
  private float invincibleTimer = 0;
  public override void Start() {
    base.Start();
    swapFX.Kill();
    if (active) {
      Ship.ActiveShip = this;
      fsm.ChangeState(ShipState.ACTIVE);
    }
    else {
      fsm.ChangeState(ShipState.INACTIVE);
    }
    this.collider = GetComponent<Collider2D>();
    input = new CharacterActions();
    input.Left.AddDefaultBinding(Key.A);
    input.Left.AddDefaultBinding(Key.LeftArrow);
    input.Left.AddDefaultBinding(InputControlType.LeftStickLeft);

    input.Right.AddDefaultBinding(Key.D);
    input.Right.AddDefaultBinding(Key.RightArrow);
    input.Right.AddDefaultBinding(InputControlType.LeftStickRight);

    input.Down.AddDefaultBinding(Key.DownArrow);
    input.Down.AddDefaultBinding(Key.S);
    input.Down.AddDefaultBinding(InputControlType.LeftStickDown);

    input.Up.AddDefaultBinding(Key.UpArrow);
    input.Up.AddDefaultBinding(Key.W);
    input.Up.AddDefaultBinding(InputControlType.LeftStickUp);

    input.Shoot.AddDefaultBinding(Key.Space);
    input.Shoot.AddDefaultBinding(InputControlType.Action1);
    input.Shoot.AddDefaultBinding(Key.Z);
    input.Shoot.AddDefaultBinding(Mouse.LeftButton);

    input.Switch.AddDefaultBinding(Key.Q);
    input.Switch.AddDefaultBinding(Key.E);
    input.Switch.AddDefaultBinding(Key.X);
    input.Switch.AddDefaultBinding(Mouse.RightButton);
    input.Switch.AddDefaultBinding(InputControlType.Action4);

    this.defaultCharacterSprite = characterSprite.sprite;


    this.fsm.Bind(ShipState.ACTIVE, OnActiveStart, OnActiveUpdate, OnActiveEnd);
    this.fsm.Bind(ShipState.INACTIVE, OnInactiveStart, OnInactiveUpdate, OnInactiveEnd);
    this.fsm.Bind(ShipState.SWITCHING, OnSwitchStart, OnSwitchUpdate, OnSwitchEnd);
  }

  public void OnActiveStart() {
    this.active = true;

  }

  public void OnActiveUpdate() {
    if (!this.disabled) this.Shoot(input.Shoot);
    characterSprite.sprite = input.Shoot.IsPressed ? shootCharacterSprite : defaultCharacterSprite;
    velocity += input.Move.Value * Time.deltaTime;
    switchCooldownTimer -= Time.deltaTime;

    if (input.Switch.WasPressed && switchCooldownTimer <= 0) {
      StartCoroutine(SwitchShip());
    }
  }

  public void OnActiveEnd() {

  }

  public void OnInactiveStart() {
    this.active = false;
  }

  public void OnInactiveUpdate() {

    characterSprite.sprite = defaultCharacterSprite;
    if (!disabled && energy < 100) {
    }
    Vector2 followVec = otherShip.transform.position + (Vector3.left * 3.5f) - this.transform.position;
    float dist = followVec.magnitude;
    velocity += followVec.normalized * dist / 2 * Time.deltaTime;

  }

  public void OnInactiveEnd() {

  }

  public virtual void OnSwitchStart() {
    this.Switch();
    this.switchTimer = 0;
    swapFX.Init();
    //this.collider.enabled = false;
    if (active) {
      Ship.ActiveShip = this.otherShip;
      var mid = (Vector2)(this.transform.position + otherShip.transform.position) / 2;
      var diffVec = (transform.position - otherShip.transform.position).normalized;
      var orthAngle = Mathf.Atan2(diffVec.y, diffVec.x) + Mathf.PI / 2;
      var orthVector = new Vector2(Mathf.Cos(orthAngle), Mathf.Sin(orthAngle)).normalized;
      StartCoroutine(Auto.CurveTo(transform, mid + orthVector * -2.5f, otherShip.transform.position, 0.3f));
      StartCoroutine(Auto.CurveTo(otherShip.transform, mid + orthVector * 2.5f, transform.position, 0.3f));
    }
    else {

    }
  }

  public void OnSwitchUpdate() {
    if (active) {
      this.switchTimer += Time.deltaTime;
      if (this.switchTimer > 0.3f) {

        this.fsm.ChangeState(ShipState.INACTIVE);
        this.otherShip.fsm.ChangeState(ShipState.ACTIVE);
      }
    }

  }

  public void OnSwitchEnd() {
    swapFX.Kill();
    switchCooldownTimer = switchCooldownTime;
  }

  public override void DoUpdate() {
    this.transform.position += (Vector3)velocity;
  }

  public override void Update() {
    base.Update();
    this.fsm.Update();

    this.velocity *= 0.93f;

    this.energyBar.SetEnergy(this.energy);
    this.energy = Mathf.Clamp(this.energy, 0, 199);

    if (energy <= 0 && !disabled) {
      this.disabled = true;
      this.energyBar.gameObject.SetActive(false);

      if (this.otherShip.disabled) {
        GameManager.instance.EndGame();
      }
      else if (otherShip.energy < 199) {
        StartCoroutine(SwitchShip());
      }
    }

    if (disabled) {
      if (this.otherShip.energy == 199) {
        this.disabled = false;
        this.energyBar.gameObject.SetActive(true);
        this.energy = 99;
        otherShip.energy = 99;
        var reviveEffect = Instantiate(reviveFX);
        reviveEffect.transform.position = transform.position;
        StartCoroutine(GameManager.instance.PauseForSeconds(0.6f));
      }

      this.shipSprite.color = darkColor;
      this.characterSprite.color = darkColor;
    }
    else {
      this.shipSprite.color = lightColor;
      this.characterSprite.color = lightColor;
    }


    if (this.invincible) {
      this.invincibleFlickerTimer += Time.deltaTime;
      if (invincibleFlickerTimer > 0.05f) {
        this.shipSprite.enabled = this.characterSprite.enabled = !this.shipSprite.enabled;
        invincibleFlickerTimer = 0;
      }

      this.invincibleTimer += Time.deltaTime;
      if (this.invincibleTimer > this.invincibleTime) {
        this.invincibleTimer = 0;
        this.invincible = false;
        this.shipSprite.enabled = this.characterSprite.enabled = true;
      }
    }
  }

  IEnumerator SwitchShip() {



    yield return new WaitForSecondsRealtime(0.1f);

    if (this.active) {//who this actually is isnt relevant, its just so that only one ship plays the sound{
      SfxManager.instance.PlaySound(SoundType.SHIP_SWITCH);
    }
    fsm.ChangeState(ShipState.SWITCHING);
    otherShip.fsm.ChangeState(ShipState.SWITCHING);
  }

  virtual public void Shoot(PlayerAction shootAction) {

  }
  virtual public void Switch() {

  }

  public virtual void TakeHit(Bullet bullet) {
    if (!disabled) {
      SfxManager.instance.PlaySound(SoundType.SHIP_HURT);
      if (this.energy > 100) this.energy = 100;
      this.energy -= bullet.power * 20;

      float pauseTime = bullet.power / 15;
      DisplayUtil.FlashWhite(this, this.shipSprite, pauseTime);
      DisplayUtil.FlashWhite(this, this.characterSprite, pauseTime);
      GameManager.instance.ClearCombo();
      StartCoroutine(GameManager.instance.PauseForSeconds(pauseTime, () => {
        if (bullet != null) bullet.Die();
        Signals.Get<HitByBulletSignal>().Dispatch(this.type, bullet.owner, energy <= 0);
        Camera.main.Shake(1f);
      }));
    }

    else {
      SfxManager.instance.PlaySound(SoundType.SHIP_HURT_AFTER_DEAD);
      invincible = true;
      Camera.main.GetComponentInChildren<HurtBG>().Show();
      StartCoroutine(GameManager.instance.PauseForSeconds(bullet.power / 10, () => {
        if (bullet != null) bullet.Die();
        Signals.Get<HitByBulletSignal>().Dispatch(this.type, bullet.owner, energy <= 0);
        Camera.main.Shake(1.3f);
      }));
      GameManager.instance.AddScore(-1000, transform.position);

    }

  }

  private void OnTriggerEnter2D(Collider2D other) {

    if (other.GetComponent<Bullet>() != null && other.GetComponent<Bullet>().team == BulletTeam.ENEMIES && this.active && !this.invincible) {
      TakeHit(other.GetComponent<Bullet>());
    }

    if (other.transform.parent) {
      EnergyDrop energyDropRange = other.transform.parent.GetComponent<EnergyDrop>();
      if (energyDropRange != null && (this.active || this.otherShip.disabled)) {
        energyDropRange.followingActiveShip = true;
      }
    }


    EnergyDrop energyDrop = other.GetComponent<EnergyDrop>();
    if (energyDrop != null && (this.active || otherShip.disabled) && !this.disabled) {
      //it has no parent, so it's the real honest-to-god energy drop
      //this sucks but im tired
      float randomVal = Random.value;

      SfxManager.instance.PlaySound(randomVal < 0.33f ? SoundType.ENERGY_COLLECTED_1 : randomVal < 0.66 ? SoundType.ENERGY_COLLECTED_2 : SoundType.ENERGY_COLLECTED_3);
      Destroy(energyDrop.gameObject);
      Debug.Log("getting energy");
      this.energy += 5;
    }
  }


}
