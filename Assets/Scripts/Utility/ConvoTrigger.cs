using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoTrigger : MonoBehaviour {

    public string key;
    public bool enabled = true;

    void OnTriggerEnter2D(Collider2D col) {
        if (!enabled) return;
        if (col.tag == "Player") {
            ConversationHandler.Instance.DisplayText(key);
            Destroy(this);
        }
    }
}
