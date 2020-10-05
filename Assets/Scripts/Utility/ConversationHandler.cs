using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ConversationHandler : MonoBehaviour {

    public static ConversationHandler Instance { get; private set; }
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI leftLabel;
    public TextMeshProUGUI rightLabel;
    public Image boxLeft;
    public Image boxRight;
    public GameObject maki;
    public GameObject lori;
    private Animator makiAnim;
    private Animator loriAnim;
    private Image makiImage;
    private Image loriImage;

    List<Conversation> convoQueue = new List<Conversation>();

    private bool chatActive = false;

    public Entries entries;

    public void Start(){
        string path = Application.streamingAssetsPath + "/Json/Conversations.json";
        string jsonText = File.ReadAllText(path);
        entries = JsonUtility.FromJson<Entries>(jsonText);
        makiAnim = maki.GetComponent<Animator>();
        loriAnim = lori.GetComponent<Animator>();
        makiImage = maki.GetComponent<Image>();
        loriImage = lori.GetComponent<Image>();
        DisableAll();
    }


    private void DisableAll(){
        boxLeft.enabled = false;
        boxRight.enabled = false;
        rightText.text = "";
        leftText.text = "";
        rightLabel.enabled = false;
        leftLabel.enabled = false;
        makiImage.enabled = false;
        loriImage.enabled = false;
    }

    public void DisplayText(string key) {
        Conversation convo = GetEntry(key);
        if (convo == null) return;
        convoQueue.Add(convo);
    }

    void FixedUpdate() {
        if (!chatActive && convoQueue.Count > 0) {
            Conversation convo = convoQueue.First();
            convoQueue.Remove(convoQueue.First());
            chatActive = true;
            StartCoroutine(Print(convo));
        }
    }

    private Conversation GetEntry(string key) {
        foreach (Conversation c in entries.Conversations) {
            if (c.Key == key) {
              return c;
            }
        }
        return null;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    IEnumerator Print(Conversation convo) {
        // for each chatline do
        foreach (ChatLine c in convo.Lines) {
            rightText.text = "";
            leftText.text = "";
            bool leftSide = false;
            // show the correct character
            if (c.Character == Character.Lori) {
                makiImage.enabled = false;
                loriImage.enabled = true;
                boxRight.enabled = true;
                boxLeft.enabled = false;
                rightLabel.enabled = false;
                leftLabel.enabled = true;
                loriAnim.Play(c.Emote);
            } else if (c.Character == Character.Maki) {
                makiImage.enabled = true;
                loriImage.enabled = false;
                boxRight.enabled = false;
                boxLeft.enabled = true;
                rightLabel.enabled = true;
                leftLabel.enabled = false;
                makiAnim.Play(c.Emote);
                leftSide = true;
            } else {
                makiImage.enabled = true;
                loriImage.enabled = false;
                boxRight.enabled = false;
                boxLeft.enabled = true;
                rightLabel.enabled = false;
                leftLabel.enabled = false;
                makiAnim.Play(c.Emote);
                leftSide = true;
            }
            //if has sound, play sound
            if (c.Sound != null) {
                AudioManager.Instance.PlayClip(c.Sound);
            }
            // print text
            string currentText = "";
            for (int i = 0; i <= c.Text.Length; i++) {
                currentText = c.Text.Substring(0, i);
                //update display text
                if (leftSide) leftText.text = currentText;
                else rightText.text = currentText;
                //wait delay
                string currentChar;
                if (currentText.Length < 2) {
                    currentChar = currentText;
                } else {
                    currentChar = currentText.Substring(currentText.Length-1, 1);
                }
                if (currentChar == "." || currentChar == "!" || currentChar == "?") {
                    Talking(c, false);
                    yield return new WaitForSeconds((0.2f / c.textSpeed) * PlayerPrefs.GetFloat("TextSpeed", 1));
                } else if (currentChar == ",") {
                    Talking(c, false);
                    yield return new WaitForSeconds((0.15f / c.textSpeed) * PlayerPrefs.GetFloat("TextSpeed", 1));
                }else {
                    Talking(c, true);
                    yield return new WaitForSeconds((0.075f / c.textSpeed) * PlayerPrefs.GetFloat("TextSpeed", 1));
                }
            }
            // wait then play next
            Talking(c, false);
            yield return new WaitForSeconds(c.endDelay);
        }
        yield return new WaitForSeconds(2f);
        DisableAll();
        chatActive = false;
    }

    private void Talking(ChatLine c, bool isTalking) {
        if (c.Character == Character.Lori) {
            loriAnim.SetBool("End", !isTalking);
        } else if (c.Character == Character.Maki) {
            makiAnim.SetBool("End", !isTalking);
        }
    }

    [System.Serializable]
    public enum Emote {Neutral, Sad, Excited, Proud}

    [System.Serializable]
    public enum Character {Lori, Maki, Help}

    [System.Serializable]
    public class Entries {
        public List<Conversation> Conversations;
    }

    [System.Serializable]
    public class Conversation {
        public string Key;
        public List<ChatLine> Lines;
        public bool IsBlocking;
    }

    [System.Serializable]
    public class ChatLine {
        public Character Character;
        public string Text;
        public string Emote;
        public string Sound;
        public float textSpeed = 1;
        public float endDelay = 0.75f;
    }
}
