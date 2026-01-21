using System.Collections;
using UnityEngine;

public class BossHazardSector : MonoBehaviour
{
    [Header("Hazard Timing")]
    public float activeDuration = 2f;   // How long they stay visible
    public float cooldownDuration = 3f; // How long they stay hidden
    public float warningDuration = 1f;  // Blinking before damage starts

    [Header("Hazard Settings")]
    public float rotationSpeed = 30f;
    public bool isStatic = false;
    public float sectorAngle = 45f; // width of each triangle angle

    [Header("References")]
    public GameObject visualWedgePrefab; // A Triangle  sprite to show the danger zone

    private float currentRotation = 0f;
    private bool isHazardActive = false;
    private Transform playerTransform;
    private GameObject[] wedges = new GameObject[2];

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Spawn visual indicators if you have them
        SpawnVisuals();

        StartCoroutine(HazardCycle());
    }


    void Update()
    {
        if (!isStatic)
        {
            // 1. Rotate the danger Zone
            currentRotation += rotationSpeed * Time.deltaTime;
            currentRotation %= 360f;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }

        if (isHazardActive)
        {
            // 2. Check if player is in the "Danger Angle"
            CheckPlayerCollision();
        }

    }

    IEnumerator HazardCycle()
    {
        while (true)
        {
            // 1. Hidden State
            SetWedgesActive(false);
            isHazardActive = false;
            yield return new WaitForSeconds(cooldownDuration);

            // 2. Warning State (Blinkking)
            float elapsed = 0;
            while (elapsed < warningDuration)
            {
                SetWedgesActive(!wedges[0].activeSelf);
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }

            // 3. Active State (Dealing damage)
            SetWedgesActive(true);
            isHazardActive = true;

            // andomly decide of this specific attack is static or rotating
            isStatic = (Random.value > 0.5f);

            yield return new WaitForSeconds(activeDuration);
        }
    }

    void SetWedgesActive(bool state)
    {
        foreach (var wedge in wedges)
        {
            if (wedge != null)
            {
                wedge.SetActive(state);
            }
        }
    }

    private void CheckPlayerCollision()
    {
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy)
        {
            return;
        }

        // Calculate angle from boss to player
        Vector3 dir = playerTransform.position - transform.position;
        float angleToPlayer = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angleToPlayer < 0)
        {
            angleToPlayer += 360f;
        }

        float actualRotation = transform.eulerAngles.z;

        // Check against the two opposite triangles
        // Traingle 1: currentRotation
        // Triangle 2: currentRotation + 180
        if (IsAngleInSector(angleToPlayer, actualRotation) || IsAngleInSector(angleToPlayer, (actualRotation+ 180f) % 360f))
        {
            PlayerHealth ph = playerTransform.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage();
                Debug.Log("Player caught in Hazard Sector!");
            }
        }
    }

    bool IsAngleInSector(float playerAngle, float sectorCenter)
    {
        float diff = Mathf.Abs(Mathf.DeltaAngle(playerAngle, sectorCenter));
        return diff < (sectorAngle / 2f);
    }

    private void SpawnVisuals()
    {
        for (int i = 0; i < 2; i++)
        {
            wedges[i] = Instantiate(visualWedgePrefab, transform.position, Quaternion.identity);
            wedges[i].transform.SetParent(this.transform);
            wedges[i].transform.localRotation = Quaternion.Euler(0, 0, i * 180f);
        }

    }
}