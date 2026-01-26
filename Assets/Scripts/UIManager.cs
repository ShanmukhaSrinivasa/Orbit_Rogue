using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI creditsText;

    [Header("Boss UI")]
    public Slider bossHealthSlider;  // Drag your UI slider here
    public GameObject bossHealthPanel;      // Optional: To hide the bar between rounds

    private int currentCredits = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(int health)
    {
        healthText.text = "Health- " + health;
    }

    public void UpdateRound(int round)
    {
        roundText.text = "Round- " + round;
    }

    public void AddCredits(int points)
    {
        currentCredits += points;
        creditsText.text = "Credits- " + currentCredits;
    }

    public int GetCredits()
    {
        return currentCredits;
    }

    public void SpendCredits(int amount)
    {
        currentCredits -= amount;
        UpdateCreditsText();
    }

    public void UpdateCreditsText()
    {
        creditsText.text = "Credits: " + currentCredits;
    }

    public void InitBossHealthBar(int maxHealth)
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(true); // Show bar
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = 0;
        }
    }

    public void UpdateBossHealth(int damageDealt)
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = damageDealt;
        }
    }

    public void HideBossHealthBar()
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
    }
}
