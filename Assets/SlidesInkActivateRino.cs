using UnityEngine;
using System.Collections;

public class SlidesInkActivateRino : MonoBehaviour {
	private CharacterControllerRunner _characterController;
	private ShamanChangeSpineAnimation _shamanSpineController;
	private bool rushing;
	public float AmountMove;
	float maxSpeed = 10f;				                // The fastest the player can travel in the x axis.
	public GameObject rinoFX;
	private GameObject rinoInstance;
	public Transform rinoPosInstance;
	public GameObject textPower;
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
		rinoInstance = (GameObject)Instantiate(rinoFX,rinoPosInstance.position,Quaternion.identity);
		rinoInstance.transform.parent = transform;
		GameObject text = (GameObject)Instantiate(textPower);
		Destroy(text,1f);
		yield return new WaitForSeconds(0.8f);
		rushing = false;
		_shamanSpineController.ChangeSpineAnimation("Run", true);
		Destroy(rinoInstance, 0.4f);

	}
}
