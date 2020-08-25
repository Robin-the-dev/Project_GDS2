using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour {

    BoxCollider2D boxCollider;
    Rigidbody2D rigid;

    public void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Interact(bool active) {
        boxCollider.enabled = !active;
        rigid.isKinematic = active;
    }
}
