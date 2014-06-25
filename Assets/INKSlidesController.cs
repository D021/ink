using UnityEngine;
using System.Collections;


namespace com.inkrunner.camera
{	
	public class INKSlidesController : MonoBehaviour {
	
		public LoopSlidesTrigger[] _loopSlides;
		private CameraSlidesScript _cameraSlidesScript;
			// Use this for initialization
		void Start () {
			_cameraSlidesScript = this.GetComponent<CameraSlidesScript>();
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				switch(_cameraSlidesScript.ActualSlide())
				{
					
				case 1:
					_loopSlides[0].StartDemoShaman();
					break;
					
				case 2:
					_loopSlides[0].StopDemoShaman();
					break;
					
				case 4:
					Time.timeScale = 1;
					break;

				case 5:
					Time.timeScale = 0;
					break;

				case 6:
					Time.timeScale = 1;
					break;
					
				}
			}

		}
	}
}

