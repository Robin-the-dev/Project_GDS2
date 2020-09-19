using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoTrigger : MonoBehaviour {

    public string key;
    private AudioSource audio;

    void Start() {
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            ConversationHandler.Instance.DisplayText(key, audio);
            Destroy(this);
        }
    }
}
