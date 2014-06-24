using UnityEngine;
using System.Collections;

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
    public float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
    bool grounded = false;								// Whether or not the player is grounded.
    public bool checkGrounded = true;

    private ShamanChangeSpineAnimation _shamanSpineController;


    private bool _verticalJumpRightTrigger;             //True -> Jump Right, False -> Jump Left

    public bool receiving_damage = false;

    public float respawnTime = 0.7f;

    public GameObject[] stepsFX;
    public Transform stepsFXPositions;
    public bool stepsChecks;
    public float stepsRadius;
    public float timeStepFX;
    private bool canInstantiateFX = true;

    public GameObject jumpFx;

    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        _shamanSpineController = GetComponent<ShamanChangeSpineAnimation>();

    }


    void FixedUpdate()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }


    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (grounded && !receiving_damage)
        {
            if (checkGrounded)
            {
                _shamanSpineController.ChangeSpineAnimation("Run", true);

                if (canInstantiateFX)
                {
                    stepsChecks = Physics2D.OverlapCircle(stepsFXPositions.position, stepsRadius, whatIsGround);
                    if (stepsChecks)
                    {
                        Instantiate(stepsFX[0], stepsFXPositions.position, Quaternion.identity);
                        Instantiate(stepsFX[1], stepsFXPositions.position, Quaternion.identity);
                    }

                    canInstantiateFX = false;
                    StartCoroutine(waitStepGrounded());
                }
            }

            verticalJump = false;
            // The Speed animator parameter is set to the absolute value of the horizontal input.

            // Move the character
            rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
                // ... flip the player.
                Flip();
        }

        if (verticalJump && jump)
        {
            if (_verticalJumpRightTrigger)
                rigidbody2D.velocity = new Vector2(-move * maxSpeed, 0);
            else
                rigidbody2D.velocity = new Vector2(move * maxSpeed, 0);

            Flip();
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            verticalJump = false;
        }

        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            checkGrounded = false;
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            _shamanSpineController.ChangeSpineAnimation("Jump", true);
            StartCoroutine(waitGrounded(0.2f));
            Instantiate(jumpFx, groundCheck.position, Quaternion.identity);
        }
        if (grounded && receiving_damage)
        {
            rigidbody2D.velocity = new Vector2(move * (maxSpeed * 0.5f), rigidbody2D.velocity.y);
            //rigidbody2D.AddForce (new Vector2 (move * (maxSpeed*0.5f),rigidbody2D.velocity.y));
            StartCoroutine(being_hurt());

        }

    }

    public IEnumerator being_hurt()
    {
        yield return new WaitForSeconds(respawnTime);
        receiving_damage = false;
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void CancelJump()
    {
        //Reset the y velocity of the player
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
    }

    public void VerticalJumpSide(bool _side)
    {
        _verticalJumpRightTrigger = _side;
    }

    IEnumerator waitGrounded(float _time)
    {
        yield return new WaitForSeconds(_time);
        checkGrounded = true;
    }

    IEnumerator waitStepGrounded()
    {
        yield return new WaitForSeconds(timeStepFX);
        canInstantiateFX = true;
    }

}
