using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixieCharacter : AnimatedCharacter {
    // Start is called before the first frame update

    private Rigidbody2D rigid;
    public float distanceToStop = 1f;
    public float maxDistance = 10f;
    public float minDistance = 1f;
    public float maxSpeed = 20f;
    public float maxVelocity = 10f;
    public float maxForce = 10f;
    public float flightGain = 20f;


    public override void Start(){
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void UpdatePosition(Vector3 mousePosition) {
        // flip sprite to face mouse
        GetComponent<SpriteRenderer>().flipX = mousePosition.x < transform.position.x;
        // adjust speed so the further you are away the faster it moves.
        float distance = Vector3.Distance(transform.position, mousePosition);
        if (distance < minDistance) return;
        float newSpeed = maxSpeed;
        if (distance < maxDistance) newSpeed = (distance/maxDistance) * maxSpeed;
        // move towards mouse
        if (distance > minDistance){
            Vector2 direction = mousePosition - transform.position;
            Vector2 targetVelocity = Vector3.ClampMagnitude(newSpeed * direction, maxVelocity);
            Vector2 error = targetVelocity - rigid.velocity;
            Vector2 force = Vector2.ClampMagnitude(flightGain * error, maxForce);
            rigid.AddForce(force);
        }
    }
}
