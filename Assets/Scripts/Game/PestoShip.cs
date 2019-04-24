using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using deVoid.Utils;

public struct AttackData {
  EntityType victim;
  EntityType attacker;
  bool entityKilled;

}
public class PestoShip : Ship {
  private float bulletInterval = 0.06f;
  private float bulletCounter = 0;

  private Dictionary<AttackData, string> responses = new Dictionary<AttackData, string>();

  override public void Start() {
    base.Start();
    this.type = EntityType.PESTO;

    Signals.Get<HitByBulletSignal>().AddListener(OnBulletHit);
  }

  void OnBulletHit(EntityType victim, EntityType attacker, bool victimKilled) {
    if (victim == EntityType.BEAN || victim == EntityType.PESTO) {
      if (attacker == EntityType.SHARK) {
        Signals.Get<ShowDialogueMessageSignal>().Dispatch("Dumb shark!!! In another life we could've been friends!!!", PestoEmote.ANGRY, true);
      }
      if (victim == EntityType.BEAN && victimKilled) {
        Signals.Get<ShowDialogueMessageSignal>().Dispatch("Don't worry Bean, I'll avenge you!!!", PestoEmote.ANGRY, true);
      }
    }
  }

  override public void Shoot(PlayerAction shootAction) {
    if (shootAction.WasPressed) {
      bulletCounter = bulletInterval;
    }
    if (shootAction.IsPressed) {
      bulletCounter += Time.deltaTime;
      if (bulletCounter > bulletInterval) {
        SfxManager.instance.PlaySound(Random.value > 0.5f ? SoundType.PESTO_SHOOT : SoundType.PESTO_SHOOT_2, 1);
        GameObject bulletCopy = Instantiate(bullet);
        bulletCopy.transform.position = bulletOrigin.position;
        var bulletScript = bulletCopy.GetComponent<Bullet>();
        bulletScript.owner = this.type;
        bulletScript.velocity = Vector2.right * bulletSpeed + Vector2.up * Random.Range(-0.1f, 0.1f);
        bulletScript.power = 0.4f;
        velocity += Vector2.left * bulletCopy.GetComponent<Bullet>().power * Time.deltaTime;
        bulletCounter = 0;
        Camera.main.Shake(0.1f);
      }
    }
  }
}
