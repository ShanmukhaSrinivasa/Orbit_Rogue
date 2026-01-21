using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public GameObject bossPrefab;
    public PlayerOrbit playerOrbit;
    public AutoShoot playerGun;

    [Header("Game Flow")]
    public float delayBetweenRounds = 2f;
    private int roundNumber = 0;

    [Header("Difficulty Settings")]
    public int baseBossHealth = 20;     // Round 1 Health
    public int healthPerRound = 10;     // How much HP to add each Round

    [Header("Canvas Panels")]
    public CanvasGroup gameStartPanel;
    public CanvasGroup gamePanel;
    public CanvasGroup gameOverPanel;

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

    void Start()
    {
        ShowCG(gameStartPanel);
        HideCG(gamePanel);
        HideCG(gameOverPanel);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        HideCG(gameStartPanel);
        ShowCG(gamePanel);
        HideCG(gameOverPanel);

        Time.timeScale = 1f;
        StartNewRound();
    }

    public void StartNewRound()
    {
        roundNumber++;
        Debug.Log("Starting Round: " + roundNumber);

        // 1. Spawn the boss at Center
        GameObject newBoss = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);

        // Math: Round 1 = 20, Round 2 = 30, Round 3 = 40 ...
        int calculatedHealth = baseBossHealth + ((roundNumber - 1) * healthPerRound);

        // Get the script and initialize health
        BossHealth bossHealthScript = newBoss.GetComponent<BossHealth>();
        bossHealthScript.InitializeBoss(calculatedHealth);

        // Optional: Make the Boss slightly bigger every 5 rounds so it feels epic
        float scaleIncrease = 1f + (roundNumber * 0.05f); // +5% bigger each round
        newBoss.transform.localScale = new Vector3(scaleIncrease, scaleIncrease, 1f);

        // 2. Tell the player scripts about the new boss
        playerOrbit.SetCenter(newBoss.transform);
        playerGun.SetTarget(newBoss.transform);

        UIManager.Instance.UpdateRound(roundNumber);

    }

    public void OnBossDied()
    {
        // Called by BossHealth when HP hits 0
        Debug.Log("Round Cleared");

        // Wait for a few seconds before starting a new round
        StartCoroutine(WaitAndNextRound());
    }

    IEnumerator WaitAndNextRound()
    {
        yield return new WaitForSeconds(delayBetweenRounds);
        StartNewRound();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Pause the game
        ShowCG(gameOverPanel);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HitStop(float duration)
    {
        if (Time.timeScale > 0)
        {
            StartCoroutine(DoHitStop(duration));
        }
    }

    IEnumerator DoHitStop(float duration)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalScale;
    }

    public void ShowCG(CanvasGroup cg)
    {
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void HideCG(CanvasGroup cg)
    {
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
