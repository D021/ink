using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	public GameObject PauseMenu;
	void OnClick(){
		if(Time.timeScale>0){
			Time.timeScale=0;
		}
		else{
			Time.timeScale=1;
		}
		PauseMenu.SetActive (!PauseMenu.activeInHierarchy);
	}
}
