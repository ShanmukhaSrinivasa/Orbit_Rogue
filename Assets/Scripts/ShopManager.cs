using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    MaxHealth,
    HealFull,
    HealOne,
    Firerate,
    MoveSpeed,
    Damage,
    CritChance,
    Shield,
    MultiShot,
    LifeSteal,
    Homing
}

[System.Serializable]
public class UpgradeOption
{
    public string upgradeName;
    public UpgradeType type;
    public int cost;
    public int costIncreasePerBuy;
    [TextArea] public string description;
    public Sprite icon;
}

public class ShopManager : MonoBehaviour
{
    [Header("Configuration")]
    public List<UpgradeOption> allPossibleUpgrades; // Drag Your ideas here

    [Header("UI References")]
    public Transform cardsContainer;    // Parent object holding the 3 card containers
    public GameObject cardPrefab;       // If you want to spawn them or use existing ones
    public Button[] shopbuttons;        // Drag your 3 or 4 UI Buttons;
    public TextMeshProUGUI shopeScoreText;

    [Header("Player References")]
    public PlayerHealth playerHealth;
    public AutoShoot playerGun;
    public PlayerOrbit playerOrbit;

    [Header("Stats UI")]
    public PlayerStatsUI statsUI;

    // Internal list to track what is currently in the 3 slots
    private List<UpgradeOption> currentShopSelection = new List<UpgradeOption>();

    public void RefreshShopUI()
    {
        if (shopeScoreText != null)
        {
            shopeScoreText.text = "Funds: " + UIManager.Instance.GetCredits();
        }

        if (statsUI != null)
        {
            statsUI.UpdateStats();
        }
    }

    // Called by GameManager whenever the shop opens
    public void GenerateRandomShop()
    {
        RefreshShopUI();
        currentShopSelection.Clear();

        // 1. Create a temporary list of available upgrades
        List<UpgradeOption> pool = new List<UpgradeOption>(allPossibleUpgrades);

        // 2. Loop through our UI Buttons (slots)
        for (int i = 0; i < shopbuttons.Length; i++)
        {
            // Pick a Random upgrade from the pool
            if (pool.Count > 0)
            {
                int randomIndex = Random.Range(0, pool.Count);
                UpgradeOption selectedUpgrade = pool[randomIndex];

                // Add to selection
                currentShopSelection.Add(selectedUpgrade);

                // Remove from pool so we don't pick the exact same one twice in one shop visit
                pool.RemoveAt(randomIndex);

                // Setup the Button
                SetupButton(shopbuttons[i], selectedUpgrade, i);
            }
            else
            {
                shopbuttons[i].gameObject.SetActive(false); // No more upgrades
            }
        }
    }

    public void SetupButton(Button btn, UpgradeOption upgrade, int index)
    {
        btn.gameObject.SetActive(true);

        // Find Text componenets inside the button
        // Assuming your button has 2 text fields: Name and cost
        TextMeshProUGUI[] texts = btn.GetComponentsInChildren<TextMeshProUGUI>();

        // Simple way: Text 0 is Name/Desc, Text 1 is Cost, Adjust based on your prefab.
        if (texts.Length > 0)
        {
            texts[0].text = upgrade.upgradeName + "\n" + upgrade.description;
        }

        if (texts.Length > 1)
        {
            texts[1].text = "Cost: " + upgrade.cost;

        }

        Transform iconTransform = btn.transform.Find("Icon");
        if (iconTransform != null)
        {
            Image iconImg = iconTransform.GetComponent<Image>();
            if (iconImg != null)
            {
                if (upgrade.icon != null)
                {
                    iconImg.sprite = upgrade.icon;
                    iconImg.enabled = true;
                }
                else
                {
                    iconImg.enabled = false;
                }
            }
        }

        // Clear old listeners and add new one
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => BuyUpgrade(index));

        btn.interactable = true;
    }

    public void BuyUpgrade(int index)
    {
        UpgradeOption upgrade = currentShopSelection[index];

        if (UIManager.Instance.GetCredits() >= upgrade.cost)
        {
            UIManager.Instance.SpendCredits(upgrade.cost);
            ApplyUpgradeEffect(upgrade);

            // increase Cost (Influence)
            upgrade.cost += upgrade.costIncreasePerBuy;

            // Update UI
            RefreshShopUI();

            // Disable the button so they can't buy twice in one turn (optional)
            shopbuttons[index].interactable = false;
            shopbuttons[index].GetComponentInChildren<TextMeshProUGUI>().text = "BOUGHT";
        }
    }
    
    private void ApplyUpgradeEffect(UpgradeOption upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.MaxHealth:
                playerHealth.IncreaseMaxHealth(1);
                break;
            case UpgradeType.HealFull:
                playerHealth.HealFull();
                break;
            case UpgradeType.HealOne:
                playerHealth.Heal(1);
                break;
            case UpgradeType.Firerate:
                playerGun.fireRate += 0.5f;
                break;
            case UpgradeType.MoveSpeed:
                playerOrbit.orbitSpeed += 15f; // make player faster
                break;
            case UpgradeType.Damage:
                playerGun.damage += 1;      // Bullets deal more damage
                break;
            case UpgradeType.CritChance:
                playerGun.critChance += 10f;
                break;
            case UpgradeType.MultiShot:
                playerGun.bulletCount += 2;     // Add 2 bullets (goes from 1 -> 3 -> 5)
                break;
            case UpgradeType.LifeSteal:
                playerGun.lifeStealChance += 5f;    // Add 5% chance to heal on hit
                break;
            case UpgradeType.Homing:
                playerGun.homingStrength += 2f;     // Add steering power
                break;
        }
    }

    public void CloseShop()
    {
        if (GameManager.Instance != null)
        {
            foreach (var btn in shopbuttons)
            {
                btn.interactable = true;
            }
            GameManager.Instance.ResumeFromShop();
        }
    }
}
