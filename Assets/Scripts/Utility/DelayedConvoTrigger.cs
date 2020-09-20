using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedConvoTrigger : MonoBehaviour {

    public string key;
    private AudioSource audio;
    private bool colliderActive = false;
    public int collisions = 0;
    public float counter = 0;
    public float maxCounter = 85;

    void Start() {
        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        if (colliderActive) {
            counter+=Time.deltaTime;
        }
        if (counter > maxCounter) {
            ConversationHandler.Instance.DisplayText(key, audio);
            Destroy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
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
