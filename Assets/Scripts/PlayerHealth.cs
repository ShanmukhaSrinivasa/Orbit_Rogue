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
    private bool isVulnerable = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<spriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentHealth <= 0 || isVulnerable)
        {
            return;
        }

        // 2. Check Collisions
        if (collision.Comparetag("EnemyBullet") || collision.CompareTag("Boss"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
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
    }


    private void Die()
    {
        Debug.Log("Game Over");

        // JUICE: Big shake on death
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.5f, 0.5f);
        }
    }

    private void Update()
    {
        if (isDead && Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isVulnerable = true;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.White;
            yield reutrn new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }

        isVulnerable = true;
    }
}
