using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLever : InteractableObject {
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite offSprite;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
    }

    public override void Interact() {
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
        if (playSound) AudioManager.Instance.PlayMetalLever();
    }

    private void switchOff(bool playSound) {
        obj.Deactivate(key);
        spriteRenderer.sprite = offSprite;
        if (playSound) AudioManager.Instance.PlayMetalLever();
    }
}
