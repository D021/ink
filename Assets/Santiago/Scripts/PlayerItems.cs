using UnityEngine;
using System.Collections;

public class PlayerItems : MonoBehaviour {

	int item1Count=0;
	int item2Count=0;
	public int inklevel=50;
	public int inklevelup=5;

	public int inkCostRush=5;
	public int inkCostFly=10;
	public int inkCostStop=10;

	
	// Update is called once per frame
	void Update () {
	
	}
	//int itemtype
	//0 : ink
	//1: shields
	//2: ???
	public void addItem(int itemType)
	{
		switch (itemType) {
		case 0:
			inklevel=inklevel+inklevelup;
			GameObject.Find("GUIText_Ink").GetComponent<GUIText>().text=inklevel.ToString();
			break;
		
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

	public void usingInk(string action)
	{
		switch (action) {
				
		case "Rush":
			inklevel=inklevel-inkCostRush;
			GameObject.Find("GUIText_Ink").GetComponent<GUIText>().text=inklevel.ToString();
			break;
		default:
			break;
		
		}
		
	}
}
