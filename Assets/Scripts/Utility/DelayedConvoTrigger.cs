using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedConvoTrigger : MonoBehaviour {

    public string key;
    private bool colliderActive = false;
    private int collisions = 0;
    private float counter = 0;
    public float maxCounter = 85;
    public bool enabled = true;

    void FixedUpdate() {
        if (colliderActive) {
            counter+=Time.deltaTime;
        }
        if (counter > maxCounter) {
            ConversationHandler.Instance.DisplayText(key);
            Destroy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!enabled) return;
        if (col.tag == "Player") {
            collisions++;
            if (collisions > 0) {
                colliderActive = true;
            }
        }

    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Player") {
            collisions--;
            if (collisions <= 0) {
                colliderActive = false;
            }
        }

    }
}
