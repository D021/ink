using UnityEngine;
using System.Collections;

public class OneSideCollider : MonoBehaviour
{

    public BoxCollider2D platform;
    public bool oneWay = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        platform.enabled = !oneWay;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("TriggerStay");
        oneWay = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        oneWay = false;
    }
}
