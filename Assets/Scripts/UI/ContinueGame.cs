using UnityEngine;
using System.Collections;

public class ContinueGame : MonoBehaviour {
	// Use this for initialization
	public GameObject PauseMenu;

	void OnClick(){
		PauseMenu.SetActive (false);
		Time.timeScale=1;	
	}
}