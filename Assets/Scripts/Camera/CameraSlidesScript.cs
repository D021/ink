using UnityEngine;
using System.Collections;

public class CameraSlidesScript : MonoBehaviour {
	public Transform[] points;
	private int actualPoint = 0;
	public float smoothMoveValue;
	public float smoothRotateValue;

	private float xTarget;
	private float yTarget;
	private float zTarget;

	private bool moving;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.RightArrow) && (actualPoint+1) < points.Length)
		{
			moving = true;
			actualPoint++;
		}
		else if(Input.GetKeyUp(KeyCode.LeftArrow) && actualPoint > 0)
		{
			moving = true;
			actualPoint--;
		}


			

		if(moving)
		{
			MoveCamera();
		}

		print(moving);
	}

	void MoveCamera()
	{
		xTarget = Mathf.SmoothStep(this.transform.position.x, points[actualPoint].position.x, smoothMoveValue);
		yTarget = Mathf.SmoothStep(this.transform.position.y, points[actualPoint].position.y, smoothMoveValue);
		zTarget = Mathf.SmoothStep(this.transform.position.z, points[actualPoint].position.z, smoothMoveValue);

		this.transform.position = new Vector3(xTarget,yTarget,zTarget);
		transform.rotation = Quaternion.Slerp(this.transform.rotation,points[actualPoint].rotation, smoothRotateValue);


		if(Mathf.Abs(transform.position.x - points[actualPoint].position.x) < 0.1f)
			moving = false;
	}

	public int ActualSlide()
	{
		return actualPoint;
	}
}
