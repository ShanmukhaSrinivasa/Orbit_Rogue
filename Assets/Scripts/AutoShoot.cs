using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab; // Drag Bullet Prefab here
    public Transform target;        // Drag BossCenter here

    [Header("Gun Stats")]
    public float fireRate = 4f; // Shots per second
    public Transform muzzlePoint; // Drag the Muzzle child object here in Inspector

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

        // 1. Spawn the Bullet at Player's Position
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);

        // 2. Rotate Bullet to face the Boss
        // Math : Get Direction vector -> Convert to Angle
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // We subtract 90 because unity Sprites "Up" is usually  90 degrees offset
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
