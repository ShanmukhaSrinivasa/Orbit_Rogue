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

    [Header("Minion Settings")]
    public GameObject minionPrefab;     // A small asteroid
    public int minionCount = 4;
    public float orbitDistance = 2.5f;

    [Header("Minion Scaling")]
    public int baseMinionCount = 4;
    public int baseMinionHealth = 3;

    private int calculatedMinionHealth;

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

        // --- NEW SCALING LOGIC ---
        int round = GameManager.Instance.roundNumber;

        // 1. Calculate how many "Scale Steps" we have passed (e.g., Round 5 = 1 step, Round 10 = 2 steps)
        int scaleSteps = round / 5;

        // 2. Increase Count: Add 1 minion every 5 rounds
        minionCount = baseMinionCount + scaleSteps;

        // 3. Increase Health: Add 3 HP every 5 rounds (Keeps them tough)
        calculatedMinionHealth = baseMinionHealth + (scaleSteps * 3);

        SpawnMinions();
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

        ClearActiveThreats();

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

            UIManager.Instance.AddCredits(calculateScore);
        }

        // Hide the bar since boss is dead
        UIManager.Instance.HideBossHealthBar();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.explosionSound);

        // Destroy Boss Object
        Destroy(gameObject);
    }

    public void ClearActiveThreats()
    {
        BossProjectile[] activeBullets = Object.FindObjectsByType<BossProjectile>(FindObjectsSortMode.None);

        foreach (BossProjectile bullet in activeBullets)
        {
            Destroy(bullet.gameObject);
        }
    }

    private void SpawnMinions()
    {
        if (minionPrefab == null)
        {
            return;
        }

        float angleStep = 360f / minionCount;

        for (int i = 0; i < minionCount; i++)
        {
            float angle = i * angleStep;

            float radian = angle * Mathf.Deg2Rad;
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * orbitDistance;

            GameObject minion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);

            minion.transform.SetParent(this.transform);

            // --- APPLY NEW HEALTH ---
            BossMinion minionScript = minion.GetComponent<BossMinion>();
            if (minionScript != null)
            {
                minionScript.health = calculatedMinionHealth;
            }
        }
    }
}
