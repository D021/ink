using UnityEngine;
using System.Collections;

public class ShamanChangeSpineAnimation : MonoBehaviour {

	SkeletonAnimation skeletonAnimation;


	// Use this for initialization
	void Start () {
		skeletonAnimation = GetComponent<SkeletonAnimation>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)){

			skeletonAnimation.skeleton.SetToSetupPose();
			skeletonAnimation.state.SetAnimation(0,"Jump",false);

		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			
			skeletonAnimation.skeleton.SetToSetupPose();
			skeletonAnimation.state.SetAnimation(0,"Run", true);
			
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			
			skeletonAnimation.skeleton.SetToSetupPose();
			skeletonAnimation.state.SetAnimation(0,"Rush", false);
			
		}
			
	}
}
