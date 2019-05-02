using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using deVoid.Utils;
using UnityEngine.SceneManagement;
public enum SoundType {
  NONE,
  BEAN_SHOOT,
  BEAN_CHARGE,
  PESTO_SHOOT,
  PESTO_SHOOT_2,
  SHIP_HURT,
  SHIP_SWITCH,
  SHIP_PARRY,
  ENERGY_COLLECTED_1,
  ENERGY_COLLECTED_2,
  ENERGY_COLLECTED_3,

  ENEMY_HURT,

  EXPLOSION,
  QUIET_EXPLOSION,

  SHIELD_DEFLECT,
  SHIELD_HURT,
  SHIELD_DESTROYED,

  PUFFER_SPIKE,

  SEAHORSE_SHOT,

  SHIP_HURT_AFTER_DEAD,

  SHARK_1,
  SHARK_2,

  SELECT,
  GAME_MUSIC,
  INTRO_MUSIC,
  GAME_OVER_MUSIC
}

[System.Serializable]
public class Sound {
  public AudioClip sound;
  public SoundType type;
}
public class SoundPlayer {
  public AudioSource player;
  public bool inUse = false;
  public SoundPlayer(AudioSource player) {
    this.player = player;
  }
}
public class SfxManagerInitSignal : ASignal { };
public class SfxManager : MonoBehaviour {
  public static SfxManager instance;
  public Sound[] sounds;
  public float volumeMultiplier;

  private List<SoundPlayer> players = new List<SoundPlayer>();

  private List<int> currentlyLoopingSounds = new List<int>();
  private List<SoundType> currentlyPlayingSounds = new List<SoundType>();

  private Dictionary<SoundType, AudioClip> soundDictionary = new Dictionary<SoundType, AudioClip>();


  // Start is called before the first frame update
  void Start() {

    //Check if instance already exists
    if (instance == null)
      instance = this;

    else if (instance != this)
      Destroy(gameObject);

    DontDestroyOnLoad(gameObject);

    for (int i = 0; i < 50; i++) {
      var player = new GameObject();
      var soundPlayer = new SoundPlayer(player.AddComponent<AudioSource>());
      this.players.Add(soundPlayer);
      soundPlayer.player.playOnAwake = false;
      player.transform.parent = transform;
    }


    for (int i = 0; i < sounds.Length; i++) {
      this.soundDictionary[sounds[i].type] = sounds[i].sound;
    }

    Signals.Get<SfxManagerInitSignal>().Dispatch();
  }

  public void Update() {
  }

  public static int PlaySoundStatic(SoundType soundType, float volume = 1, bool looping = false) {
    return SfxManager.instance.PlaySound(soundType, volume, looping);
  }

  public int PlaySound(SoundType soundType, float volume = 1, bool looping = false) {

    if (soundType == SoundType.NONE) return -1;
    if (!looping) {
      var sameSoundsPlaying = 0;
      for (int i = 0; i < this.currentlyPlayingSounds.Count; i++) {
        if (this.currentlyPlayingSounds[i] == soundType) sameSoundsPlaying++;
      }
      if (sameSoundsPlaying > 3) return -1;
    }
    var playerIndex = GetAvailablePlayer();
    var player = this.players[playerIndex];
    if (looping) {
      player.player.clip = this.soundDictionary[soundType];
      player.player.loop = true;
      player.player.volume = this.volumeMultiplier * volume;
      player.player.Play();
      currentlyLoopingSounds.Add(playerIndex);
    }
    else {

      this.currentlyPlayingSounds.Add(soundType);

      player.player.volume = 1;
      player.player.PlayOneShot(this.soundDictionary[soundType], volume * this.volumeMultiplier);
      StartCoroutine(FreeUpSourceAfterSoundEnds(player, this.soundDictionary[soundType], soundType));
    }

    return playerIndex;

  }

  /**
  If playerToken provided is -1, it will return the first player in the pool playing that sound.
   */
  public void StopLoopingSound(SoundType soundType, int playerToken) {

    if (playerToken > this.currentlyLoopingSounds.Count - 1) {
      return;
    }
    if (playerToken == -1) {
      for (int i = 0; i < players.Count; i++) {
        if (players[i].player.clip == soundDictionary[soundType]) {
          playerToken = i;
        }
      }
      if (playerToken == -1) return;
    }
    if (this.players[playerToken] != null && currentlyLoopingSounds.IndexOf(playerToken) >= 0) {
      this.currentlyLoopingSounds.Remove(playerToken);
      this.players[playerToken].player.Stop();
      this.players[playerToken].inUse = false;
    }
    else {
      Debug.LogWarning("StopLoopingSound failed; player not found for that playerIndex");
    }

  }

  public void StopAllSounds() {
    for (int i = 0; i < this.players.Count; i++) {
      players[i].player.Stop();
    }
  }
  private int GetAvailablePlayer() {
    for (int i = 0; i < this.players.Count; i++) {
      if (!this.players[i].inUse) {
        this.players[i].inUse = true;
        return i;
      }
    }
    Debug.LogWarning("Ran out of audio sources to play sound.");
    return -1;
  }

  private IEnumerator FreeUpSourceAfterSoundEnds(SoundPlayer player, AudioClip sound, SoundType soundType) {
    yield return new WaitForSeconds(sound.length);
    this.currentlyPlayingSounds.Remove(soundType);

    player.inUse = false;
  }
}
