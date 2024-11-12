using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Regions help visually organize your code into sections
    #region Variables
    // Headers are like titles for the Unity inspector
    [Header("Movement Variables")]

    /* In C# if you do not specify a variable modifier (i.e. public, private, protected), it defaults to private
    The private variable modifier stops other scripts from accessing those variables */
    Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;//effectormask


    // SerializeField allows you to see private variables in the inspector while keeping them private
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 10f;





    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = .4f;
    [SerializeField] float dashCooldown = 1.5f;
    private bool isDashing = false;
    private float lastDashTime;
    private Vector2 lastDirection = Vector2.right; // Default to facing right for the dashing







    //part of stub that i wont use
    //    bool jumpRequested; // A boolean to check if the player has requested a jump.

    float movement; // The horizontal movement of the player
    float time = 0;

    [SerializeField] GameObject spawnPoint;


    private int jumpCount;
    bool hasNewJump; //used for bootcamp custom methods about changing jump height
    float jumpForceOld = 12f;

    private bool isGrounded;
    private enum MovementState { idle, running, jumping, falling } //animationstate enumerator order


    #endregion // Marks the end of the region

    #region Unity Methods
    // Start is called before the first frame update
    private void Start()
    {
        // GetComponent is used to get the component of the object this script is attached to
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        jumpForceOld = jumpForce;
    }

    // Update is called once per frame
    private void Update()
    {
        // Input.GetAxisRaw returns a float value of either -1 or 1 based on if the player is pressing left or right. It is 0 if the player is not pressing anything.
        // Use GetAxis instead if you want smooth movement with acceleration and deceleration.
        movement = Input.GetAxisRaw("Horizontal");


        isGrounded = IsGrounded();

        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }


        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time > lastDashTime + dashCooldown)
        {
            Debug.Log("Dash initiated");

            StartCoroutine(Dash());
        }



        UpdateAnimationState();


        
        
        //NOT USING, BUT IF I COMMENT THIS IT MAKES OTHER SCRIPTS GO CRAZY
        if (hasNewJump)//stuff from bootcamp, logic to change jump height for powerups
        {
            if (time >= 5)
            {
                hasNewJump = false;
                jumpForce = jumpForceOld;
                time = 0;
                setJumpForce(jumpForce);
            }
            time += Time.deltaTime;
        }
    }

    // FixedUpdate is used for physics calculations and is called 50 times a second
    private void FixedUpdate()
    {

        if (!isDashing)
        {
            rb.velocity = new Vector2(movement * moveSpeed, rb.velocity.y); // Regular movement
        }

        /*  // Handle the jump request
          if (jumpRequested)
          {
              Jump();
              jumpRequested = false; // Reset the jump request flag
          }*/
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// This method is used to update the sprite direction based on the player's movement
    /// </summary>
    private void UpdateAnimationState()
    {
        MovementState state;
        if (movement > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            lastDirection = Vector2.right;//setting direction for dash
        }
        else if (movement < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
            lastDirection = Vector2.left; //setting direction for dash

        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    /// <summary>
    /// This method is used to make the player jump
    /// </summary>
    private void Jump()
    {
        // jump from ground logic
        if (isGrounded)
        {
            Debug.Log("Ground Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // Mid-air jump logic
        else if (!isGrounded && jumpCount < 1) // Allow one mid-air jump
        {
            Debug.Log("Mid-Air Jump");
            jumpCount++; // Increment jump count
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public float getJumpForce()
    {
        return jumpForce;
    }
    public void setJumpForce(float newJump)
    {
        jumpForce = newJump;
        hasNewJump = true;

    }


    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Use the last direction to determine the dash direction
        Vector2 dashDirection = lastDirection;

        // Apply the impulse force in the dash direction
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        // Wait for the duration of the dash
        yield return new WaitForSeconds(dashDuration);

        //Apply a small drag effect to slow down after dashing
        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y); // Reduce speed

        isDashing = false;
    }



}



#endregion