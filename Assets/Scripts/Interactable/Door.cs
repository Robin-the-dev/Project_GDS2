using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite openDoorSprite;

    private SpriteRenderer spriteRenderer;
    private Sprite closedDoorSprite;
    private BoxCollider2D doorCollider;
    [HideInInspector] public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();
        isOpen = false;
        closedDoorSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen) {
            spriteRenderer.sprite = openDoorSprite;
            doorCollider.enabled = false;
        }
        else {
            spriteRenderer.sprite = closedDoorSprite;
            doorCollider.enabled = true;
        }
    }
}
