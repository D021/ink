using UnityEngine;

public class OneShotParticle : MonoBehaviour
{
    void Start()
    {
        if (gameObject.particleSystem)
            Destroy(gameObject, gameObject.particleSystem.duration + gameObject.particleSystem.startLifetime);
    }
}
