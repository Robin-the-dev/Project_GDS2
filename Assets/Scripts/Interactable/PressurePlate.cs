using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D pressurePlateCollider;
    private Sprite offSprite;
    private Vector2 offSize;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offSprite = spriteRenderer.sprite;
        offSize = pressurePlateCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            spriteRenderer.sprite = onSprite;
            pressurePlateCollider.size = new Vector2(1.0f, 0.19f);
            door.isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            spriteRenderer.sprite = offSprite;
            pressurePlateCollider.size = offSize;
            door.isOpen = false;
        }
    }
}
