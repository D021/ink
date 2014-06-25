using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    public GameObject Particle;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Instantiate(Particle, new Vector3(clickPosition.x, clickPosition.y, 0), transform.rotation);
        }
    }
}
