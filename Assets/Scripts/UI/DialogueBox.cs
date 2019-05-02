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
  private bool queuedIntro = false;
  private List<string> escapeMessages = new List<string> {
    "<j=crazy> YEEESS!!! FREEDOM IS OURS!!!",
    "<j=crazy> SEE YOU LATER, JERKS!!!",
    "<j=crazy> I TOLD YOU THAT WOULD WORK!!! LET'S GET OUTTA HERE!!!",

  };
  // Start is called before the first frame update
  void Start() {
    Signals.Get<ShowDialogueMessageSignal>().AddListener(QueueMessage);

    foreach (PestoSprite sprite in pestoSprites) {
      pestoSpriteDict[sprite.emote] = sprite.sprite;
    }
  }

  public void OnEnable() {
    if (queuedIntro) return;
    queuedIntro = true;
    StartCoroutine(QueueIntro());
  }

  IEnumerator QueueIntro() {
    yield return new WaitForSeconds(0.1f);
    string msg = escapeMessages[Random.Range(0, escapeMessages.Count - 1)];
    QueueMessage(msg, PestoEmote.ANGRY, true);
    textMesh.OnCompleteEvent += () => { StartCoroutine(StopReading()); };
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
