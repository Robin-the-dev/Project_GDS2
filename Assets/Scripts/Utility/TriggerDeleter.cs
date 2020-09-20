using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeleter : MonoBehaviour {

    public GameObject trigger;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
          Destroy(trigger);
        }
    }
}
