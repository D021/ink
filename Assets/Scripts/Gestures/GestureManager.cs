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
		public float Threshold = 0.5f;
		//Private variables
		private Vector2 _firstPressPosition;
		private Vector2 _secondPressPosition;
		private Vector2 _currentSwipe;
		private GUIText GestureText;
		
		#endregion
		
		#region MonoBehavior Methods
		
		void Awake()
		{
			//GUI Text only for testing
			GestureText = FindObjectOfType<GUIText>();
			GestureText.fontSize = Screen.width / 8;
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
					GestureText.text = "TAP!";
				}
				if (t.phase == TouchPhase.Ended)
				{
					//save ended touch 2d point				
					_secondPressPosition = new Vector2(t.position.x, t.position.y);
					
					//create vector from the two points			
					_currentSwipe = new Vector3(_secondPressPosition.x - _firstPressPosition.x, _secondPressPosition.y - _firstPressPosition.y);
					
					//normalize the 2d vector				
					_currentSwipe.Normalize();
					
					//swipe upwards				
					if (_currentSwipe.y > 0 && _currentSwipe.x > -Threshold && _currentSwipe.x < Threshold)
					{
						GestureText.text = "Swipe UP!";
					}
					//swipe down				
					if (_currentSwipe.y < 0 && _currentSwipe.x > -Threshold && _currentSwipe.x < Threshold)
					{
						GestureText.text = "Swipe DOWN!";
					}
					//swipe left				
					if (_currentSwipe.x < 0 && _currentSwipe.y > -Threshold && _currentSwipe.y < Threshold)
					{
						GestureText.text = "Swipe LEFT!";
					}
					//swipe right				
					if (_currentSwipe.x > 0 && _currentSwipe.y > -Threshold && _currentSwipe.y < Threshold)
					{
						GestureText.text = "Swipe RIGHT!";
					}
				}
			}
		}		
		#endregion	
	}
}



