using UnityEngine;

[RequireComponent(typeof(ControladorPersonaje))]
public class InputsJugador : MonoBehaviour
{
    private ControladorPersonaje character;
    private bool jump;
    public bool canMove;
    public float amountMove;

    void Awake()
    {
        character = GetComponent<ControladorPersonaje>();
    }

    void Update()
    {
        // Read the jump input in Update so button presses aren't missed.
        //#if CROSS_PLATFORM_INPUT
        //        if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
        //#else
        if (Input.GetButtonDown("Jump")) jump = true;
        //#endif

    }

    public void Jump()
    {
        jump = true;
    }

    void FixedUpdate()
    {
        // Read the inputs.
        if (canMove)
        {
            //		#if CROSS_PLATFORM_INPUT
            //		float h = CrossPlatformInput.GetAxis("Horizontal");
            //		#else
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis("Horizontal");
            character.Move(h, crouch, jump);
        }
        else
            // Pass all parameters to the character control script.
            character.Move(amountMove, false, jump);

        // Reset the jump input once it has been used.
        jump = false;
    }
}
