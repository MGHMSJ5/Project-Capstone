using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSilhouette : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private SkinnedMeshRenderer playerRenderer;

    [Header("Occlusion")]
    [Tooltip("Layers that can hide the player.")]
    [SerializeField] private LayerMask occluderMask;

    [Tooltip("How much of the player must be hidden before the silhouette appears.")]
    [Range(0f, 1f)]
    [SerializeField] private float occlusionThreshold = 0.6f;

    [Header("Silhouette")]
    [SerializeField]
    private Color silhouetteColor =
        new Color(0.02f, 0.01f, 0.04f, 1f);

    [Header("Fade")]
    [SerializeField] private bool smoothFade = true;

    [SerializeField] private float fadeSpeed = 12f;

    private GameObject silhouetteObject;
    private SkinnedMeshRenderer silhouetteRenderer;
    private Material silhouetteMaterial;

    private float currentAlpha;

    private static readonly int BaseColor =
        Shader.PropertyToID("_BaseColor");

    private static readonly int Color =
        Shader.PropertyToID("_Color");


    private void Awake()
    {
        Setup();
    }


    private void LateUpdate()
    {
        if (playerCamera == null ||
            playerRenderer == null ||
            silhouetteRenderer == null)
            return;

        bool occluded = IsOccluded();

        float targetAlpha = occluded ? 1f : 0f;

        if (smoothFade)
        {
            currentAlpha = Mathf.MoveTowards(
                currentAlpha,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );
        }
        else
        {
            currentAlpha = targetAlpha;
        }

        UpdateSilhouette();
    }


    // =========================================================
    // SETUP
    // =========================================================

    private void Setup()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerRenderer == null)
        {
            Debug.LogError(
                "PlayerSilhouette: No SkinnedMeshRenderer assigned.",
                this
            );

            enabled = false;
            return;
        }

        Shader shader =
            Shader.Find("Custom/PlayerSilhouette");

        if (shader == null)
        {
            Debug.LogError(
                "PlayerSilhouette: Could not find URP Unlit shader.",
                this
            );

            enabled = false;
            return;
        }

        // -----------------------------------------------------
        // Create silhouette object
        // -----------------------------------------------------

        silhouetteObject =
            new GameObject("Player Silhouette");

        silhouetteObject.transform.SetParent(
            playerRenderer.transform,
            false
        );

        // -----------------------------------------------------
        // Create renderer
        // -----------------------------------------------------

        silhouetteRenderer =
            silhouetteObject.AddComponent<SkinnedMeshRenderer>();

        // Same mesh.
        silhouetteRenderer.sharedMesh =
            playerRenderer.sharedMesh;

        // IMPORTANT:
        // Use the exact same bones as the original renderer.
        silhouetteRenderer.bones =
            playerRenderer.bones;

        silhouetteRenderer.rootBone =
            playerRenderer.rootBone;

        silhouetteRenderer.localBounds =
            playerRenderer.localBounds;

        silhouetteRenderer.updateWhenOffscreen = true;

        // -----------------------------------------------------
        // Create material
        // -----------------------------------------------------

        silhouetteMaterial =
            new Material(shader);

        silhouetteMaterial.name =
            "Player Silhouette (Runtime)";

        silhouetteMaterial.enableInstancing = true;

        SetMaterialColor(0f);

        Material[] silhouetteMaterials =
    new Material[playerRenderer.sharedMaterials.Length];

        for (int i = 0; i < silhouetteMaterials.Length; i++)
        {
            silhouetteMaterials[i] = silhouetteMaterial;
        }

        silhouetteRenderer.sharedMaterials =
            silhouetteMaterials;

        // We don't want the silhouette to cast shadows.
        silhouetteRenderer.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        silhouetteRenderer.receiveShadows = false;

        // Start invisible.
        silhouetteRenderer.enabled = false;

        currentAlpha = 0f;
    }


    // =========================================================
    // OCCLUSION
    // =========================================================

    private bool IsOccluded()
    {
        Bounds bounds =
            playerRenderer.bounds;

        Vector3 center = bounds.center;

        Vector3[] points =
        {
            // Center
            center,

            // Bottom
            new Vector3(
                center.x,
                bounds.min.y + bounds.extents.y * 0.15f,
                center.z
            ),

            // Chest
            new Vector3(
                center.x,
                Mathf.Lerp(
                    bounds.min.y,
                    bounds.max.y,
                    0.5f
                ),
                center.z
            ),

            // Head
            new Vector3(
                center.x,
                bounds.max.y - bounds.extents.y * 0.1f,
                center.z
            ),

            // Left
            center - playerCamera.transform.right *
            bounds.extents.x * 0.5f,

            // Right
            center + playerCamera.transform.right *
            bounds.extents.x * 0.5f
        };

        int blocked = 0;

        foreach (Vector3 point in points)
        {
            if (IsPointBlocked(point))
                blocked++;
        }

        float occlusion =
            (float)blocked / points.Length;

        return occlusion >= occlusionThreshold;
    }


    private bool IsPointBlocked(Vector3 point)
    {
        Vector3 origin =
            playerCamera.transform.position;

        Vector3 direction =
            point - origin;

        float distance =
            direction.magnitude;

        if (distance <= 0.01f)
            return false;

        direction.Normalize();

        return Physics.Raycast(
            origin,
            direction,
            distance,
            occluderMask,
            QueryTriggerInteraction.Ignore
        );
    }


    // =========================================================
    // SILHOUETTE
    // =========================================================

    private void UpdateSilhouette()
    {
        if (currentAlpha <= 0.001f)
        {
            silhouetteRenderer.enabled = false;
            return;
        }

        silhouetteRenderer.enabled = true;

        SetMaterialColor(currentAlpha);
    }


    private void SetMaterialColor(float alpha)
    {
        Color finalColor =
            silhouetteColor;

        finalColor.a =
            alpha;

        if (silhouetteMaterial.HasProperty(BaseColor))
        {
            silhouetteMaterial.SetColor(
                BaseColor,
                finalColor
            );
        }

        if (silhouetteMaterial.HasProperty(Color))
        {
            silhouetteMaterial.SetColor(
                Color,
                finalColor
            );
        }
    }


    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDestroy()
    {
        if (silhouetteMaterial != null)
            Destroy(silhouetteMaterial);

        if (silhouetteObject != null)
            Destroy(silhouetteObject);
    }


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (playerRenderer == null)
            return;

        Bounds bounds =
            playerRenderer.bounds;

        Gizmos.color =
            UnityEngine.Color.magenta;

        Gizmos.DrawWireCube(
            bounds.center,
            bounds.size
        );
    }

#endif
}