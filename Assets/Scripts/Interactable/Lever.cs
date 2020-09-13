using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    [SerializeField] private Door door;
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;
    private bool isOn;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        isOn = false;
        
    }

    public override void Interact() {
        if(isOn) {
            isOn = false;
            door.isOpen = false;
            spriteRenderer.sprite = offSprite;
        }
        else {
            isOn = true;
            door.isOpen = true;
            spriteRenderer.sprite = onSprite;
        }
    }

    public override void CheckState() {
    }
}
