using UnityEngine;
using System.Collections;

public class SlidesInkActivateRino : MonoBehaviour {
	private CharacterControllerRunner _characterController;
	private ShamanChangeSpineAnimation _shamanSpineController;
	private bool rushing;
	public float AmountMove;
	float maxSpeed = 10f;				                // The fastest the player can travel in the x axis.

	// Use this for initialization
	void Start () {
		_characterController = GetComponent<CharacterControllerRunner>();
		_shamanSpineController = GetComponent<ShamanChangeSpineAnimation>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = new Vector2(AmountMove * maxSpeed, rigidbody2D.velocity.y);

	}


	void OnTriggerEnter2D(Collider2D active)
	{
		if(active.gameObject.name == "ActivateRinoTrigger" && !rushing)
		{
			StartCoroutine(inkvoking());

			_shamanSpineController.ChangeSpineAnimation("Rush", false);
			//Activate the trigger in front of the player

		}
	}

	IEnumerator inkvoking()
	{
		rushing = true;
		yield return new WaitForSeconds(0.8f);
		rushing = false;
		_shamanSpineController.ChangeSpineAnimation("Run", true);

	}
}
