using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFinder : MonoBehaviour {

    public PlayerCharacter character;
    public List<InteractableBox> boxes = new List<InteractableBox>();

    private void Start() {
        character = GetComponentInParent<PlayerCharacter>();
    }

    // determines the closest box within the collision area
    private void FixedUpdate() {
        float closestDist = -1;
        InteractableBox closestBox = null;
        foreach (InteractableBox box in boxes) {
            float tempDist = Vector2.Distance(box.transform.position, transform.position);
            if (closestBox == null) {
                closestBox = box;
                closestDist = tempDist;
            } else if (tempDist < closestDist) {
                closestBox = box;
                closestDist = tempDist;
            }
        }
        character.UpdateBox(closestBox);
    }

    // adds box to potential targets
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableBox box)){
            boxes.Add(box);
        }
    }

    // if found after enter adds box to potential targets
    private void OnTriggerStay2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableBox box)){
            if (!boxes.Contains(box)) boxes.Add(box);
        }
    }

    // on trigger exit remove box
    private void OnTriggerExit2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableBox box)){
            boxes.Remove(box);
        }
    }
}
