using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using deVoid.Utils;


public enum PestoEmote {
  ANGRY,
  NEUTRAL,
  SCARED
}
[System.Serializable]
public class PestoSprite {
  public Sprite sprite;
  public PestoEmote emote;
}


public class PestoMessage {
  public string msg;
  public PestoEmote emote;

  public PestoMessage(string msg, PestoEmote emote) {
    this.msg = msg;
    this.emote = emote;
  }
}

public class DialogueBox : MonoBehaviour {

  public SuperTextMesh textMesh;
  public Image pestoImage;
  private Dictionary<PestoEmote, Sprite> pestoSpriteDict = new Dictionary<PestoEmote, Sprite>();
  public PestoSprite[] pestoSprites;
  private List<PestoMessage> messages = new List<PestoMessage>();
  private bool currentlyReading = false;
  private float startDelay = 0;
  // Start is called before the first frame update
  void Start() {
    Signals.Get<ShowDialogueMessageSignal>().AddListener(QueueMessage);

    foreach (PestoSprite sprite in pestoSprites) {
      pestoSpriteDict[sprite.emote] = sprite.sprite;
    }


    StartCoroutine(QueueTest());

  }

  IEnumerator QueueTest() {
    yield return new WaitForSeconds(0.1f);
    textMesh.OnCompleteEvent += () => { StartCoroutine(StopReading()); };
    QueueMessage("Uh oh Bean, looks like we've got some company!", PestoEmote.SCARED);
    QueueMessage("<j=crazy>LET'S KILL EM!!!</j>", PestoEmote.ANGRY);
  }

  IEnumerator StopReading() {
    yield return new WaitForSeconds(1f);
    currentlyReading = false;
  }

  void Update() {
    startDelay += Time.deltaTime;

    if (!currentlyReading) {
      if (messages.Count > 0) {
        var message = messages[0];
        textMesh.text = message.msg;
        pestoImage.sprite = pestoSpriteDict[message.emote];
        messages.RemoveAt(0);
        currentlyReading = true;
      }
    }

    SetVisible(currentlyReading);
  }

  void SetVisible(bool visible) {
    transform.localScale = Vector3.one * (visible ? 1 : 0);
  }
  void QueueMessage(string msg, PestoEmote emote, bool allowQueueing = true) {
    if (allowQueueing || messages.Count == 0) {
      messages.Add(new PestoMessage(msg, emote));
    }
  }
}
