using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    [SerializeField] private Door door;
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;
    private bool isPressed;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        isPressed = false;

    }

    public override void Interact() {
        if (isPressed) {
            isPressed = false;
            door.isOpen = false;
            spriteRenderer.sprite = offSprite;
        }
        else {
            isPressed = true;
            door.isOpen = true;
            spriteRenderer.sprite = onSprite;
        }
    }

    public override void CheckState() {
    }
}
