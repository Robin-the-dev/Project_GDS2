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

    public float flightSpeed = 10f;

    private MakiPathing pathing;

    private float nextActionTime = 0.0f;
    private float period = 0.25f;

    private Stack<Vector3Int> path = null;
    private bool pixieMode = false;
    public PlayerCharacter player;
    public float maxDistanceFromPlayer = 30f;

    private InteractableObject currentInteraction = null;

    private bool moveUp = false;
    private bool moveDown = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    private int moveX = 0;
    private int moveY = 0;
    private float distance;

    private bool inWater = false;

    private AudioSource source;

    public override void FixedUpdate() {
        base.FixedUpdate();
        GetComponent<SpriteRenderer>().flipX = rigid.velocity.x < 0;
        source = GetComponent<AudioSource>();
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > maxDistanceFromPlayer + 5f) WarpHome(FindObjectOfType<PlayerCharacter>());
        if (pixieMode) {
            ManualMovement();
        } else {
            PathMovement();
        }
    }

    public void UpdatePixieMode(bool pixieMode) {
        this.pixieMode = pixieMode;
        if (pixieMode) {
            rigid.velocity = Vector3.zero;
        }
    }

    private void Update() {
        if (source == null) return;
        float volume = PlayerPrefs.GetFloat("SoundEffectVolume");
        if (source.volume != volume) {
            source.volume = volume;
        }
        if (AudioManager.Instance.isPaused) {
            source.Pause();
        } else {
            source.UnPause();
        }
    }

    public void WarpHome(PlayerCharacter player) {
        transform.position = player.transform.position;
    }

    public override void Start(){
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        pathing = GetComponent<MakiPathing>();
        nextActionTime = Time.time + period;
    }

    public void Interact() {
        if (currentInteraction != null) {
            if (currentInteraction.pixieCanUse) {
                currentInteraction.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = interact;
        }
        if (col.tag == "Water") inWater = true;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = interact;
        }
        if (col.tag == "Water") inWater = true;
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = null;
        }
        if (col.tag == "Water") inWater = false;
    }

    public void MoveRight(bool active) {
        moveRight = active;
    }

    public void MoveLeft(bool active) {
        moveLeft = active;
    }

    public void MoveUp(bool active) {
        moveUp = active;
    }

    public void MoveDown(bool active) {
        moveDown = active;
    }

    private void ManualMovement() {

        Vector3 directionHome = player.transform.position - transform.position;
        if (distance < maxDistanceFromPlayer) {
            if (moveRight){
                moveX += 1;
            } else if (moveLeft) {
                moveX -= 1;
            }
            if (moveUp){
                moveY += 1;
            } else if (moveDown) {
                moveY -= 1;
            }
        } else {
            rigid.AddForce(directionHome / directionHome.magnitude * flightSpeed * 4);
        }
        Vector2 currentVelocity = rigid.velocity;
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        if (moveX != 0 || moveY != 0) {
            rigid.velocity = movement * flightSpeed;
        } else {
            //comes to a slow stop rather than an isntant one.
            rigid.velocity = new Vector2(rigid.velocity.x * 0.85f, rigid.velocity.y * 0.85f);
        }
        moveX = 0;
        moveY = 0;
    }

    private void PathMovement() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            Stack<Vector3Int> newPath = pathing.Algorithm();
            if (newPath != null) {
                path = newPath;
            }
        }
        if (path == null) return;



        if (path.Count == 0) return;
        Vector3 position = pathing.ToWorldSpace(path.Peek());

        float distance = Vector3.Distance(transform.position, pathing.endPos);
        if (Vector3.Distance(transform.position, position) < minDistance) path.Pop();
        float newSpeed = maxSpeed;
        if (distance < maxDistance) newSpeed = (distance/maxDistance) * maxSpeed;
        if (inWater) newSpeed = newSpeed / 3;
        if (distance > minDistance) {
            Vector2 direction = position - transform.position;
            Vector2 targetVelocity = Vector3.ClampMagnitude(newSpeed * direction, maxVelocity);
            Vector2 error = targetVelocity - rigid.velocity;
            Vector2 force = Vector2.ClampMagnitude(flightGain * error, maxForce);
            rigid.AddForce(force);
        }
    }

    public void UpdatePosition(Vector3 mousePosition) {
        pathing.updateEndPos(mousePosition);
    }
}
