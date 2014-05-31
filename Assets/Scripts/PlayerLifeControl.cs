using UnityEngine;
using System.Collections;

public class PlayerLifeControl : MonoBehaviour {

	public bool attacking=false;
	GameObject enemy;

	void Awake()
	{	//0 is de default layer, while 10 is the enemies layer
		Physics2D.IgnoreLayerCollision(0,10);
	}

	public void enemyHit(GameObject enemy1)
	{
		this.enemy = enemy1;
		GetComponent<CharacterControllerRunner>().receiving_damage=true;
	}


}

