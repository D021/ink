﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterControllerRunner))]
public class InputManager : MonoBehaviour
{
     #region Variables

        //Public variables
        [Range(0,1)]
        public float SwipeThreshold = 0.1f;
        public bool CanMove;
        public float AmountMove;
		public GameObject FrontTrigger;
		public float ChargingTime;





        //Private variables
        private Vector2 _firstPressPosition;
        private Vector2 _secondPressPosition;
        private Vector2 _currentSwipe;
        private InputManager _inputManager;
        private CharacterControllerRunner _characterController;
        private bool _jump;
	private GUIText _gestureText;

        #endregion

        #region MonoBehavior Methods

        void Awake()
        {
            _inputManager = GetComponent<InputManager>();
            _characterController = GetComponent<CharacterControllerRunner>();
		_gestureText = FindObjectOfType<GUIText>();
		//_gestureText.fontSize = Screen.width / 8;

		//Linea añadida para empezar con el trigger off
		//FrontTrigger.GetComponent<BoxCollider2D>().enabled = false;
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
        private void CheckGesture()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point				
                    _firstPressPosition = new Vector2(t.position.x, t.position.y);
                }
                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point				
                    _secondPressPosition = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points			
                    _currentSwipe = new Vector3(_secondPressPosition.x - _firstPressPosition.x, _secondPressPosition.y - _firstPressPosition.y);

                    _currentSwipe.x = _currentSwipe.x / Screen.width;
                    _currentSwipe.y = _currentSwipe.y / Screen.height;

                    //swipe ups
                    if (_currentSwipe.y > SwipeThreshold && Mathf.Abs(_currentSwipe.x) < SwipeThreshold)
                    {
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
					_gestureText.text = "SWIPE RIGHT!";
					//Start animation
					_characterController.boolAnimation("Charge", true);
					//Activate the trigger in front of the player
					FrontTrigger.GetComponent<BoxCollider2D>().enabled = true;
					StartCoroutine(charging());
					}

                    //Tap
                    if (Mathf.Abs(_currentSwipe.x) < SwipeThreshold && Mathf.Abs(_currentSwipe.y) < SwipeThreshold)
                    {
					_gestureText.text = "TAP!";
						_jump = true;
                    }
                }
            }
        }

	IEnumerator charging(){
		yield return new WaitForSeconds(ChargingTime);
		_characterController.boolAnimation("Charge", false);
	}

        #endregion












    





    
}
