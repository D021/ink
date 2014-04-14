using UnityEngine;
using System.Collections;

public class Item_pickup : MonoBehaviour {


	public AudioClip pickupClip;		// Sound for pickup.
	public int itemType;
	
	//Usaremos animacion para el item?
	//private Animator anim;				// Reference to the animator component.
	// Use this for initialization
	void Awake () {
		// Setting up the reference.
		//Si usaramos animacion
		//anim = transform.root.GetComponent<Animator>();
	
	}
	

	void OnTriggerEnter2D (Collider2D other) {

		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// ... play the pickup sound effect.
			//Sonido para coger items...
			//AudioSource.PlayClipAtPoint(pickupClip, transform.position);
			
			// Increase the number of bombs the player has.
			//Incrementar la cuenta de items del jugador

			other.GetComponent<PlayerItems>().addItem(itemType);
			
			// Destroy the item.
			Destroy(transform.gameObject);
		}
	
	}
}
