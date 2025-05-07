using UnityEngine;

public class JumpShadow : MonoBehaviour
{
    [Header("Shadow Settings")]
    public GameObject shadowPrefab;  
    public float shadowOffset = 0.05f; // How far the shadow is from the ground.
    public LayerMask groundMask; // Layer the script looks for the shadow.
    public float maxShadowScale = 1f; // Size closest to ground.
    public float minShadowScale = 0.3f; // Size farthest from the ground.
    public float maxDistance = 5f;

    private GameObject shadowInstance;

    void Start()
    {
        if (shadowPrefab != null)
        {
            shadowInstance = Instantiate(shadowPrefab, transform);
            shadowInstance.SetActive(true); // Makes drop shadow always permanent.
        }
        else
        {
            Debug.LogWarning("Shadow prefab not added!");
        }
    }

    void Update()
    {
        if (shadowInstance == null) return;

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 100f, groundMask))
        {
            bool isAirborne = !Physics.Raycast(transform.position, -transform.up, 1.1f, groundMask);

            float distance = hit.distance;
            float scale = isAirborne 
                ? Mathf.Lerp(maxShadowScale, minShadowScale, Mathf.Clamp01(distance / maxDistance)) 
                : maxShadowScale;

            shadowInstance.transform.position = hit.point + hit.normal * shadowOffset;
            shadowInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            shadowInstance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
