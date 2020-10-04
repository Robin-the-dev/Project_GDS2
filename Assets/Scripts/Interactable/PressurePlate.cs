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

    public override void CheckState(bool playSound) {
        if (active)
            switchOn(playSound);
        else
            switchOff(playSound);

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

    private void switchOn(bool playSound) {
        obj.Activate(key);
        spriteRenderer.sprite = onSprite;
        pressurePlateCollider.size = new Vector2(1.0f, 0.18f);
        if (playSound) AudioManager.Instance.PlayMetalLatch();
    }

    private void switchOff(bool playSound) {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        pressurePlateCollider.size = offSize;
        if (playSound) AudioManager.Instance.PlayLatchButton();
    }

    private void interactPressurePlate(bool active) {
        this.active = active;
        CheckState(true);
    }
}
