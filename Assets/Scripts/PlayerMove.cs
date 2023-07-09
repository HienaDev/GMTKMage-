using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int health;
    public int Health { get; private set; }
    public bool Flashing { get; private set; }


    private SpriteRenderer spriteRendererPlayer;

    public bool Crouched { get; private set; }

    private float colliderNormalOffset = -1.5f;
    private float colliderNormalSize = 25f;

    private float colliderCrouchedOffset = -5f;
    private float colliderCrouchedSize = 17.6f;

    private PlayerSounds playerSounds;

    private bool dead = false;
    private bool deadAndOver = false;

    // Variables to check if the player is on the ground
    [SerializeField, Header("\nGroundCheck")] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 2f;
    [SerializeField] private float groundCheckSeparation;
    [SerializeField] private LayerMask groundMask;
    public bool Grounded { get; private set; }
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

    // Trail variables
    [SerializeField, Header("\nTrail")] private float trailTime;
    [SerializeField] private GameObject fader;
    private bool isTrailing = true;
    private float timeCounter = 0f;

    // Roll variables
    [SerializeField, Header("\nRoll")] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    private bool canDash = true;
    public bool IsDashing { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        
        ResetPlayer();

    }

    // Update is called once per frame
    void Update()
    {

        


        DetectGround();


        if(Health <= 0 && !dead)
        {
            dead = true;
            animator.SetTrigger("Death");
            animator.SetFloat("VelocityY", 0);
            playerSounds.playDeath();
            StartCoroutine(Death());
        }

        if(!IsDashing)
        { 
            groundCollider.enabled = Grounded;
            airCollider.enabled = !Grounded;
        }

        currentVelocity = rb.velocity;

        isTrailing = IsDashing;

        if(!dead)
        { 
            animator.SetFloat("VelocityY", currentVelocity.y);

            animator.SetBool("crouched", Crouched);
        }

        if (Input.GetAxis("Horizontal") * transform.right.x < 0 && !dead)
        {
            IsDashing = false;
        }

        if (!IsDashing && !dead)
        { 
            if(Crouched)
            {
                currentVelocity.x = Input.GetAxis("Horizontal") * crouchedMoveSpeed;

                groundCollider.size = new Vector2(groundCollider.size.x, colliderCrouchedSize);
                groundCollider.offset = new Vector2(groundCollider.offset.x, colliderCrouchedOffset);

            }
            else { 
                currentVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

                groundCollider.size = new Vector2(groundCollider.size.x, colliderNormalSize);
                groundCollider.offset = new Vector2(groundCollider.offset.x, colliderNormalOffset);

            }
        }

        if (!dead)
            animator.SetFloat("MoveSpeed", Mathf.Abs(currentVelocity.x));


        if (Grounded && currentVelocity.y <= 1e-3)
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

        if (Input.GetButtonDown("Jump") && (nJumps > 0) && !Crouched && !dead)
        {
            currentVelocity.y = jumpSpeed;
            lastJumpTime = Time.time;
            rb.gravityScale = jumpGravity;
            leftGround = Time.time - coyoteTime;
            nJumps--;
            playerSounds.playJump();
        }
        else if (Input.GetButton("Jump") && ((Time.time - lastJumpTime) <= jumpMaxTime) && currentVelocity.y > 0 && !dead)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
            lastJumpTime = 0;
        }

        if ((Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) && canDash && !dead)// && Grounded)
        {
            animator.SetTrigger("Roll");
            playerSounds.playDash();
            StartCoroutine(Dash());
        }

        if (Input.GetKey(KeyCode.S) && !dead)
        {
            Crouched = true;
        }
        else
        {
            Crouched = false;
        }

        if (isTrailing)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= trailTime)
            {
                timeCounter = 0;
                GameObject f = Instantiate(fader, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), transform.rotation) as GameObject;
                SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
                faderSprite.color = new Color(255f/255f, 200f/255f, 0, 0.4f);

                spriteRendererPlayer.color = new Color(255f / 255f, 200f / 255f, 0, 0.4f);

                faderSprite.sprite = GetComponentInChildren<SpriteRenderer>().sprite;
            }
        }
        else
        {
            spriteRendererPlayer.color = Color.white;
        }

        if (Flashing && Health >= 1)
        {
            spriteRendererPlayer.enabled = !spriteRendererPlayer.enabled;
        }
        else
        {
            spriteRendererPlayer.enabled = true;
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

        if (collider != null) Grounded = true;
        else
        {
            collider = Physics2D.OverlapCircle(groundCheck.position + transform.right * groundCheckSeparation, groundCheckRadius, groundMask);
            if (collider != null) Grounded = true;
            else
            {
                collider = Physics2D.OverlapCircle(groundCheck.position - transform.right * groundCheckSeparation, groundCheckRadius, groundMask);
                if (collider != null) Grounded = true;
                else
                {
                    Grounded = false;
                }
            }
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        IsDashing = true;
        currentVelocity = new Vector2(transform.right.x * defaultSpeed * dashingPower, currentVelocity.y);

        yield return new WaitForSeconds(dashingTime/2);

        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    IsDashing = false;
        //}

        yield return new WaitForSeconds(dashingTime/2);

        IsDashing = false;


        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        playerSounds.playHit();
        StartCoroutine(FlashPlayer());
    }

    private IEnumerator FlashPlayer()
    {
        Flashing = true;

        yield return new WaitForSeconds(2);

        Flashing = false;
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

    private IEnumerator Death()
    {
        
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.4f);

        Time.timeScale = 1f;

        yield return new WaitForSeconds(2f);

        deadAndOver = true;
    }
    

    public void IncreaseHealthUpgrade()
    {
        health += 1;
    }

    public void IncreaseJumpsUpgrade()
    {
        maxJumps += 1;
    }

    public void IncreaseSpeedUpgrade()
    {
        moveSpeed += 20;
    }

    public bool IsDead() => deadAndOver;

    public void KillPlayer()
    {
        deadAndOver = true;
    }

    public void ResetPlayer()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultSpeed = moveSpeed;
        animator = GetComponentInChildren<Animator>();

        playerSounds = GetComponent<PlayerSounds>();

        Health = health;

        spriteRendererPlayer = GetComponentInChildren<SpriteRenderer>();

        IsDashing = false;

        dead = false;
        deadAndOver = false;

        gameObject.transform.position = new Vector3(0f, -96f, 0f);
    }
}
