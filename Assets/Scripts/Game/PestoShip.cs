using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using deVoid.Utils;
using System;
using Random = UnityEngine.Random;

public struct AttackData {
  public EntityType victim;
  public EntityType attacker;
  public bool entityKilled;
  public AttackData(EntityType victim, EntityType attacker, bool entityKilled) {
    this.victim = victim;
    this.attacker = attacker;
    this.entityKilled = entityKilled;
  }
}

public struct PestoDialogue {
  public string dialogue;
  public PestoEmote pestoEmote;
  public PestoDialogue(string dialogue, PestoEmote pestoEmote) {
    this.dialogue = dialogue;
    this.pestoEmote = pestoEmote;
  }
}



public class PestoShip : Ship {
  private float bulletInterval = 0.06f;
  private float bulletCounter = 0;

  //i want to pick a conditional out of a list, and have a response mapped to that
  private List<Func<AttackData, PestoDialogue>> attackResponses = new List<Func<AttackData, PestoDialogue>>();
  private List<Func<EntityType, PestoDialogue>> spawnResponses = new List<Func<EntityType, PestoDialogue>>();

  override public void Start() {
    base.Start();
    this.type = EntityType.PESTO;

    Signals.Get<HitByBulletSignal>().AddListener(OnBulletHit);
    Signals.Get<SpawningSignal>().AddListener(OnEnemySpawn);

    /* attackResponses.Add((AttackData data) => {
      return new PestoDialogue("<j=crazy>See you later, fucko!!!", PestoEmote.ANGRY);
    });

    spawnResponses.Add((EntityType entitySpawning) => {
      return new PestoDialogue("We got company!!!", PestoEmote.SCARED);
    });*/
  }

  void OnBulletHit(AttackData attackData) {
    bool giveResponse = Random.value > 0.9f;
    if (giveResponse == true) {
      foreach (Func<AttackData, PestoDialogue> func in attackResponses) {
        PestoDialogue dialogue = func(attackData);
        if (dialogue.dialogue != "") {
          Signals.Get<ShowDialogueMessageSignal>().Dispatch(dialogue.dialogue, dialogue.pestoEmote, true);
          attackResponses.Remove(func);
          break;
        }
      }
    }
  }

  void OnEnemySpawn(EntityType enemySpawning) {
    bool giveResponse = Random.value > 0.9f;
    if (giveResponse == true) {
      foreach (Func<EntityType, PestoDialogue> func in spawnResponses) {
        PestoDialogue dialogue = func(enemySpawning);
        if (dialogue.dialogue != "") {
          Signals.Get<ShowDialogueMessageSignal>().Dispatch(dialogue.dialogue, dialogue.pestoEmote, true);
          spawnResponses.Remove(func);
          break;
        }
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
