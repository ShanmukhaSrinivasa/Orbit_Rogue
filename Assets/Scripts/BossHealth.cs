using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 20;
    public int currentHealth;

    [Header("Visuals")]
    public GameObject deathEffectPrefab;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    /*
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    */

    public void InitializeBoss(int hp)
    {
        maxHealth = hp;
        currentHealth = hp;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damageAmount)
    {
        // 1. Reduce Health
        currentHealth -= damageAmount;

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

        CameraShake.Instance.Shake(0.5f; 0.5f);
        
        //Optional: Spawn explosion effect
        if(deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Notify GameManager
        if(GameManager.instance != null)
        {
            GameManager.instance.OnBossDied();
        }

        // Destroy Boss Object
        Destroy(gameObject);
    }
}
