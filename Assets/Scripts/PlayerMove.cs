using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    //[RequireComponent()]

    // Player Variables (Colliders, speed, rigidbody, animations)

    [SerializeField, Header("Player")] private float moveSpeed = 100f;
    [SerializeField] private float crouchedMoveSpeed = 50f;
    [SerializeField] private BoxCollider2D groundCollider;
    [SerializeField] private CapsuleCollider2D airCollider;
    private float defaultSpeed;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Animator animator;

    private bool crouched = false;

    private float colliderNormalOffset = -1.5f;
    private float colliderNormalSize = 25f;

    private float colliderCrouchedOffset = -5f;
    private float colliderCrouchedSize = 17.6f;

    // Variables to check if the player is on the ground
    [SerializeField, Header("\nGroundCheck")] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 2f;
    [SerializeField] private float groundCheckSeparation;
    [SerializeField] private LayerMask groundMask;
    private bool grounded;
    private float leftGround;

    // Variables related to the player's jump
    [SerializeField, Header("\nJump")] private float jumpMaxTime;
    [SerializeField] private float jumpSpeed = 125f;
    [SerializeField] private float coyoteTime;
    [SerializeField] private int maxJumps;
    [SerializeField] private float jumpGravity;
    [SerializeField] private float defaultGravity;
    private float lastJumpTime;
    private int nJumps;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultSpeed = moveSpeed;
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();

        groundCollider.enabled = grounded;
        airCollider.enabled = !grounded;

        currentVelocity = rb.velocity;

        

        animator.SetFloat("VelocityY", currentVelocity.y);

        animator.SetBool("crouched", crouched);


        if(crouched)
        {
            currentVelocity.x = Input.GetAxis("Horizontal") * crouchedMoveSpeed;

            groundCollider.size = new Vector2(groundCollider.size.x, colliderCrouchedSize);
            groundCollider.offset = new Vector2(groundCollider.offset.x, colliderCrouchedOffset);

            airCollider.size = new Vector2(airCollider.size.x, colliderCrouchedSize);
            airCollider.offset = new Vector2(airCollider.offset.x, colliderCrouchedOffset);
        }
        else { 
            currentVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

            groundCollider.size = new Vector2(groundCollider.size.x, colliderNormalSize);
            groundCollider.offset = new Vector2(groundCollider.offset.x, colliderNormalOffset);

            airCollider.size = new Vector2(airCollider.size.x, colliderNormalSize);
            airCollider.offset = new Vector2(airCollider.offset.x, colliderNormalOffset);
        }

        animator.SetFloat("MoveSpeed", Mathf.Abs(currentVelocity.x));


        if (grounded && currentVelocity.y <= 1e-3)
        {
            leftGround = Time.time;

        }

        if ((Time.time - leftGround) <= coyoteTime)
        {
            nJumps = maxJumps;
        }
        else
        {
            if (nJumps == maxJumps)
            {
                nJumps = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && (nJumps > 0) && !crouched)
        {
            currentVelocity.y = jumpSpeed;
            lastJumpTime = Time.time;
            rb.gravityScale = jumpGravity;
            leftGround = Time.time - coyoteTime;
            nJumps--;
        }
        else if (Input.GetButton("Jump") && ((Time.time - lastJumpTime) <= jumpMaxTime) && currentVelocity.y > 0)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
            lastJumpTime = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            crouched = true;
        }
        else
        {
            crouched = false;
        }


        rb.velocity = currentVelocity;


        FlipPlayer();
    }

    private void FlipPlayer()
    {
        if (currentVelocity.x < 0 && transform.right.x > 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (currentVelocity.x > 0 && transform.right.x < 0)
            transform.rotation = Quaternion.identity;

        // https://answers.unity.com/questions/640162/detect-mouse-in-right-side-or-left-side-for-player.html
        if (Input.GetMouseButton(1))
        {
            var playerScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            if (Input.mousePosition.x < playerScreenPoint.x)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                transform.rotation = Quaternion.identity;
        }
        //Debug.Log($"Input.mousePosition: {(Input.mousePosition.x - 615) / 2},{(Input.mousePosition.y - 360) / 2}");
        //Debug.Log($"gameObject.transform.position: {gameObject.transform.position}");
    }


    private void DetectGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        if (collider != null) grounded = true;
        else
        {
            collider = Physics2D.OverlapCircle(groundCheck.position + transform.right * groundCheckSeparation, groundCheckRadius, groundMask);
            if (collider != null) grounded = true;
            else
            {
                collider = Physics2D.OverlapCircle(groundCheck.position - transform.right * groundCheckSeparation, groundCheckRadius, groundMask);
                if (collider != null) grounded = true;
                else
                {
                    grounded = false;
                }
            }
        }

    }


    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position + transform.right * groundCheckSeparation, groundCheckRadius);
            Gizmos.DrawWireSphere(groundCheck.position - transform.right * groundCheckSeparation, groundCheckRadius);
        }
    }

    
}
