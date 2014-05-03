using UnityEngine;
using System.Collections;

public class ShamanChangeSpineAnimation : MonoBehaviour {

	SkeletonAnimation skeletonAnimation;
	string actualAnimation = "Run";

	// Use this for initialization
	void Start () {
		skeletonAnimation = GetComponent<SkeletonAnimation>();

	}
	
	public void ChangeSpineAnimation(string p_animation, bool p_loop){
		if (actualAnimation != p_animation) 
		{
			skeletonAnimation.skeleton.SetToSetupPose();
			skeletonAnimation.state.SetAnimation(0,p_animation,p_loop);
			actualAnimation = p_animation;
		}

	}
}
