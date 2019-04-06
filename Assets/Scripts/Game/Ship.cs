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
  public Ship otherShip;
  public static Ship ActiveShip;

  public SpriteRenderer shipSprite;
  public SpriteRenderer characterSprite;
  public Sprite shootCharacterSprite;
  public EnergyBar energyBar;
  public float energy = 100;

  public SwapFX swapFX;

  protected Vector2 velocity = new Vector2();
  protected float bulletSpeed = 0.5f;

  private StateMachine<ShipState> fsm = new StateMachine<ShipState>(ShipState.ACTIVE);

  private Sprite defaultCharacterSprite;
  private bool disabled = false;
  private Color darkColor = new Color(0.6f, 0.6f, 0.6f);
  private Color lightColor = new Color(1f, 1f, 1f);

  private float switchTimer = 0;
  new private Collider2D collider;
  private float switchCooldownTime = 1;
  private float switchCooldownTimer;
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

    if (input.Switch.WasPressed && !this.otherShip.disabled && switchCooldownTimer <= 0) {
      StartCoroutine(SwitchShip());
    }
  }

  public void OnActiveEnd() {

  }

  public void OnInactiveStart() {

  }

  public void OnInactiveUpdate() {

    characterSprite.sprite = defaultCharacterSprite;
    if (!disabled) {
      this.energy += 0.25f;
    }
    Vector2 followVec = otherShip.transform.position + (Vector3.left * 3.5f) - this.transform.position;
    float dist = followVec.magnitude;
    velocity += followVec.normalized * dist / 2 * Time.deltaTime;

  }

  public void OnInactiveEnd() {
    this.active = false;
  }

  public void OnSwitchStart() {
    this.switchTimer = 0;
    swapFX.Init();
    this.collider.enabled = false;
    if (active) {

      Ship.ActiveShip = this.otherShip;
      var mid = (Vector2)(this.transform.position + otherShip.transform.position) / 2;
      var diffVec = (transform.position - otherShip.transform.position).normalized;
      var orthAngle = Mathf.Atan2(diffVec.y, diffVec.x) + Mathf.PI / 2;
      var orthVector = new Vector2(Mathf.Cos(orthAngle), Mathf.Sin(orthAngle)).normalized;
      Debug.DrawLine(mid, mid + orthVector);
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
    if (!active) collider.enabled = true;
  }

  public override void DoUpdate() {



    this.transform.position += (Vector3)velocity;
  }
  public override void Update() {
    base.Update();
    this.fsm.Update();

    this.velocity *= 0.93f;

    this.energyBar.SetEnergy(this.energy);
    this.energy = Mathf.Clamp(this.energy, 0, 100);

    if (energy <= 0 && !disabled) {
      this.disabled = true;
      if (this.otherShip.disabled) {
        GameManager.instance.EndGame();
      }
      else {
        StartCoroutine(SwitchShip());
      }
    }

    if (disabled) {
      this.shipSprite.color = darkColor;
      this.characterSprite.color = darkColor;
    }
    else {
      this.shipSprite.color = lightColor;
      this.characterSprite.color = lightColor;
    }
  }
  IEnumerator SwitchShip() {

    yield return new WaitForSecondsRealtime(0.1f);

    fsm.ChangeState(ShipState.SWITCHING);
    otherShip.fsm.ChangeState(ShipState.SWITCHING);
  }

  virtual public void Shoot(PlayerAction shootAction) {

  }

  public virtual void TakeHit(Bullet bullet) {
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

  private void OnTriggerEnter2D(Collider2D other) {

    if (other.GetComponent<Bullet>() != null && other.GetComponent<Bullet>().team == BulletTeam.ENEMIES && this.active) {
      TakeHit(other.GetComponent<Bullet>());
    }
  }
}
