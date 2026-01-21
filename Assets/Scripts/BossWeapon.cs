using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;     // Drag BossBullet here
    public int bulletCount = 8;         // Number of bullets to shoot in a circle
    public float fireRate = 2f;        // Seconds between shots
    public float bulletSpeed = 4f;

    [Header("Bullet Pattern Settings")]
    public int novaBulletCount = 8;   // Number of bullets in nova pattern

    [Header("Arc/Half-Circle Pattern")]
    public int arcBulletCount = 5;
    public float arcAngle = 60f;      // Total angle of the arc

    private float nextFireTime;
    private Transform playerTransform;

    void Start()
    {
        // Don't shoot immediately on spawn, give player 1 second time to breathe
        nextFireTime = Time.time + 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            playerTransform = player.transform;
        }
    }


    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            int chance = Random.Range(0, 2);

            if(chance == 0)
            {
                ShootNova();
            }
            else
            {
                ShootArcShot();
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootNova()
    {
        float angleStep = 360f / novaBulletCount;
        for (int i = 0; i < novaBulletCount; i++)
        {
            SpawnBullet(i * angleStep);
        }
    }

    private void ShootArcShot()
    {
        if(playerTransform == null)
        {
            return;
        }

        // 1. Find direction to player
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float centreAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;

        // 2. Spawn Bullets in an arc centered on the player
        float StartAngle = centreAngle - (arcAngle / 2f);
        float angleStep = arcAngle / (arcBulletCount - 1);

        for(int i=0; i < arcBulletCount; i++)
        {
            float bulletAngle = StartAngle + (i * angleStep);
            SpawnBullet(bulletAngle);
        }
    }

    private void SpawnBullet(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        BossProjectile bp = bullet.GetComponent<BossProjectile>();

        if (bp != null)
        {
            bp.speed = bulletSpeed;
        }
    }
}
