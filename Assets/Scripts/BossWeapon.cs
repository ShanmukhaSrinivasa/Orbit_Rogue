using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;     // Drag BossBullet here
    public int bulletCount = 8;         // Number of bullets to shoot in a circle
    public float fireRate = 2f;        // Seconds between shots
    public float bulletSpeed = 4f;

    private float nextFireTime;

    void Start()
    {
        // Don't shoot immediately on spawn, give player 1 second time to breathe
        nextFireTime = Time.time + 1f;
    }


    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootNova();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootNova()
    {
        // Math : 360 degrees divided by number of bullets
        float angleStep = 360 / bulletCount;
        float startAngle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // 1. Calculate Rotation for this specific bullet
            float currentAngle = startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

            // 2. Spawn it
            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

            // 3. Set Speed
            BossProjectile bp = bullet.GetComponent<BossProjectile>();
            if (bp != null)
            {
                bp.speed = bulletSpeed;
            }
        }
    }
}
