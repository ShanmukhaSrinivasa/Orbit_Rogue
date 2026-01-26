using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BossMinion : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public int contactDamage = 1;

    [Header("orbit Settings")]
    public float rotationSpeed = 100f;

    private void Update()
    {
        if (transform.parent != null)
        {
            transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }



    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ... (Player collision check stays same) ...

        if (collision.CompareTag("PlayerBullet"))
        {
            // 1. Get the bullet script
            Projectile proj = collision.GetComponent<Projectile>();

            // 2. Determine damage (Default to 1 if script is missing)
            int dmgToTake = (proj != null) ? proj.damage : 1;

            // 3. Take actual damage
            TakeDamage(dmgToTake);

            // 4. Destroy bullet
            Destroy(collision.gameObject);
        }
    }
}
