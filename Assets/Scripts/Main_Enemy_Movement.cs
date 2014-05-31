using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CharacterControllerRunner))]
public class Main_Enemy_Movement : MonoBehaviour {

	
	#region Variables
	
	//Public variables
	[Range(0, 1)]

	//public bool CanMove;
	public float AmountMove;
	public GameObject FrontTrigger;
//	public float ChargingTime;
//	public float FlyingTime;
	private string activeInkoke;
	private Transform frontTrigger;


	
	
	
	
	
	//Private variables

	private Main_Enemy_Movement _inputManager;

	private bool _jump;
	private GUIText _gestureText;
	private ShamanChangeSpineAnimation _shamanSpineController;

	public bool verticalJump;
	
	bool facingRight = true;							// For determining which way the player is currently facing.
	
	[SerializeField]
	float maxSpeed = 10f;
	public float maxSpeedRestart=10f;                    // The fastest the player can travel in the x axis.
	[SerializeField]
	float jumpForce = 400f;			                    // Amount of force added when the player jumps.	
	
	[SerializeField]
	LayerMask whatIsGround;	// A mask determining what is ground to the character

	Transform groundCheck;								// A position marking where to check if the player is grounded.
	public float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	public bool checkGrounded = true;


	
	private bool _verticalJumpRightTrigger;             //True -> Jump Right, False -> Jump Left
	
	public bool receiving_damage=false;
	
	public float respawnTime=0.7f;

	
	
	#endregion
	
	#region MonoBehavior Methods
	
	void Awake()
	{
		frontTrigger = transform.Find ("FrontTrigger");
		groundCheck = transform.Find("GroundCheck");
		_shamanSpineController = GetComponent<ShamanChangeSpineAnimation>();

		_inputManager = GetComponent<Main_Enemy_Movement>();
		print ("what is ground"+ whatIsGround);
		_gestureText = FindObjectOfType<GUIText>();
		_shamanSpineController = GetComponent<ShamanChangeSpineAnimation>();
		
		//_gestureText.fontSize = Screen.width / 8;
		
		//Linea añadida para empezar con el trigger on
		FrontTrigger.GetComponent<BoxCollider2D>().enabled = true;


		//Ignore Collision Between Main enemy and normal enemies
		Physics2D.IgnoreLayerCollision(13,10);

		
		//Ignore Collision Between Main enemy and ground platforms
		Physics2D.IgnoreLayerCollision(13,10);
	}

	
	void FixedUpdate()
	{	
		Move(AmountMove);
		
		// Reset the jump input once it has been used.
		_jump = false;

		// The enemy is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

		//check if the player has been reached
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontTrigger.position, 1);
		
		// Check each of the colliders.
		

		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is the player...game_over
			//print (c.tag);
			if(c.tag=="Player")
			{
				Application.LoadLevel(Application.loadedLevelName);
				break;
			}
			else if(c.tag=="Jump_trigger")
			{
				_jump=true;
				break;
			}
			else if(c.tag=="Stop_trigger")
			{
				maxSpeed=0.0f;
				break;
			}
			else if(c.tag=="Start_trigger")
			{
				maxSpeedRestart=10f;
				this.transform.position=c.transform.position;
				break;
			}
		}
	}
	
	#endregion
	
	#region Methods
	
	/// <summary>
	/// Actions for the main enemy
	/// </summary>
	

	IEnumerator inkvoking(float _time, string _animation)
	{
		checkGrounded = false;
		yield return new WaitForSeconds(_time);
		checkGrounded = true;
		activeInkoke = "";
		FrontTrigger.GetComponent<BoxCollider2D>().enabled = false;
		//GetComponent<PlayerLifeControl> ().attacking = false;
		
	}

	public void Move(float move)
	{
		//only control the player if grounded or airControl is turned on
		if (grounded && !receiving_damage) {
			if (checkGrounded)
				_shamanSpineController.ChangeSpineAnimation ("Run", true);
			verticalJump = false;
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			
			// Move the character
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
			//print(move);
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
				// ... flip the player.
				Flip ();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
				// ... flip the player.
				Flip ();
		}
		
		if (verticalJump && _jump) {
			if (_verticalJumpRightTrigger)
				rigidbody2D.velocity = new Vector2 (-move * maxSpeed, 0);
			else
				rigidbody2D.velocity = new Vector2 (move * maxSpeed, 0);
			
			Flip ();
			rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			verticalJump = false;
		}
		
		// If the player should jump...
		if (grounded && _jump) {
			// Add a vertical force to the enemy.
			checkGrounded = false;
			rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			_shamanSpineController.ChangeSpineAnimation ("Jump", true);
			StartCoroutine (waitGrounded (0.2f));
			
		}
		if (grounded && receiving_damage) {
			rigidbody2D.velocity = new Vector2 (move * (maxSpeed*0.5f), rigidbody2D.velocity.y);
			//rigidbody2D.AddForce (new Vector2 (move * (maxSpeed*0.5f),rigidbody2D.velocity.y));
			StartCoroutine (being_hurt());
			
		}
		
	}
	
	public IEnumerator being_hurt()
	{
		yield return new WaitForSeconds(respawnTime);
		receiving_damage=false;
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
	
	
	#endregion

}