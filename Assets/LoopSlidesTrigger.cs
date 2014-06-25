using UnityEngine;
using System.Collections;

public class LoopSlidesTrigger : MonoBehaviour {
	public Transform initialPoint;
	public InputManagerMouse shaman;
	public SlidesInkActivateRino rino;

	// Use this for initialization

	public void StartDemoShaman()
	{
		if(shaman)
		{
			shaman.AmountMove = 0.7f;

		}
		
	}



	public void StopDemoShaman()
	{
		if(shaman)
			shaman.AmountMove = 0;


	}
	


	void OnTriggerEnter2D(Collider2D player)
	{
		player.gameObject.transform.position = initialPoint.transform.position;
	}
}
