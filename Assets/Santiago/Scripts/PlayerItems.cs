using UnityEngine;
using System.Collections;

public class PlayerItems : MonoBehaviour {

	int item1Count=0;
	int item2Count=0;

	
	// Update is called once per frame
	void Update () {
	
	}

	public void addItem(int itemType)
	{
		switch (itemType) {
		case 1:
			item1Count++;
			GameObject.Find("GUIText_1").GetComponent<GUIText>().text=item1Count.ToString();
			break;
		case 2:
			item2Count++;
			//GameObject.Find("GUIText_2").GetComponent<GUIText>().text=item2Count.ToString();
			break;
		default:
			break;
		}
	}
}
