using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab; // Drag Bullet Prefab here
    public Transform target;        // Drag BossCenter here
    public Transform muzzlePoint; // Drag the Muzzle child object here in Inspector

    [Header("Gun Stats")]
    public float fireRate = 4f; // Shots per second
    public int damage = 1; // Starting Damage
    public float critChance = 0f;

    public int bulletCount = 1;         // 1 = normal, 3 = Triple shot
    public float spreadAngle = 15f;     // Angle between bullets
    public float lifeStealChance = 0f;
    public float homingStrength = 0f;

    private float nextFireTime = 0f;


    void Update()
    {

        // Check if it is Time to Shoot
        if (Time.time >= nextFireTime)
        {
            Shoot();

            //Calculate next Shot time (Current time + interval)
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    private void Shoot()
    {
        // Safety Check : If the boss is dead/destroyed, stop shooting to prevent crash
        if (target == null)
        {
            return;
        }

        // 2. Rotate Bullet to face the Boss
        // Math : Get Direction vector -> Convert to Angle
        Vector3 direction = target.position - transform.position;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // -- MULTISHOT LOOP ---
        // If bulletCount is 1, loop runs once (offset 0)
        // If bulletCount is 3, angles are: -15, 0, 15

        float startAngle = baseAngle - (spreadAngle * (bulletCount - 1) / 2f);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (i * spreadAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);

            // Calculate Critical rate
            int finalDamage = damage;
            bool isCrit = (Random.Range(0f, 100f) < critChance);
            if (isCrit)
            {
                finalDamage *= 2; // Double damage
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            Projectile project = bullet.GetComponent<Projectile>();
            if (project != null)
            {
                project.damage = finalDamage;
                project.isCrit = isCrit;
                project.lifeStealChance = lifeStealChance;
                project.homingStrength = homingStrength;
            }

            // We subtract 90 because unity Sprites "Up" is usually  90 degrees offset
            bullet.transform.rotation = Quaternion.Euler(0, 0, baseAngle - 90f);
        } 
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
