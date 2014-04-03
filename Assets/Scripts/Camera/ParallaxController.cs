using UnityEngine;

namespace com.inkrunner.camera
{	
	/// <summary>
	/// Controls the scrolling of the layers
	/// </summary>
	public class ParallaxController : MonoBehaviour {
		
		public float Speed = 0;
		
		void Update () 
		{
			renderer.material.mainTextureOffset = new Vector2 ((Time.time * Speed) % 1, 0);
		}
	}
}
