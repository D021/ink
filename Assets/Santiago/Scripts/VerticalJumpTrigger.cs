using UnityEngine;
using System.Collections;

public class VerticalJumpTrigger : MonoBehaviour {

	public bool rightTrigger;
	private CharacterControllerRunner _characterController;
	private bool entered =false; //If the player has already entered in the trigger, to avoid
	// Use this for initialization
	void Start () {
		_characterController = GameObject.FindWithTag("Player").GetComponent<CharacterControllerRunner>();
	}

	void Update(){

	}

	void OnTriggerStay2D(Collider2D c){
		if (c.gameObject.tag == "Player" && !entered) {
			_characterController.VerticalJumpSide(rightTrigger);
			_characterController.verticalJump = true;
			entered = true;
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if (c.gameObject.tag == "Player" && entered)
			entered = false;
	}
}
