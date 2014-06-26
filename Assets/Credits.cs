using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	public float target;
	public float time;
	public string level;
	private float yVelocity;
	// Update is called once per frame
	void Update () {

		float targetY = Mathf.SmoothDamp(transform.position.y, target, ref yVelocity, time);
		transform.position = new Vector3(transform.position.x,targetY,transform.position.z);

		if(Mathf.Abs(transform.position.y - target) < 0.5f)
		Application.LoadLevel(level);
	}

}
