using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f; // Destroy bullet after 3 seconds if it misses
    public int damage = 1;

    void Start()
    {
        //Safety Cleanup : Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifeTime);
    }


    void Update()
    {
        // Move "Up" relative to the bullet's own rotation
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if we hit the Boss
        if(collision.CompareTag("Boss"))
        {
            // 1. Try to get the BoosHealth component from the object we hit
            BossHealth boss = collision.GetComponent<BossHealth>();

            // 2. If it has health, damage it
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            // 3. Destroy the Bullet
            Destroy(gameObject);
        }
    }
}
