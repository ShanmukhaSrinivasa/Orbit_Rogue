using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // For testing 1 hit = dead
    private int currentHealth;

    [Header("Invincibility Settings")]
    public float iFrameDuration = 1f; // How long you are safe after hit
    public int numberOfFlashes = 5;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        UIManager.Instance.UpdateHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If dead or invincible, ignore collision
        if (currentHealth <= 0 || isInvincible)
        {
            return;
        }

        // 2. Check Collisions
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Boss"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isInvincible || currentHealth <= 0)
        {
            return;
        }

        currentHealth--;

        // JUICE: Shake screen when hit
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.2f, 0.3f);
        }

        Debug.Log("Player Hit! HP: + " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // If still alive, give temporary invincibility
            StartCoroutine(InvulnerabilityRoutine());
        }

        UIManager.Instance.UpdateHealth(currentHealth);
    }


    private void Die()
    {
        Debug.Log("Game Over");

        // JUICE: Big shake on death
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.5f, 0.5f);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        gameObject.SetActive(false);
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvincible = true;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }

        isInvincible = false;
    }
}
