using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour {

    public string key;
    public bool mapEnabled = false;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            if (!mapEnabled) {
                MapController.Instance.EnableLayer(key);
            }
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.tag == "Player") {
            MapController.Instance.UpdateMapPosition(key);
        }
    }
}
