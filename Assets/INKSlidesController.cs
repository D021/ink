using UnityEngine;
using System.Collections;


namespace com.inkrunner.camera
{	
	public class INKSlidesController : MonoBehaviour {
	
		private CameraSlidesScript _cameraSlidesScript;
			// Use this for initialization
		void Start () {
			_cameraSlidesScript = this.GetComponent<CameraSlidesScript>();
		}
		
		// Update is called once per frame
		void Update () {
			switch(_cameraSlidesScript.ActualSlide())
			{
				case 3:
				if(Input.GetKeyUp(KeyCode.P))
				{
					if(Time.timeScale == 0)
					{
						Time.timeScale = 1;
					}
					else
					{
						Time.timeScale =0;						
					}
				}	
				break;

			}
		}
	}
}

