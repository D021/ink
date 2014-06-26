using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	public string level;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			Application.LoadLevel(level);
		}
	}
}
