using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;
    private float timeLeft;
    private const float closingTimeOffset = 3.0f; // Change this if you want to change closing time offset

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        timeLeft = closingTimeOffset;
    }

    private void FixedUpdate() {
        if(active) {
            timeLeft -= Time.deltaTime;
        }

        if(timeLeft <= 0.0f) {
            active = !active;
            switchOff();
        }
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
        AudioManager.Instance.PlaySwitchOnAndOff();
    }

    private void switchOff() {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        timeLeft = closingTimeOffset;
        AudioManager.Instance.PlaySwitchOnAndOff();
    }
}
