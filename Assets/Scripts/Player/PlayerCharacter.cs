using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    //private variables
    [HideInInspector]
    public bool recording = false;
    [HideInInspector]
    public float currentTime = 0f;
    [HideInInspector]
    public float moveX = 0;
    [HideInInspector]
    public float runMultiplier = 1f;
    [HideInInspector]
    public bool holdJump = false;
    [HideInInspector]
    public bool moveLeft = false;
    [HideInInspector]
    public bool moveRight = false;
    [HideInInspector]
    public bool climbUp = false;
    [HideInInspector]
    public bool climbDown = false;

    public float jumpHeight = 14f;
    public float walkSpeed = 7;

    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public Animator anim;

    // [HideInInspector]
    // public InteractableObject currentInteraction = null;
    // [HideInInspector]
    // public BackgroundTransition currentTransition = null;
    // [HideInInspector]
    // public LockController currentLock = null;
    [HideInInspector]
    public bool onLadder = false;

    public virtual void Start() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// runs the character's primary Attack
    /// </summary>
    /// <param name="active"> if true keyPressed, if false keyReleased </param>
    /// <param name="mousePosition"> current mousePosition </param>
    public void doAttackAction(bool active, Vector3 mousePosition){

    }

    /// <summary>
    /// runs the character's special Attack
    /// </summary>
    /// <param name="active"> if true keyPressed, if false keyReleased </param>
    /// <param name="mousePosition"> current mousePosition </param>
    public void doSpecialAction(bool active, Vector3 mousePosition){

    }

    /// <summary>
    /// for use to trigger events on attack animations
    /// </summary>
    public void Attack(){

    }

    /// <summary>
    /// for use to trigger events on special animations
    /// </summary>
    public void Special(){

    }

    public virtual void Jump() {
        if (IsGrounded()) {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
            anim.SetTrigger("Jump");
            // AudioManager.Instance.RequestPlay(AudioManager.Instance.shared[0]);
        }
    }

    public void HoldJump(bool active) {
        holdJump = active;
    }

    public void MoveRight(bool active) {
        if (active) GetComponent<SpriteRenderer>().flipX = !active;
        moveRight = active;
    }

    public void MoveLeft(bool active) {
        if (active) GetComponent<SpriteRenderer>().flipX = active;
        moveLeft = active;
    }

    public void ClimbUp(bool active) {
        climbUp = active;
    }

    public void ClimbDown(bool active) {
        climbDown = active;
    }

    public void Sprint(bool active) {
        if (active) {
            runMultiplier = 1.5f;
        } else {
            runMultiplier = 1f;
        }
    }

    public virtual void HandleJumpHeld() {
        // default does nothing
    }

    // when override make sure to implement base.HandleClimb();
    public virtual void HandleClimb() {
        if (onLadder) {
            if (climbUp) {
                rigid.velocity = new Vector2(rigid.velocity.x, 7);
            }
        }
    }

    public virtual void HandleMovement() {
        // setAnimation Triggers
        anim.SetBool("OnGround", IsGrounded());
        anim.SetBool("Moving", moveRight || moveLeft);

        if (moveRight){
            moveX += 1;
        } else if (moveLeft) {
            moveX -= 1;
        }
        Vector2 currentVelocity = rigid.velocity;

        Vector2 movement = new Vector2(moveX, 0);

        if (moveX != 0) {
            rigid.velocity = movement * walkSpeed * runMultiplier;
        } else {
            //comes to a slow stop rather than an isntant one.
            rigid.velocity = new Vector2(rigid.velocity.x * 0.95f, currentVelocity.y);
        }
        rigid.velocity = new Vector2(rigid.velocity.x, currentVelocity.y);
        moveX = 0;
    }

    // public virtual void PreventWallHanging() {
    //     // Check if the body's current velocity will result in a collision
    //     Vector3 direction = Vector3.up;
    //     if (moveLeft) {
    //         direction = Vector3.left * walkSpeed * runMultiplier;
    //     } else if (moveRight) {
    //         direction = Vector3.right * walkSpeed * runMultiplier;
    //     }
    //     if (rigid.SweepTest(direction, out RaycastHit hit, direction.magnitude * Time.deltaTime, QueryTriggerInteraction.Ignore)) {
    //         // If so, stop the movement
    //         if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default")) {
    //             if (hit.point.y < transform.position.y - transform.localScale.y * 0.4) return;
    //             rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
    //         }
    //     }
    // }

    public virtual void HandleFalling() {
        if (rigid.velocity.y < 0) {
            rigid.velocity += Vector2.ClampMagnitude(Vector3.up * 2.5f * Physics.gravity.y * Time.deltaTime, jumpHeight*5);
        }
    }

    // public virtual void Update() {
    //     if (Time.timeScale > 0 ) {
    //         HandleCoolDowns();
    //     }
    // }

    // public virtual void HandleCoolDowns() {
    //     if (primaryTimeLeft > 0) {
    //         primaryTimeLeft -= Time.deltaTime;
    //     }
    //     if (secondaryTimeLeft > 0) {
    //         secondaryTimeLeft -= Time.deltaTime;
    //     }
    // }

    public virtual void FixedUpdate() {
        HandleMovement();
        HandleJumpHeld();
        // PreventWallHanging();
        HandleFalling();
        HandleClimb();
        // CheckLocks();
    }

    public void Interact() {
        // if (currentInteraction != null) {
        //     currentInteraction.Interact();
        // }
    }
    //
    // public void EnterDoor() {
    //     if (currentTransition != null) {
    //         rigid.transform.position = currentTransition.link.spawnPoint.transform.position;
    //     }
    // }

    // private void OnTriggerEnter(Collider col) {
    //     if (col.TryGetComponent(out BackgroundTransition transition)) {
    //         if (transition.active && transition.link.active) {
    //             currentTransition = transition;
    //         }
    //     }
    //     if (col.TryGetComponent(out LockController padlock)) {
    //         if (!padlock.active) {
    //             currentLock = padlock;
    //         }
    //     } else if (col.TryGetComponent(out InteractableObject interact)) {
    //         currentInteraction = interact;
    //     }
    //     if (col.CompareTag("Ladder")) {
    //         onLadder = true;
    //     }
    // }
    //
    // private void OnTriggerStay(Collider col) {
    //     if (col.TryGetComponent(out BackgroundTransition transition)) {
    //         if (transition.active && transition.link.active) {
    //             currentTransition = transition;
    //         }
    //     }
    //     if (col.TryGetComponent(out LockController padlock)) {
    //         if (!padlock.active) {
    //             currentLock = padlock;
    //         }
    //     } else if (col.TryGetComponent(out InteractableObject interact)) {
    //         currentInteraction = interact;
    //     }
    //     if (col.CompareTag("Ladder")) {
    //         onLadder = true;
    //     }
    // }
    //
    // private void OnTriggerExit(Collider col) {
    //     if (col.TryGetComponent(out BackgroundTransition transition)) {
    //         currentTransition = null;
    //     }
    //     if (col.TryGetComponent(out LockController padlock)) {
    //         currentLock = null;
    //     } else if (col.TryGetComponent(out InteractableObject interact)) {
    //         currentInteraction = null;
    //     }
    //     if (col.CompareTag("Ladder")) {
    //         onLadder = false;
    //     }
    // }

    // private void CheckLocks() {
    //     if (currentLock != null && PlayerPrefs.GetInt("keyCount") > 0) {
    //         currentLock.Interact();
    //         currentLock = null;
    //         PlayerPrefs.SetInt("keyCount", PlayerPrefs.GetInt("keyCount")-1);
    //     }
    // }

    public bool IsGrounded() {
        float width = (transform.localScale.x * 0.15f) * 1.5f;
        float height = (transform.localScale.y * 0.4f) * 1.5f;

        Vector2 corner1Start = new Vector2(rigid.position.x + width, rigid.position.y);
        Vector2 corner1End = Vector3.down * height;
        Vector2 corner2Start = new Vector2(rigid.position.x - width, rigid.position.y);
        Vector2 corner2End = Vector3.down * height;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(~LayerMask.GetMask("Player"));
        List<RaycastHit2D> results1 = new List<RaycastHit2D>();
        List<RaycastHit2D> results2 = new List<RaycastHit2D>();

        int corner1 = Physics2D.Raycast(corner1Start, Vector2.down, filter, results1, height);
        int corner2 = Physics2D.Raycast(corner2Start, Vector2.down, filter, results2, height);

        // debug raycast drawing
        if (corner1 > 0) Debug.DrawRay(corner1Start, corner1End, Color.green);
        else Debug.DrawRay(corner1Start, corner1End, Color.red);
        if (corner2 > 0) Debug.DrawRay(corner2Start, corner2End, Color.green);
        else Debug.DrawRay(corner2Start, corner2End, Color.red);

        return (corner1 > 0 || corner2 > 0);
    }
}
