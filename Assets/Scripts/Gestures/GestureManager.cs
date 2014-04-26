using UnityEngine;

namespace com.inkrunner.gestures
{
    /// <summary>
    /// Detects the Touch Gestures in the game
    /// </summary>
    public class GestureManager : MonoBehaviour
    {
        #region Variables

        //Public variables
        [Range(0,1)]
        public float SwipeThreshold = 0.1f;
        //Private variables
        private Vector2 _firstPressPosition;
        private Vector2 _secondPressPosition;
        private Vector2 _currentSwipe;
        private InputManagerGesture _inputManager;
        //private GUIText _gestureText;

        #endregion

        #region MonoBehavior Methods

        void Awake()
        {
            _inputManager = GetComponent<InputManagerGesture>();
            //GUI Text only for testing
            //_gestureText = FindObjectOfType<GUIText>();
            //_gestureText.fontSize = Screen.width / 8;
        }

        void Update()
        {
            CheckGesture();
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
                    }

                    //Tap
                    if (Mathf.Abs(_currentSwipe.x) < SwipeThreshold && Mathf.Abs(_currentSwipe.y) < SwipeThreshold)
                    {
                    }
                }
            }
        }

        #endregion
    }
}



