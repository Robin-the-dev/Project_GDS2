using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractableObject : MonoBehaviour {

    public LinkedObject obj;
    public GameObject popup;
    public bool interactable = false;
    public string key;
    public bool active = false;
    [HideInInspector]
    public int interacting = 0;

    public abstract void Interact();

    public abstract void CheckState();

    // Start is called before the first frame update
    public virtual void Start() {
        CheckState();
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
