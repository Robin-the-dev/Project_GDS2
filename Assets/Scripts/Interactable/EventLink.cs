using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventLink : LinkedObject {

    private Transform startPosition;

    [Serializable]
    public struct Key {
        public string key;
        public InteractableObject interactable;
        public bool value;
        public bool inverted;

        public void UpdateKey(string key) {
            this.key = key;
        }
    }
    public Key[] keys;
    public LinkedObject[] objs;
    public String key;
    private bool active = false;

    public bool orMode = false;

    private int getKeyID(string key) {
        for (int i = 0; i < keys.Length; i++) {
            if (string.Equals(key, keys[i].key)){
                return i;
            }
        }
        return -1;
    }

    public override void Activate(string msg) {
        int keyID = getKeyID(msg);
        if (keyID == -1) return;
        if (keys[keyID].inverted) {
            keys[keyID].value = false;
        } else {
            keys[keyID].value = true;
        }

        CheckActive();
    }

    private bool isActive() {
        bool result = !orMode;
        foreach (Key entry in keys) {
            if (orMode) {
                if (entry.value == true) return true;
            } else {
                if (entry.value == false) result = false;
            }
        }
        return result;
    }

    public override void Deactivate(string msg) {
        int keyID = getKeyID(msg);
        if (keyID == -1) return;
        if (keys[keyID].inverted) {
            keys[keyID].value = true;
        } else {
            keys[keyID].value = false;
        }

        CheckActive();
    }

    void CheckActive() {
        active = isActive();
        if (isActive()) {
            foreach (LinkedObject l in objs) {
                if (l == null) continue;
                l.Activate(key);
            }
        } else {
            foreach (LinkedObject l in objs) {
                if (l == null) continue;
                l.Deactivate(key);
            }
        }
    }
    // Start is called before the first frame update
    void Start() {
        startPosition = transform;
        for (int i = 0; i < keys.Length;i++) {
            if (keys[i].interactable == null) continue;
            if (keys[i].key == null || keys[i].key == "") keys[i].UpdateKey("Link_" + i);
            keys[i].interactable.InitLink(keys[i].key, this);
        }
    }

    // Update is called once per frame
    public void OnDrawGizmos() {
        foreach (Key k in keys) {
            if (k.interactable == null) continue; // skip null
            if (k.value) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, k.interactable.transform.position);

        }
        if (active) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        foreach (LinkedObject l in objs) {
            if (l == null) continue; // skip null
            Gizmos.DrawLine(transform.position, l.transform.position);
        }
        Gizmos.DrawIcon(transform.position, "EventLink.png", true);

    }
}
