using System.Collections;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;     // Drag BossBullet here
    public float baseFireRate = 2f;        // Seconds between shots
    public float bulletSpeed = 4f;

    [Header("Nova Bullet Pattern Settings")]
    public int novaBulletCount = 8;   // Number of bullets in nova pattern
    public int novaWaves = 3;
    public float novaSpinSpeed = 10f;
    private float cureentSpin = 0f;

    [Header("Arc/Half-Circle Pattern")]
    public int arcBulletCount = 5;
    public int arcWaves = 2;
    public float arcAngle = 60f;      // Total angle of the arc

    private Transform playerTransform;

    void Start()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            playerTransform = player.transform;
        }

        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            int round = GameManager.Instance.roundNumber;

            if (round <= 2)
            {
                yield return StartCoroutine(FireArcRoutine());
            }
            else if(round <= 6)
            {
                if (Random.value > 0.5)
                {
                    yield return StartCoroutine(FireNovaRoutine());
                }
                else
                {
                    yield return StartCoroutine(FireArcRoutine());
                }
            }
            else
            {
                yield return StartCoroutine(FireNovaRoutine());

                yield return new WaitForSeconds(0.5f);

                yield return StartCoroutine(FireArcRoutine());
            }

            float cooldown = Mathf.Max(0.5f, baseFireRate - (round * 0.1f));
            yield return new WaitForSeconds(cooldown);
        }
    }

    private IEnumerator FireNovaRoutine()
    {
        for (int w = 0; w < novaWaves; w++)
        {
            float angleStep = 360f / novaBulletCount;
            cureentSpin += novaSpinSpeed;

            for (int i = 0; i < novaBulletCount; i++)
            {
                float angle = (i * angleStep) + cureentSpin;
                SpawnBullet(angle);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FireArcRoutine()
    {
        if (playerTransform == null)
        {
            yield break;
        }

        for(int w = 0; w < arcWaves; w++)
        {
            if (playerTransform == null)
            {
                break;
            }

            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float centreAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            float StartAngle = centreAngle - (arcAngle / 2f);
            float angleStep = arcAngle / (arcBulletCount - 1);

            for (int i = 0; i < arcBulletCount; i++)
            {
                float bulletAngle = StartAngle + (i * angleStep);
                SpawnBullet(bulletAngle);
            }

            yield return new WaitForSeconds(0.3f);
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
