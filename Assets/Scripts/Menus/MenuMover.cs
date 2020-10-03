using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMover : MonoBehaviour {

    void Start() {
        Time.timeScale = 1;
    }

    void FixedUpdate() {
        transform.position = new Vector3(transform.position.x + Time.fixedDeltaTime, transform.position.y, transform.position.z);
    }
}
