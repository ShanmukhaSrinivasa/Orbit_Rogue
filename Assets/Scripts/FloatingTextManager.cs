using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    public GameObject textPrefab;

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

    public void ShowDamage(int damage, Vector3 position, bool isCrit)
    {
        if (textPrefab != null)
        {
            // Spawn the text slightly above the collision point
            Vector3 spawnPos = position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

            GameObject go = Instantiate(textPrefab, spawnPos, Quaternion.identity);
            FloatingText ft = go.GetComponent<FloatingText>();

            if (ft != null)
            {
                ft.Setup(damage, isCrit);
            }
        }
    }

    public void ShowHealText(int value, Vector3 position)
    {
        if (textPrefab != null)
        {
            // Spawn slightly to the right / up so it doesn't overlap the damage numbers
            Vector3 spawnPos = position + new Vector3(0.8f, 0.8f, 0);

            GameObject go = Instantiate(textPrefab, spawnPos, Quaternion.identity);
            FloatingText ft = go.GetComponent<FloatingText>();

            if (ft != null)
            {
                ft.SetupHeal(value);
            }
        }
    }
}
