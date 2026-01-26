using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI specialText;     // For Multishot....

    [Header("Player References")]
    public AutoShoot playerGun;
    public PlayerOrbit playerOrbit;
    public PlayerHealth playerHealth;

    // Call this whenever the Shop opens
    public void UpdateStats()
    {
        if (damageText != null)
        {
            damageText.text = "<b>Damage:<b> " + playerGun.damage;
        }

        if (fireRateText != null)
        {
            fireRateText.text = "Fire rate: " + playerGun.fireRate.ToString("F1") + "/s"; //F1 means 1 decimal place
        }

        if (speedText != null)
        {
            speedText.text = "Speed: " + playerOrbit.orbitSpeed;
        }

        if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth.currentHealth;
        }

        if (critText != null)
        {
            critText.text = "Crit Chance: " + playerGun.critChance;
        }

        if (specialText != null)
        {
            string special = " ";
            if (playerGun.bulletCount > 1)
            {
                special += "Multi Shot: " + playerGun.bulletCount + "x\n";
            }
            if (playerGun.lifeStealChance > 0)
            {
                special += "Vampirism: " + playerGun.lifeStealChance + "%";
            }

            specialText.text = special;
        }
    }
}
