using UnityEngine;
using System.Collections;

public class LoopSlidesTrigger : MonoBehaviour {
	public Transform initialPoint;
	public InputManagerMouse shaman;
	// Use this for initialization

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.P))
		{
			if(shaman)
			{
				if(shaman.AmountMove == 0)
				{
					shaman.AmountMove = 1;
				}
				
				else
				{
					shaman.AmountMove = 0;
				}
			}

		}
			
	}

	void OnTriggerEnter2D(Collider2D player)
	{
		player.gameObject.transform.position = initialPoint.transform.position;
	}
}
