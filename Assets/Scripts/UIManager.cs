using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI scoreText;

    [Header("Boss UI")]
    public Slider bossHealthSlider;  // Drag your UI slider here
    public GameObject bossHealthPanel;      // Optional: To hide the bar between rounds

    private int currentScore = 0;

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
        healthText.text = "Health: " + health;
    }

    public void UpdateRound(int round)
    {
        roundText.text = "Round: " + round;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: " + currentScore;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void SpendScore(int amount)
    {
        currentScore -= amount;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore;
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
