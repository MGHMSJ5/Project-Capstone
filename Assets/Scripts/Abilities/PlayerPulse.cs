using UnityEngine;

public class PlayerPulse : MonoBehaviour
{
    public float pulseRange = 5f; // Range of the pulse field (circle around player)
    public float pulseDuration = 0.5f; // How long it takes for pulse to trigger.
    public LayerMask panelLayer; // Layer mask for panels (filters out non-panel objects, for interactions).

    public bool hasPulseAbility = true; // Toggle for whether the player has the pulse ability or not.

    private bool isPulseActive = false;

    public GameObject pulseVisualPrefab; // For visual indicator

    private Transform _carryPoint;
    private PlayerController _playerController;

    public bool IsPulseActive => isPulseActive;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _carryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
    }

    void Update()
    {
        if (!hasPulseAbility)
            return;

        // Only allow pulse if player is grounded
        if (Input.GetButtonDown("Pulse") && _playerController.Grounded)
        {
            ActivatePulse();
        }
    }

    void ActivatePulse()
    {
        if (isPulseActive)
            return;

        isPulseActive = true;
        Debug.Log("Pulse activated!");

        InterruptCarrying();

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, pulseRange, panelLayer);

        foreach (var hit in hitObjects)
        {
            PanelPulse panel = hit.GetComponent<PanelPulse>();
            if (panel != null)
            {
                panel.ActivatePlatform();
                continue;
            }

            DoorPulse door = hit.GetComponent<DoorPulse>();
            if (door != null)
            {
                door.ActivateDoor();
            }
        }

        if (pulseVisualPrefab != null)
        {
            GameObject visual = Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);
            visual.transform.localScale = Vector3.zero;
            StartCoroutine(AnimatePulseVisual(visual));
        }

        Invoke("DeactivatePulse", pulseDuration);
    }

    void DeactivatePulse()
    {
        isPulseActive = false;
        Debug.Log("Pulse deactivated.");
    }

    private void InterruptCarrying()
    {
        if (_carryPoint != null && _carryPoint.childCount > 0)
        {
            CarryObjectEXAMPLE carryObjectEXAMPLE = _carryPoint.GetChild(0).GetChild(0).GetComponent<CarryObjectEXAMPLE>();
            if (carryObjectEXAMPLE != null)
            {
                carryObjectEXAMPLE.Interrupt();
            }
        }
    }

    private System.Collections.IEnumerator AnimatePulseVisual(GameObject visual)
    {
        float time = 0f;
        float scale = pulseRange * 2f; // diameter, not radius

        Material mat = visual.GetComponent<Renderer>()?.material;

        while (time < pulseDuration)
        {
            float t = time / pulseDuration;
            visual.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(scale, scale, scale), t);

            if (mat != null)
            {
                Color c = mat.color;
                c.a = Mathf.Lerp(0.5f, 0f, t);
                mat.color = c;
            }

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(visual);
    }
}
