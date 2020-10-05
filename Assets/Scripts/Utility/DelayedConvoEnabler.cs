using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedConvoEnabler : MonoBehaviour {

    public DelayedConvoTrigger convo;

    void OnTriggerEnter2D(Collider2D col) {
        if (convo == null) return;
        if (col.tag == "Player") {
            convo.enabled = true;
            Destroy(this);
        }
    }
}
