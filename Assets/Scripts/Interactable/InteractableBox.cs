using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour {

    BoxCollider2D boxCollider;
    Rigidbody2D rigid;
    SpriteRenderer sprite;

    public void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Interact(bool active) {
        boxCollider.enabled = !active;
        rigid.isKinematic = active;
        sprite.enabled = !active;
    }
}
