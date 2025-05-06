using UnityEngine;

public class JumpShadow : MonoBehaviour
{
    [Header("Shadow Settings")]
    public GameObject shadowPrefab;  
    public float shadowOffset = 0.05f;
    public LayerMask groundMask;
    public float maxShadowScale = 1f;  // Scales up when close to ground
    public float minShadowScale = 0.3f; // Scales down at max jump height
    public float maxDistance = 5f;     // distance used to calculate above values

    private GameObject shadowInstance;

    void Start()
    {
        if (shadowPrefab != null)
        {
            shadowInstance = Instantiate(shadowPrefab, transform);
            shadowInstance.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Shadow prefab not added!");
        }
    }

    void Update()
    {
        if (shadowInstance == null) return;

        if (IsAirborne())
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 100f, groundMask))
            {
                float distance = hit.distance;
                float t = Mathf.Clamp01(distance / maxDistance);
                float scale = Mathf.Lerp(maxShadowScale, minShadowScale, t);

                shadowInstance.SetActive(true);
                shadowInstance.transform.position = hit.point + hit.normal * shadowOffset;
                shadowInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                shadowInstance.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        else
        {
            shadowInstance.SetActive(false);
        }
    }

    bool IsAirborne()
    {
        return !Physics.Raycast(transform.position, -transform.up, 1.1f, groundMask);
    }
}
