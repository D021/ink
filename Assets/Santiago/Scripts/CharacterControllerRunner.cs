using UnityEngine;

public class CharacterControllerRunner : MonoBehaviour
{
    public bool verticalJump;

    bool facingRight = true;							// For determining which way the player is currently facing.

    [SerializeField]
    float maxSpeed = 10f;				                // The fastest the player can travel in the x axis.
    [SerializeField]
    float jumpForce = 400f;			                    // Amount of force added when the player jumps.	

    [SerializeField]
    LayerMask whatIsGround;			                    // A mask determining what is ground to the character

    Transform groundCheck;								// A position marking where to check if the player is grounded.
    float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
    bool grounded = false;								// Whether or not the player is grounded.
    Animator anim;										// Reference to the player's animator component.

    private bool _verticalJumpRightTrigger;             //True -> Jump Right, False -> Jump Left

    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        // Set the vertical animation
        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

    }


    public void Move(float move, bool jump)
    {


        //only control the player if grounded or airControl is turned on
        if (grounded)
        {
            verticalJump = false;
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            //if (move > 0 && !facingRight)
            // ... flip the player.
            //Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            //else if (move < 0 && facingRight)
            // ... flip the player.
            //Flip();
        }

        if (verticalJump && jump)
        {
            if (_verticalJumpRightTrigger)
                rigidbody2D.velocity = new Vector2(-move * maxSpeed, 0);
            else
                rigidbody2D.velocity = new Vector2(move * maxSpeed, 0);

            //Flip();
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            verticalJump = false;
        }

        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);

            rigidbody2D.AddForce(new Vector2(0f, jumpForce));

        }
    }

    public void boolAnimation(string _stringVariable, bool _bool)
    {
        anim.SetBool(_stringVariable, _bool);
    }

    //void Flip()
    //{
    //    // Switch the way the player is labelled as facing.
    //    facingRight = !facingRight;

    //    // Multiply the player's x local scale by -1.
    //    Vector3 theScale = transform.localScale;
    //    theScale.x *= -1;
    //    transform.localScale = theScale;
    //}

    public void CancelJump()
    {
        //Reset the y velocity of the player
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
    }

    public void VerticalJumpSide(bool _side)
    {
        _verticalJumpRightTrigger = _side;
    }

}
