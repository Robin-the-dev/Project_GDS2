using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour {

    public string key;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            MapController.Instance.EnableLayer(key);
        }
    }
}
