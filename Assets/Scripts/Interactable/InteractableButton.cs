using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : InteractableObject {
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;
    private float timeLeft;
    public float closingTimeOffset = 3.0f; // Change this if you want to change closing time offset

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        timeLeft = 0;
    }

    private void FixedUpdate() {
        if(active) {
            timeLeft -= Time.deltaTime;
        }
        if (timeLeft <= 0.0f && active) {
            active = false;
            switchOff(true);
        }
    }

    public override void Interact() {
        if (timeLeft > 0) {
            timeLeft = closingTimeOffset;
            return;
        }
        active = !active;
        CheckState(true);
    }

    public override void CheckState(bool playSound) {
        if (active)
            switchOn(playSound);
        else
            switchOff(playSound);
    }

    private void switchOn(bool playSound) {
        obj.Activate(key);
        spriteRenderer.sprite = onSprite;
        timeLeft = closingTimeOffset;
        if (playSound) AudioManager.Instance.PlaySwitchOnAndOff();
    }

    private void switchOff(bool playSound) {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        if (playSound) AudioManager.Instance.PlaySwitchOnAndOff();
    }
}
