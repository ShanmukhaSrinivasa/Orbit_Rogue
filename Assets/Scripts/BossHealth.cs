using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 20;
    public int currentHealth;

    [Header("Score Settings")]
    public int baseScore = 100;             // Score for Round 1
    public int scoreIncreasePerRound = 50;  // Extra Score Per round

    [Header("Visuals")]
    public GameObject deathEffectPrefab;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    

    public void InitializeBoss(int hp)
    {
        maxHealth = hp;
        currentHealth = hp;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Juice: Initialize the UI bar
        UIManager.Instance.InitBossHealthBar(maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        // 1. Reduce Health
        currentHealth -= damageAmount;

        int damageDealt = maxHealth - currentHealth;

        // JUICE: Update the UI Bar
        UIManager.Instance.UpdateBossHealth(damageDealt);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HitStop(0.02f);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.05f, 0.1f);
        }

        // 2. Flash Color
        StartCoroutine(FlashRed());

        // 3. Check for Death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        // swap to red color
        spriteRenderer.color = flashColor;

        // wait for a split second
        yield return new WaitForSeconds(flashDuration);

        // return to normal color
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!");

        CameraShake.Instance.Shake(0.5f, 0.5f);
        
        //Optional: Spawn explosion effect
        if(deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Notify GameManager
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnBossDied();

            // --- DYNAMIC SCORE CALCULATION ---
            // Formula: Base + (Round * Bonus)
            // Example Round 1: 100 + (1 * 50) = 150
            // Example Round 5: 100 + (5 * 50) = 350
            int round = GameManager.Instance.roundNumber;
            int calculateScore = baseScore + (round * scoreIncreasePerRound);

            UIManager.Instance.AddScore(calculateScore);
        }

        // Hide the bar since boss is dead
        UIManager.Instance.HideBossHealthBar();

        // Destroy Boss Object
        Destroy(gameObject);
    }
}
