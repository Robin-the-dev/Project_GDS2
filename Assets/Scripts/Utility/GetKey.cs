using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetKey : MonoBehaviour
{
    private string initialText = null;
    private TextMeshPro textObject = null;
    private List<string> keys = new List<string>();

    void Start() {
        if (TryGetComponent(out TextMeshPro text)) {
            initialText = text.text;
            textObject = text;
            string updatedText = initialText;
            Regex compare = new Regex(".*\\<.*\\>.*");
            while(compare.IsMatch(updatedText)) {
                int start = updatedText.IndexOf("<") + 1;
                int end = updatedText.IndexOf(">");
                string key = updatedText.Substring(start, end - start);
                string keyText = ControlSettings.Instance.getString(key);
                keys.Add(key);
                updatedText = updatedText.Replace("<" + key + ">", keyText);
            }
            textObject.text = updatedText;
        }
    }

    void FixedUpdate(){
        if (initialText != null) {
            string updatedText = initialText;
            foreach(string s in keys) {
                string keyText = ControlSettings.Instance.getString(s);
                updatedText = updatedText.Replace("<" + s + ">", keyText);
            }
            textObject.text = updatedText;
        }
    }
}
