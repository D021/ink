﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterControllerRunner))]
public class InputManagerMouse : MonoBehaviour
{
    #region Variables

    //Public variables
    [Range(0, 1)]
    public float SwipeThreshold = 0.1f;
    public bool CanMove;
    public float AmountMove;
    public GameObject FrontTrigger;
    public float ChargingTime;
    public float FlyingTime;
	private string activeInkoke;


	public GameObject rinoFX;
	private GameObject rinoInstance;
	public Transform rinoPosInstance;



    //Private variables
    private Vector2 _firstPressPosition;
    private Vector2 _secondPressPosition;
    private Vector2 _currentSwipe;
    private InputManagerMouse _inputManager;
    private CharacterControllerRunner _characterController;
    private bool _jump;
    private GUIText _gestureText;
	private ShamanChangeSpineAnimation _shamanSpineController;


    #endregion

    #region MonoBehavior Methods

    void Awake()
    {
        _inputManager = GetComponent<InputManagerMouse>();
        _characterController = GetComponent<CharacterControllerRunner>();
        _gestureText = FindObjectOfType<GUIText>();
		_shamanSpineController = GetComponent<ShamanChangeSpineAnimation>();

        //_gestureText.fontSize = Screen.width / 8;

        //Linea añadida para empezar con el trigger off
        FrontTrigger.GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        //Testing
        if (Input.GetButtonDown("Jump"))
            _jump = true;

        CheckGesture();
    }

    void FixedUpdate()
    {
        // Read the inputs.
        if (CanMove)
        {
            float h = Input.GetAxis("Horizontal");
            _characterController.Move(h, _jump);
        }
        else
            // Pass all parameters to the character control script.
            _characterController.Move(AmountMove, _jump);

        // Reset the jump input once it has been used.
        _jump = false;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks the gesture.
    /// </summary>



    public void CheckGesture()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point			
            _firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //_startTime = Time.time;
            //_gestureText.text = "Tap!";
        }

        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point			
            _secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            _currentSwipe = new Vector2(_secondPressPosition.x - _firstPressPosition.x, _secondPressPosition.y - _firstPressPosition.y);

            _currentSwipe.x = _currentSwipe.x / Screen.width;
            _currentSwipe.y = _currentSwipe.y / Screen.height;


            //swipe ups
            if (_currentSwipe.y > SwipeThreshold && Mathf.Abs(_currentSwipe.x) < SwipeThreshold)
            {
//				print ( "SWIPE UP!");
//				_characterController.CancelJump();
//				//Start animation
//				_shamanSpineController.ChangeSpineAnimation("Fly", false);
//				
//				//Notify player items control to decreasse the level of ink
//				this.GetComponent<PlayerItems>().usingInk("Fly");
//				
//				StartCoroutine(inkvoking(FlyingTime, "Fly"));
            }

            //swipe down
            if (_currentSwipe.y < -SwipeThreshold && Mathf.Abs(_currentSwipe.x) < SwipeThreshold)
            {
            }

            //swipe left
            if (_currentSwipe.x < -SwipeThreshold && Mathf.Abs(_currentSwipe.y) < SwipeThreshold)
            {
            }

            //swipe right
            if (_currentSwipe.x > SwipeThreshold && Mathf.Abs(_currentSwipe.y) < SwipeThreshold)
            {
				print ( "SWIPE RIGHT!");
				FrontTrigger.GetComponent<BoxCollider2D>().enabled = true;
				
				if(activeInkoke != "Rush")
				{
					//Start animation
					_characterController.CancelJump();
					activeInkoke = "Rush";

					_shamanSpineController.ChangeSpineAnimation("Rush", true);
					//Activate the trigger in front of the player

					
					//Notify player items control to decreasse the level of ink
					this.GetComponent<PlayerItems>().usingInk("Rush");
//					GetComponent<PlayerLifeControl>().attacking=true;

					StartCoroutine(inkvoking(ChargingTime, "Rush"));

				}


            }

            //Tap
            if (Mathf.Abs(_currentSwipe.x) < SwipeThreshold && Mathf.Abs(_currentSwipe.y) < SwipeThreshold)
            {
                print ( "TAP!");
                _jump = true;
            }
        }
    }



	
	IEnumerator inkvoking(float _time, string _animation)
	{
		if(_animation == "Rush")
		{
			rinoInstance = (GameObject)Instantiate(rinoFX,rinoPosInstance.position,Quaternion.identity);
			rinoInstance.transform.parent = transform;
			Destroy(rinoInstance,1.2f);
		}
		_characterController.checkGrounded = false;
		yield return new WaitForSeconds(_time);
		_characterController.checkGrounded = true;
		activeInkoke = "";
		FrontTrigger.GetComponent<BoxCollider2D>().enabled = false;
//		GetComponent<PlayerLifeControl> ().attacking = false;
		
	}


    #endregion



















}
