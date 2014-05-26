using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour {
	// Use this for initialization
	public GameObject PauseMenu;
	public GameObject RestartMenu;
	
	void OnClick(){
		PauseMenu.SetActive (false);
		RestartMenu.SetActive (false);
		Application.LoadLevel(Application.loadedLevelName);		
		Debug.Log ("MIKU");
		Time.timeScale=1;
	}
}