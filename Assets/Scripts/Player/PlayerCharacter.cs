using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : AnimatedCharacter {

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

    // [HideInInspector]
    public InteractableBox foundBox = null;
    // [HideInInspector]
    public InteractableBox heldBox = null;
    [HideInInspector]
    public InteractableObject currentInteraction = null;

    public GameObject boxPosition;
    public BoxFinder boxFinder;

    // [HideInInspector]
    // public BackgroundTransition currentTransition = null;
    // [HideInInspector]
    // public LockController currentLock = null;
    [HideInInspector]
    public bool onLadder = false;

    public override void Start() {
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
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
        if (foundBox != null && heldBox == null && active) {
            UpdateBoxState(true);
        } else if (heldBox != null && !active) {
            UpdateBoxState(false);
        }
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
        if (active) UpdateFacingDirection(!active);
        moveRight = active;
    }

    public void MoveLeft(bool active) {
        if (active) UpdateFacingDirection(active);
        moveLeft = active;
    }

    public void ClimbUp(bool active) {
        climbUp = active;
    }

    public void ClimbDown(bool active) {
        climbDown = active;
    }

    public void Sprint(bool active) {
        if (heldBox != null) return;
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
        anim.SetBool("Moving", Mathf.Abs(rigid.velocity.x) > 0.5);

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

    private void UpdateBoxState(bool active) {
        if (active){
            heldBox = foundBox;
            heldBox.Interact(true);
            runMultiplier = 0.75f;
        } else {
            heldBox.Interact(false);
            heldBox = null;
            runMultiplier = 1f;
        }
        boxPosition.GetComponent<SpriteRenderer>().enabled = active;
        boxPosition.GetComponent<BoxCollider2D>().enabled = active;
    }

    public void UpdateFacingDirection(bool isFacingLeft) {
        float newBoxPosX = Mathf.Abs(boxPosition.transform.localPosition.x);
        float newFinderPosX = Mathf.Abs(boxFinder.transform.localPosition.x);
        if (isFacingLeft) {
            newBoxPosX = -newBoxPosX;
            newFinderPosX = -newFinderPosX;
        }
        boxPosition.transform.localPosition = new Vector2(newBoxPosX, boxPosition.transform.localPosition.y);
        boxFinder.transform.localPosition = new Vector2(newFinderPosX, boxFinder.transform.localPosition.y);
        GetComponent<SpriteRenderer>().flipX = isFacingLeft;
    }

    // public virtual void PreventWallHanging() {
    //     // Check if the body's current velocity will result in a collision
    //     Vector3 direction = Vector2.up;
    //     if (moveLeft) {
    //         direction = Vector2.left * walkSpeed * runMultiplier;
    //     } else if (moveRight) {
    //         direction = Vector2.right * walkSpeed * runMultiplier;
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

    public override void FixedUpdate() {
        base.FixedUpdate();
        HandleMovement();
        HandleJumpHeld();
        // PreventWallHanging();
        HandleFalling();
        HandleClimb();
        UpdateBoxPos();
        // CheckLocks();
    }

    public void Interact() {
        if (currentInteraction != null && heldBox == null) {
            currentInteraction.Interact();
        }
    }

    public void HoldInteract(bool active) {

    }

    public void UpdateBox(InteractableBox box){
        if (heldBox != null) return;
        foundBox = box;
    }

    private void UpdateBoxPos() {
        if (heldBox != null) {
            heldBox.transform.position = boxPosition.transform.position;
        }
    }


    // public void EnterDoor() {
    //     if (currentTransition != null) {
    //         rigid.transform.position = currentTransition.link.spawnPoint.transform.position;
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = interact;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = interact;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.TryGetComponent(out InteractableObject interact)) {
            currentInteraction = null;
        }
    }

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
