using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    
    void Update()
    {
        // Move straight forward (Green Axis)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
