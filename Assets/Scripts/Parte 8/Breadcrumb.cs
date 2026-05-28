using UnityEngine;

public class Breadcrumb : MonoBehaviour
{
    // El PDF sugiere 3 segundos para que el rastro no sea eterno
    public float lifeTime = 3f;
    public float Lifespan => Lifespan;
    void Start()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) 
        {
            Destroy(gameObject, lifeTime);
        }
       
        
    }
}