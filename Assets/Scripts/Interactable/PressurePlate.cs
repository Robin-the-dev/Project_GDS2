using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : InteractableObject {
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D pressurePlateCollider;
    private Sprite offSprite;
    private Vector2 offSize;

    // Start is called before the first frame update
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        offSize = pressurePlateCollider.size;
    }

    public override void Interact() {

    }

    public override void CheckState() {
        if (active)
            switchOn();
        else
            switchOff();

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            interactPressurePlate(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            interactPressurePlate(false);
        }
    }

    private void switchOn() {
        obj.Activate(key);
        spriteRenderer.sprite = onSprite;
        pressurePlateCollider.size = new Vector2(1.0f, 0.18f);
        AudioManager.Instance.PlayMetalLatch();
    }

    private void switchOff() {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        pressurePlateCollider.size = offSize;
        AudioManager.Instance.PlayLatchButton();
    }

    private void interactPressurePlate(bool active) {
        this.active = active;
        CheckState();
    }
}
