using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI scoreText;

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
}
