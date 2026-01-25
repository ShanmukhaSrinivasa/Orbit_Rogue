using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f; // Destroy bullet after 3 seconds if it misses
    public int damage = 1;
    public bool isCrit = false;

    public float lifeStealChance = 0f;  // 0 to 100
    public float homingStrength = 0f;   // 0 to 5
    private Transform target;

    void Start()
    {
        //Safety Cleanup : Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifeTime);

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            target = boss.transform;
        }
    }


    void Update()
    {
        // --- Homing Logic ---
        if (homingStrength > 0 && target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * homingStrength);
        }

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

                if (FloatingTextManager.Instance != null)
                {
                    FloatingTextManager.Instance.ShowDamage(damage, transform.position, isCrit);
                }

                // --- VAMPIRE LOGIC ---
                // If we roll a success on life Steal
                if (lifeStealChance > 0 && Random.Range(0f, 100f) < lifeStealChance)
                {
                    // Find player and Heal
                    PlayerHealth player = Object.FindFirstObjectByType<PlayerHealth>();
                    if (player != null)
                    {
                        player.Heal(1);

                        if (FloatingTextManager.Instance != null)
                        {
                            FloatingTextManager.Instance.ShowHealText(1, transform.position);
                        }
                    }
                }
            }

            // 3. Destroy the Bullet
            Destroy(gameObject);
        }
    }
}
