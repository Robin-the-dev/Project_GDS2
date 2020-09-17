using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ConversationHandler : MonoBehaviour {

    public static ConversationHandler Instance { get; private set; }

    private string currentText = "";
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public GameObject chatBox;

    public Entries entries;

    public void Start(){
        string path = Application.dataPath + "/Json/Conversations.json";
        string jsonText = File.ReadAllText(path);
        entries = JsonUtility.FromJson<Entries>(jsonText);
    }

    public void DisplayText(string key) {
        rightText.text = "";
        leftText.text = "";
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator Print(string originText, bool leftSide) {
        string currentText = "";
        for (int i = 0; i < originText.Length; i++) {
            currentText = originText.Substring(0, i);
            //update display text
            if (leftSide) leftText.text = currentText;
            else rightText.text = currentText;
            //wait delay
            yield return new WaitForSeconds(PlayerPrefs.GetInt("TextDelay"));
        }
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
        public Emote Emote;
    }
}
