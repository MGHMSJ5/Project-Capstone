using UnityEngine;

public class PlayerPulse : MonoBehaviour
{
    public float pulseRange = 5f; // Range of the pulse field (circle around player)
    public float pulseDuration = 0.5f; // How long it takes for pulse to trigger.
    public LayerMask panelLayer; // Layer mask for panels (filters out non-panel objects, for interactions).

    public bool hasPulseAbility = true; // Toggle for whether the player has the pulse ability or not.

    private bool isPulseActive = false;

    void Update()
    {
        // Only allows Pulse if the player has it unlocked:
        if (!hasPulseAbility)
            return;

        // Controller and keyboard support:
        if (Input.GetButtonDown("Pulse"))
        {
            ActivatePulse();
        }
    }

    void ActivatePulse()
    {
        if (isPulseActive)
            return;

        // Debug to see if it activates:
        isPulseActive = true;
        Debug.Log("Pulse activated!");

        // Interactions within a player's radius:
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, pulseRange, panelLayer);

        foreach (var hit in hitObjects)
        {
            PanelPulse panel = hit.GetComponent<PanelPulse>(); // Checks the object has the PanelPulse script attached
            if (panel != null)
            {
                panel.ActivatePlatform(); // Triggers the platform working.
            }
        }

        // Deactivate pulse after a short duration (this is more for future additions, ignore now):
        Invoke("DeactivatePulse", pulseDuration);
    }

    void DeactivatePulse()
    {
        isPulseActive = false;
        Debug.Log("Pulse deactivated.");
    }
}
