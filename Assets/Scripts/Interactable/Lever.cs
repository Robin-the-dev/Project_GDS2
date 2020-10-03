using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
    }

    public override void Interact() {
        active = !active;
        CheckState();
    }

    public override void CheckState() {
        if (active)
            switchOn();
        else
            switchOff();
    }

    private void switchOn() {
        obj.Activate(key);
        spriteRenderer.sprite = onSprite;
        AudioManager.Instance.PlayMetalLever();
    }

    private void switchOff() {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        AudioManager.Instance.PlayMetalLever();
    }
}
