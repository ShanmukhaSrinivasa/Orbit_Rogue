using UnityEngine;

public class HazardWedge : MonoBehaviour
{
    private BossHazardSector bossController;

    void Start()
    {
        // Find the main controller script in the parent
        bossController = GetComponentInParent<BossHazardSector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // DEBUG 1: Did we touch ANYTHING?
        // Debug.Log("Touched: " + collision.name); 

        if (collision.CompareTag("Player"))
        {
            // DEBUG 2: Found Player. Is Boss Controller found?
            if (bossController == null)
            {
                Debug.LogError("HazardWedge: BossHazardSector script NOT found in parent!");
                return;
            }

            // DEBUG 3: Is Damage Active?
            if (bossController.IsDamageActive())
            {
                PlayerHealth ph = collision.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    ph.TakeDamage();
                }
                else
                {
                    Debug.LogError("HazardWedge: Player has no PlayerHealth script!");
                }
            }
            else
            {
                // Uncomment to verify the 'Safety' phase works
                // Debug.Log("Hit Player, but Damage is INACTIVE (Blinking Phase)");
            }
        }
    }
}