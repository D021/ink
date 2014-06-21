using UnityEngine;
using System.Collections;

/*Script para seguir al personaje con un cierto margen*/


public class CameraFollowScript : MonoBehaviour {
	
	public float xMargen;
	public float yMargen;
	public float yMargenUp;
	public float yMargenDown;
	public float yMin;
	public float XsmoothValue;
	public float YsmoothValueUp;
	public float YsmoothValueDown;
	private float yVelocity;
	private float xVelocity;

	bool goingDown = false;

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

	void MargenY(){
		//Retorna verdadero si la diferencia entre las posiciones es mayor al margen
		//Eso significaria que es hora de movernos!
		float diference = transform.position.y - player.transform.position.y;
		if (diference < yMargen)
			goingDown = false;
		else
			goingDown = true;
	}
	

	public void Follow(){
		float targetX;
		float targetY;
//		if(MargenX()) //Hacemos un cambio suave o "smooth" entre la posicion actual y la del personaje
//		targetX = Mathf.Lerp(transform.position.x, player.transform.position.x + xMargen, Time.deltaTime * XsmoothValue);
		targetX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x + xMargen, ref xVelocity, Time.smoothDeltaTime * XsmoothValue);

//		else //De lo contrario dejamos la camara en su posicion actual
//			targetX = transform.position.x;
		MargenY ();
		if(!goingDown)
			targetY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + yMargenUp, ref yVelocity, YsmoothValueUp);
		else
			targetY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + yMargenDown, ref yVelocity,YsmoothValueDown);

//		if(!goingDown)
//			targetY = Mathf.Lerp(transform.position.y, player.transform.position.y + yMargenUp, Time.smoothDeltaTime * YsmoothValueUp);
//		else
//			targetY = Mathf.Lerp(transform.position.y, player.transform.position.y + yMargenDown, Time.smoothDeltaTime * YsmoothValueDown);

		//Limitamos el valor de la camara en el eje y para que no baje demasiado
		//targetY = Mathf.Clamp(targetY,yMin,100);
		//Modificamos la posicion de la camara con los nuevos (o mismos) valores
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
	
}
