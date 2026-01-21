using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // For testing 1 hit = dead

    private bool isDead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        // Did we hit a red bullet?
        if (collision.CompareTag("EnemyBullet"))
        {
            Die();
        }

        // Did we crash into the boss body?
        else if (collision.CompareTag("Boss"))
        {
            Die();
        }
    }

    private void Die()
    {

        isDead = true;
        Debug.Log("Game Over");

        // 1. Pause the game / stop time
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isDead && Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
