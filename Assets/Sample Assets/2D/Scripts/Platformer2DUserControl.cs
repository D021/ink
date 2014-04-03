using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour 
{
	private PlatformerCharacter2D character;
    private bool jump;
	public bool canMove;
	public float amountMove;

	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
//#if CROSS_PLATFORM_INPUT
//        if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
//#else
		if (Input.GetButtonDown("Jump")) jump = true;
//#endif

    }

	void FixedUpdate()
	{
		// Read the inputs.
		if(canMove){
			//		#if CROSS_PLATFORM_INPUT
			//		float h = CrossPlatformInput.GetAxis("Horizontal");
			//		#else
			bool crouch = Input.GetKey(KeyCode.LeftControl);
			float h = Input.GetAxis("Horizontal");
			character.Move( h, crouch , jump );
		}else
			// Pass all parameters to the character control script.
			character.Move( amountMove, false , jump );

        // Reset the jump input once it has been used.
	    jump = false;
	}
}
