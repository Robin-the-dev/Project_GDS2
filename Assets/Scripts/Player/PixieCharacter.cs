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

    private MakiPathing pathing;

    private float nextActionTime = 0.0f;
    private float period = 0.25f;

    private Stack<Vector3Int> path = null;

    private void FixedUpdate() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            Stack<Vector3Int> newPath = pathing.Algorithm();
            if (newPath != null) {
                path = newPath;
            }
        }
        if (path == null) return;

        GetComponent<SpriteRenderer>().flipX = rigid.velocity.x < 0;

        Vector3 position = pathing.ToWorldSpace(path.Peek());

        float distance = Vector3.Distance(transform.position, pathing.endPos);
        if (Vector3.Distance(transform.position, position) < minDistance) path.Pop();
        float newSpeed = maxSpeed;
        if (distance < maxDistance) newSpeed = (distance/maxDistance) * maxSpeed;
        if (distance > minDistance){
            Vector2 direction = position - transform.position;
            Vector2 targetVelocity = Vector3.ClampMagnitude(newSpeed * direction, maxVelocity);
            Vector2 error = targetVelocity - rigid.velocity;
            Vector2 force = Vector2.ClampMagnitude(flightGain * error, maxForce);
            rigid.AddForce(force);
        }
    }

    public override void Start(){
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        pathing = GetComponent<MakiPathing>();
        nextActionTime = Time.time + period;
    }

    public void UpdatePosition(Vector3 mousePosition) {
        pathing.updateEndPos(mousePosition);
        // flip sprite to face mouse

        // // adjust speed so the further you are away the faster it moves.
        // float distance = Vector3.Distance(transform.position, mousePosition);
        // if (distance < minDistance) return;
        // float newSpeed = maxSpeed;
        // if (distance < maxDistance) newSpeed = (distance/maxDistance) * maxSpeed;
        // // move towards mouse
        // if (distance > minDistance){
        //     Vector2 direction = mousePosition - transform.position;
        //     Vector2 targetVelocity = Vector3.ClampMagnitude(newSpeed * direction, maxVelocity);
        //     Vector2 error = targetVelocity - rigid.velocity;
        //     Vector2 force = Vector2.ClampMagnitude(flightGain * error, maxForce);
        //     rigid.AddForce(force);
        // }
    }
}
