using UnityEngine;
using System.Collections;

public class FallTriggerScript : MonoBehaviour {
	
	public GameObject restartMenu;
	void OnTriggerEnter2D (Collider2D other) {
		// If the player enters the trigger zone...
		if (other.tag == "Player") {
			restartMenu.SetActive (true);
			Time.timeScale=0;
		}
	}
}
