using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour {

    private bool lit = false;

    private SpriteRenderer sprite;
    public GameObject pixie;
    private ContactFilter2D filter;
    private List<RaycastHit2D> results = new List<RaycastHit2D>();

    void Start() {
        filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(~LayerMask.GetMask("Player", "AntiWallGrab", "Water", "PixieLight", "Interactable", "Hidden"));
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "PixieLight") {
            pixie = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "PixieLight") {
            pixie = null;
        }
    }

    private bool rayTest() {
        if (pixie == null) return false;
        results = new List<RaycastHit2D>();
        float dist = Vector3.Distance(pixie.transform.position, transform.position);
        Vector3 direction = pixie.transform.position - transform.position;
        Physics2D.Raycast(transform.position, direction, filter, results, dist);
        if (results[0] != null && results[0].collider.tag == "Pixie") {
            Debug.DrawRay(transform.position, direction, Color.green);
            return true;
        }
        Debug.DrawRay(transform.position, direction, Color.red);
        return false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        sprite.enabled = rayTest();
    }
}
