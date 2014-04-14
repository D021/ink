using UnityEngine;
using System.Collections;

public class FallTriggerScript : MonoBehaviour {
	

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		
				// If the player enters the trigger zone...
				if (other.tag == "Player") {
					Application.LoadLevel(Application.loadedLevelName);
				}
		}
}
