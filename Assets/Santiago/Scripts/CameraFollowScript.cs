using UnityEngine;
using System.Collections;

/*Script para seguir al personaje con un cierto margen*/


public class CameraFollowScript : MonoBehaviour {
	
	public float xMargen;
	public float yMargen;
	public float yMin;
	public float smoothValue;

	public GameObject player;

	void Update(){
		Follow();
	}

	bool MargenX(){
		//Retorna verdadero si la diferencia entre las posiciones es mayor al margen y se mueve hacia la derecha
		//Eso significaria que es hora de movernos!
		float diferencia = transform.position.x - player.transform.position.x;
		return Mathf.Abs(diferencia) > xMargen;
	}

	bool MargenY(){
		//Retorna verdadero si la diferencia entre las posiciones es mayor al margen
		//Eso significaria que es hora de movernos!
		return Mathf.Abs(transform.position.y - player.transform.position.y) > yMargen;
	}
	

	public void Follow(){
		float targetX;
		float targetY;
//		if(MargenX()) //Hacemos un cambio suave o "smooth" entre la posicion actual y la del personaje
		targetX = Mathf.Lerp(transform.position.x, player.transform.position.x + xMargen, Time.deltaTime * smoothValue);
//		else //De lo contrario dejamos la camara en su posicion actual
//			targetX = transform.position.x;
		
//		if(MargenY())
			targetY = Mathf.Lerp(transform.position.y, player.transform.position.y + yMargen, Time.deltaTime * smoothValue);
//		else
//			targetY = transform.position.y;
	
		//Limitamos el valor de la camara en el eje y para que no baje demasiado
		//targetY = Mathf.Clamp(targetY,yMin,100);
		//Modificamos la posicion de la camara con los nuevos (o mismos) valores
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
	
}
