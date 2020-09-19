using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ConversationHandler : MonoBehaviour {

    public static ConversationHandler Instance { get; private set; }

    private string currentText = "";
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

    public void DisplayText(string key, AudioSource audio) {
        rightText.text = "";
        leftText.text = "";
        Conversation convo = GetEntry(key);
        if (convo == null) return;
        StartCoroutine(Print(convo, audio));
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

    IEnumerator Print(Conversation convo, AudioSource audio) {
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
                loriAnim.SetTrigger(c.Emote);
            } else if (c.Character == Character.Maki) {
                makiImage.enabled = true;
                loriImage.enabled = false;
                boxRight.enabled = false;
                boxLeft.enabled = true;
                rightLabel.enabled = true;
                leftLabel.enabled = false;
                makiAnim.SetTrigger(c.Emote);
                leftSide = true;
            }
            //if has sound, play sound
            if (c.Sound != null) {
                AudioClip clip = Resources.Load<AudioClip>(c.Sound);
                audio.PlayOneShot(clip);
            }
            // print text
            string currentText = "";
            for (int i = 0; i <= c.Text.Length; i++) {
                currentText = c.Text.Substring(0, i);
                //update display text
                if (leftSide) leftText.text = currentText;
                else rightText.text = currentText;
                //wait delay
                yield return new WaitForSeconds(0.1f);
            }
            // wait then play next
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        DisableAll();
    }

    [System.Serializable]
    public enum Emote {Neutral, Sad, Excited, Proud}

    [System.Serializable]
    public enum Character {Lori, Maki}

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
    }
}
