using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour {

    BoxCollider2D[] boxColliders;
    Rigidbody2D rigid;
    SpriteRenderer sprite;

    [HideInInspector] public Vector3 startPosition;

    public void Start() {
        boxColliders = GetComponents<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        startPosition = transform.position;
    }

    public void Interact(bool active) {
        foreach (BoxCollider2D boxCollider in boxColliders) {
            boxCollider.enabled = !active;
        }
        rigid.isKinematic = active;
        sprite.enabled = !active;
    }
}
