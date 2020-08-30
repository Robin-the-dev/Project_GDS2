using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;

    private Vector3 respawnPosition;
    private Vector3 pitCollisionPosition;

    // Start is called before the first frame update
    void Start() {
        respawnPosition = playerCharacter.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (playerCharacter.IsGrounded()) {
            respawnPosition = playerCharacter.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            pitCollisionPosition = playerCharacter.transform.position;

            if (pitCollisionPosition.x > respawnPosition.x) {
                respawnPosition = new Vector3(respawnPosition.x - 1f, respawnPosition.y);
                collision.transform.position = respawnPosition;
                GameManager.instance.damage();
            } else {
                respawnPosition = new Vector3(respawnPosition.x + 1f, respawnPosition.y);
                collision.transform.position = respawnPosition;
                GameManager.instance.damage();
            }
        }
    }
}
