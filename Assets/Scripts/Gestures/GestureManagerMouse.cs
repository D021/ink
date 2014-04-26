using UnityEngine;
using System.Collections;

namespace com.inkrunner.gestures
{
    /// <summary>
    /// This class is for test the functionality in the Unity Editor
    /// </summary>
    public class GestureManagerMouse : MonoBehaviour
    {
        public float SwipeThreshold = 0.1f;
        private Vector2 _firstPressPosition;
        private Vector2 _secondPressPosition;
        private Vector2 _currentSwipe;
        private InputManagerGesture _inputManager;


        void Awake()
        {
			_inputManager = GetComponent<InputManagerGesture>();
        }

        void Update()
        {
            CheckGesture();
        }

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

                _currentSwipe.x = _currentSwipe.x/Screen.width;
                _currentSwipe.y = _currentSwipe.y / Screen.height;

                Debug.Log("Y movement: " + _currentSwipe.y);
                Debug.Log("X movement: " + _currentSwipe.x);

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
}
