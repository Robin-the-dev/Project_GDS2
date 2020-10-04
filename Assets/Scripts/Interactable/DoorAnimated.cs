using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : LinkedObject {
    public bool isOpen;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Awake() {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        if(isOpen) openDoor();
    }

    public override void Activate(string msg) {
        openDoor();
    }

    public override void Deactivate(string msg) {
        closeDoor();
    }

    private void openDoor() {
        if (isOpen) return;
        isOpen = true;
        anim.Play("Open");
    }

    private void closeDoor() {
        if (!isOpen) return;
        anim.Play("Close");
        isOpen = false;
    }
}
