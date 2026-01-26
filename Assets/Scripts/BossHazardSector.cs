using System.Collections;
using UnityEngine;

public class BossHazardSector : MonoBehaviour
{
    [Header("Hazard Timing")]
    public float activeDuration = 2f;
    public float cooldownDuration = 3f;
    public float warningDuration = 1f;

    [Header("Hazard Settings")]
    public float rotationSpeed = 30f;
    public bool isStatic = false;

    [Header("References")]
    public GameObject visualWedgePrefab;

    private float currentRotation = 0f;
    private bool damageActive = false; // The "Safety Switch"
    private GameObject[] wedges = new GameObject[2];

    void Start()
    {
        SpawnVisuals();

        if (GameManager.Instance.roundNumber >= 5)
        {
            StartCoroutine(HazardCycle());
        }

        SetWedgesVisible(false);
    }

    void Update()
    {
        // Only handle rotation here. No math checks!
        if (!isStatic)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            currentRotation %= 360f;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
    }

    // Public function for the child wedges to check
    public bool IsDamageActive()
    {
        return damageActive;
    }

    IEnumerator HazardCycle()
    {
        while (true)
        {
            // 1. COOLDOWN (Hidden)
            damageActive = false;
            SetWedgesVisible(false);
            yield return new WaitForSeconds(cooldownDuration);

            // 2. WARNING (Blinking)
            // damageActive stays FALSE so player doesn't get hurt yet
            float elapsed = 0;
            while (elapsed < warningDuration)
            {
                // Blink on/off every 0.1s
                SetWedgesVisible(!wedges[0].activeSelf);
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }

            // 3. ATTACK (Visible + Damage ON)
            SetWedgesVisible(true);
            damageActive = true; // <--- NOW the colliders will hurt

            // Randomize movement for this attack
            isStatic = (Random.value > 0.5f);

            yield return new WaitForSeconds(activeDuration);
        }
    }

    void SetWedgesVisible(bool state)
    {
        foreach (var w in wedges) if (w != null) w.SetActive(state);
    }

    private void SpawnVisuals()
    {
        for (int i = 0; i < 2; i++)
        {
            wedges[i] = Instantiate(visualWedgePrefab, transform.position, Quaternion.identity);
            wedges[i].transform.SetParent(this.transform);

            // Reset position so they stick to the center
            wedges[i].transform.localPosition = Vector3.zero;

            // Rotate them opposite ways
            wedges[i].transform.localRotation = Quaternion.Euler(0, 0, i * 180f);
        }
    }
}