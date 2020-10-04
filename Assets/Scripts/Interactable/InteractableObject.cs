using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractableObject : MonoBehaviour {

    public LinkedObject obj;
    public string key;
    public GameObject popup;
    public bool interactable = false;
    public bool active = false;
    public bool pixieCanUse = false;
    [HideInInspector]
    public int interacting = 0;

    public void InitLink(string key, LinkedObject obj) {
        this.obj = obj;
        this.key = key;
        CheckState(false);
    }

    public abstract void Interact();

    public abstract void CheckState(bool playSound);

    // Start is called before the first frame update
    public virtual void Start() {
        CheckState(false);
        if (popup != null) popup.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update() {
        if (popup != null) popup.SetActive(interactable);
    }

    public virtual void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player")) {
            interacting--;
            if (interacting == 0) interactable = false;
        }
    }

    public virtual void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            interacting++;
            if (interacting > 0) interactable = true;
        }
    }
}
