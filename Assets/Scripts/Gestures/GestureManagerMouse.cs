using UnityEngine;
using System.Collections;

namespace com.inkrunner.gestures
{
	/// <summary>
	/// This class is for test the functionality in the Unity Editor
	/// </summary>
	public class GestureManagerMouse : MonoBehaviour {
		
		private Vector2 _firstPressPos;	
		private Vector2 _secondPressPos;	
		private Vector2 _currentSwipe;
		private GUIText SwipeText;
		
		void Awake()
		{
			SwipeText = FindObjectOfType<GUIText> ();
		}	
		
		void Update()
		{
			CheckGesture ();
		}
		
		public void CheckGesture()		
		{		
			if(Input.GetMouseButtonDown(0))			
			{		
				//save began touch 2d point			
				_firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);	
				SwipeText.text = "Tap!";
			}
			
			if(Input.GetMouseButtonUp(0))			
			{			
				//save ended touch 2d point			
				_secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);		
				
				//create vector from the two points
				_currentSwipe = new Vector2(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y); 
				
				//normalize the 2d vector
				_currentSwipe.Normalize();
				
				//swipe upwards
				if(_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
				{				
					Debug.Log("up swipe");
					SwipeText.text = "Swipe UP!";
				}
				
				//swipe down
				if(_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
				{
					Debug.Log("down swipe");
					SwipeText.text = "Swipe DOWN!";
				}
				
				//swipe left
				if(_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
				{
					Debug.Log("left swipe");
					SwipeText.text = "Swipe LEFT!";
				}
				
				//swipe right
				if(_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
				{
					Debug.Log("right swipe");
					SwipeText.text = "Swipe RIGHT!";
				}
				
			}
			
		}
	}
}
