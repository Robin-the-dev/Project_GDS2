using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    [HideInInspector]
    public InteractableBox foundBox = null;
    [HideInInspector]
    public InteractableBox heldBox = null;
    [HideInInspector]
    public InteractableObject currentInteraction = null;

    public GameObject boxPosition;
    public BoxFinder boxFinder;

    public float detectionRange = 1f;

    public float minGrappleDistance = 0.5f;
    public float grappleDistance = 3.5f;
    public float maxGrappleDistance = 6f;

    private SpringJoint2D joint;

    private float currentGrappleDistance = 0;

    public LayerMask grapplelayer;
    public LineRenderer ropeRender;
    public GameObject handObject;
    private GameObject currentGrapplePoint = null;
    private List<Collider2D> grapplePoints;

    private bool hasHitMark = false;
    private Vector2 markPosition = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    public int maxRopePoints = 5;
    private int ropePoint = 0;

    public float ropeCooldown = 5f;
    [HideInInspector]
    public float ropeTimeLeft = 0f;

    [HideInInspector]
    public bool onLadder = false;

    public bool inWater = false;

    public override void Start() {
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        joint = GetComponent<SpringJoint2D>();
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
        if (active && currentGrapplePoint == null) {
            SetNearestGrapple(mousePosition);
            if (currentGrapplePoint != null) {
                doGrappleAction();
                return;
            }
        }
        if (!active && currentGrapplePoint != null) {
            if (currentGrapplePoint == null) return;
            // anim.SetBool("Grappling", false);
            joint.connectedBody = null;
            joint.enabled = false;
            currentGrapplePoint = null;
            // if (ropeTimeLeft <= 0) {
            //     rigid.velocity = Vector3.ClampMagnitude(rigid.velocity * 3, maxGrappleVelocity/2);
            // }
        } else if (foundBox != null && heldBox == null && active) {
            UpdateBoxState(true);
        } else if (heldBox != null && !active) {
            UpdateBoxState(false);
        }
    }

    public void doGrappleAction() {
        // anim.SetTrigger("Special");
        // anim.SetBool("Grappling", true);
        joint.connectedBody = currentGrapplePoint.GetComponent<Rigidbody2D>();
        joint.enabled = true;
        hasHitMark = false;
        ropePoint = 1;
        ropeTimeLeft = ropeCooldown;
        currentGrappleDistance = grappleDistance;
        joint.distance = currentGrappleDistance;
    }

    private void SetNearestGrapple(Vector3 target) {
        // set up filter to only collect grapple points
        ContactFilter2D filter = new ContactFilter2D();
        grapplePoints = new List<Collider2D>();
        filter.SetLayerMask(LayerMask.GetMask("Grapple"));
        filter.useTriggers = true;
        // test if any grapple points exist within range
        Physics2D.OverlapCircle(target, detectionRange, filter, grapplePoints);
        Collider2D nearest = null;
        float dist = -1;
        //find the nearest grapple point to mouse position
        foreach (Collider2D p in grapplePoints) {
            float newDist = Vector2.Distance(target, p.transform.position);
            float distPlayer = Vector2.Distance(transform.position, p.transform.position);
            if (dist == -1 && CanReachGrapple(p.gameObject.transform) && distPlayer < maxGrappleDistance) {
                nearest = p;
                dist = newDist;
            } else {
                if (newDist < dist && CanReachGrapple(p.gameObject.transform) && distPlayer < maxGrappleDistance) {
                    nearest = p;
                    dist = newDist;
                }
            }
        }
        //update grapple point
        if (nearest != null) {
            currentGrapplePoint = nearest.gameObject;
        }
    }

    private bool CanReachGrapple(Transform grapple) {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(~LayerMask.GetMask("Player", "AntiWallGrab", "SupportPlayer"));
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        Physics2D.Raycast(transform.position, (grapple.position - transform.position), filter, results, detectionRange);
        return results.Count == 0;
    }

    public virtual void Jump() {
        if (IsGrounded()) {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
            anim.SetTrigger("Jump");
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
            runMultiplier = 1.25f;
        } else {
            runMultiplier = 1f;
        }
    }

    public virtual void HandleJumpHeld() {
        // default does nothing
    }

    public void HandleClimb() {
        if (currentGrapplePoint == null) return;
        float playerDistance = Vector2.Distance(currentGrapplePoint.transform.position, handObject.transform.position);
        if (playerDistance < currentGrappleDistance) currentGrappleDistance = playerDistance;
        if (climbUp) {
            currentGrappleDistance -= 4 * Time.deltaTime;
        } else if (climbDown) {
            currentGrappleDistance += 4 * Time.deltaTime;
        }
        if (currentGrappleDistance > maxGrappleDistance) currentGrappleDistance = maxGrappleDistance;
        if (currentGrappleDistance < minGrappleDistance) currentGrappleDistance = minGrappleDistance;
    }

    public void DoWaterMovement() {
        if (climbUp) {
            rigid.velocity = Vector2.up * walkSpeed * runMultiplier / 2;
        } else if (climbDown) {
            rigid.velocity = Vector2.down * walkSpeed * runMultiplier / 2;
        }
    }


    public virtual void HandleMovement() {
        // setAnimation Triggers
        anim.SetBool("OnGround", IsGrounded());
        anim.SetBool("Moving", Mathf.Abs(rigid.velocity.x) > 0.5);

        if (inWater) {
            DoWaterMovement();
        } 

        // don't use ground movement if grappeling
        if (currentGrapplePoint != null) return;

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
        boxPosition.GetComponent<PolygonCollider2D>().enabled = active;
    }

    public void UpdateFacingDirection(bool isFacingLeft) {
        float newBoxPosX = Mathf.Abs(boxPosition.transform.localPosition.x);
        float newFinderPosX = Mathf.Abs(boxFinder.transform.localPosition.x);
        if (isFacingLeft) {
            newBoxPosX = -newBoxPosX;
            newFinderPosX = -newFinderPosX;
            boxPosition.transform.localScale = new Vector3(-Mathf.Abs(boxPosition.transform.localScale.x), boxPosition.transform.localScale.y, boxPosition.transform.localScale.z);
        } else {
            boxPosition.transform.localScale = new Vector3(Mathf.Abs(boxPosition.transform.localScale.x), boxPosition.transform.localScale.y, boxPosition.transform.localScale.z);
        }
        boxPosition.transform.localPosition = new Vector2(newBoxPosX, boxPosition.transform.localPosition.y);
        boxFinder.transform.localPosition = new Vector2(newFinderPosX, boxFinder.transform.localPosition.y);
        GetComponent<SpriteRenderer>().flipX = isFacingLeft;
    }


    public virtual void HandleFalling() {
        if (currentGrapplePoint != null || inWater) return;
        if (rigid.velocity.y < 0) {
            anim.SetBool("Falling", true);
            rigid.velocity += Vector2.ClampMagnitude(Vector3.up * 2.5f * Physics.gravity.y * Time.deltaTime, jumpHeight*5);
        } else if (rigid.velocity.y > 0) {
            anim.SetBool("Falling", false);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        HandleMovement();
        HandleJumpHeld();
        HandleFalling();
        HandleClimb();
        HandleGrapple();
        UpdateBoxPos();
        RenderGrapple();
        // CheckLocks();
    }

    public void Interact() {
        if (currentInteraction != null && heldBox == null) {
            currentInteraction.Interact();
        }
    }

    public void HoldInteract(bool active) {

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

    public bool IsGrounded() {
        float width = (transform.localScale.x * 0.15f) * 1.5f;
        float height = (transform.localScale.y * 0.4f) * 1.5f;

        Vector2 corner1Start = new Vector2(rigid.position.x + width, rigid.position.y);
        Vector2 corner1End = Vector3.down * height;
        Vector2 corner2Start = new Vector2(rigid.position.x - width, rigid.position.y);
        Vector2 corner2End = Vector3.down * height;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(~LayerMask.GetMask("Player", "AntiWallGrab", "SupportPlayer"));
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

    private void UpdateSwingAngle(Vector2 grapplePosition, Vector2 handPosition) {
        // calculate mark position
        if (!moveLeft && !moveRight) return;
        Vector2 targetVector = grapplePosition - handPosition;
        float swingAngle = Vector2.SignedAngle(targetVector, Vector2.up);

        float anglePercent = 1 - Mathf.Abs(swingAngle)/120;
        if (moveLeft) {
            swingAngle -= anglePercent * 100;
        } else if(moveRight) {
            swingAngle += anglePercent * 100;
        }

        Vector2 tempPos = new Vector2();
        tempPos.x = grapplePosition.x + (currentGrappleDistance * Mathf.Sin(swingAngle/180 * Mathf.PI));
        tempPos.y = grapplePosition.y + (currentGrappleDistance * Mathf.Cos(swingAngle/180 * Mathf.PI));

        rigid.AddForce((tempPos - new Vector2(transform.position.x, transform.position.y)));
    }


    private void HandleGrapple(){
        if (currentGrapplePoint != null && hasHitMark) {
            joint.distance = currentGrappleDistance;
            UpdateSwingAngle(currentGrapplePoint.transform.position, handObject.transform.position);
        }
    }

    private void RenderGrapple() {
        if (currentGrapplePoint != null && !hasHitMark) {
            ropeRender.enabled = true;
            ropeRender.SetPosition(0, handObject.transform.position);
            Vector2 mark = GetMarkPosition();
            ropeRender.SetPosition(1, mark);
            ropePoint++;
            if (ropePoint == maxRopePoints) hasHitMark = true;
        } else if (currentGrapplePoint != null && hasHitMark) {
            ropeRender.enabled = true;
            ropeRender.SetPosition(0, handObject.transform.position);
            ropeRender.SetPosition(1, currentGrapplePoint.transform.position);
        } else {
            ropeRender.enabled = false;
        }
    }

    private Vector2 GetMarkPosition() {
        float distance = Vector2.Distance(handObject.transform.position, currentGrapplePoint.transform.position) * ropePoint / maxRopePoints;

        Vector2 grapplePosition = currentGrapplePoint.transform.position;
        Vector2 handPosition = handObject.transform.position;
        Vector2 targetVector = handPosition - grapplePosition;
        float angle = Vector2.SignedAngle(targetVector, Vector3.down);

        Vector2 tempPos = new Vector2();
        tempPos.x = handPosition.x + (distance * Mathf.Sin(angle/180 * Mathf.PI));
        tempPos.y = handPosition.y + (distance * Mathf.Cos(angle/180 * Mathf.PI));
        return new Vector2(tempPos.x, tempPos.y);
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
}
