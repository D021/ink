using UnityEngine;
using System.Collections;

public class PlayerItems : MonoBehaviour {

	int item1Count=0;
	int item2Count=0;
	public float inklevel=0.0f;
	public float inklevelup=0.0f;
	public float quantityItems;

//	public int inkCostRush=5;
//	public int inkCostFly=10;
//	public int inkCostStop=10;

	
	// Update is called once per frame
	void Update () {
	
	}
	//int itemtype
	//0 : ink
	//1: shields
	//2: special item
	public void addItem(int itemType)
	{
		switch (itemType) {
		case 0:
			inklevel+=inklevelup;
			GameObject.Find("InkBar").GetComponent<UISlider>().value=inklevel;
			GameObject.Find("InkPercentage").GetComponent<UILabel>().text=(int)inklevel*100+"%";
			break;
		
		case 1:
			item1Count++;
			inklevel = (item1Count+item2Count)/quantityItems;
			GameObject.Find("InkBar").GetComponent<UISlider>().value=inklevel;
			GameObject.Find("InkPercentage").GetComponent<UILabel>().text=((int)(inklevel*100))+"%";
			GameObject.Find("CoinsCounter").GetComponent<UILabel>().text="x"+(item1Count+item2Count).ToString();
			break;
		case 2:
			item2Count++;
			inklevel = (item1Count+item2Count)/quantityItems;
			GameObject.Find("InkBar").GetComponent<UISlider>().value=inklevel;
			GameObject.Find("InkPercentage").GetComponent<UILabel>().text=((int)(inklevel*100))+"%";
			GameObject.Find("CoinsCounter").GetComponent<UILabel>().text="x"+(item1Count+item2Count).ToString();

			//GameObject.Find("GUIText_2").GetComponent<GUIText>().text=item2Count.ToString();
			//print ("cogi item especial");
			break;
		default:
			break;
		}
	}

	public void usingInk(string action)
	{
		switch (action) {
				
		case "Rush":
			//inklevel=inklevel-inkCostRush;
			//GameObject.Find("GUIText_Ink").GetComponent<GUIText>().text=inklevel.ToString();
			break;
		default:
			break;
		
		}
		
	}
}
