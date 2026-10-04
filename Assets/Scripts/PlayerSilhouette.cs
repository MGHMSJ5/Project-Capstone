using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public class PlayerSilhouette : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SkinnedMeshRenderer playerRenderer;

    [SerializeField]
    private Shader silhouetteShader;

    [Header("Silhouette")]
    [SerializeField]
    private Color silhouetteColor =
        new Color(0.02f, 0.01f, 0.04f, 1f);

    private GameObject silhouetteObject;
    private SkinnedMeshRenderer silhouetteRenderer;
    private Material silhouetteMaterial;

    private static readonly int BaseColor =
        Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        Setup();
    }

    private void LateUpdate()
    {
        if (silhouetteRenderer == null)
            return;

        silhouetteRenderer.enabled = true;

        if (silhouetteMaterial != null)
        {
            silhouetteMaterial.SetColor(
                BaseColor,
                silhouetteColor
            );
        }
    }

    private void Setup()
    {
        if (playerRenderer == null)
        {
            playerRenderer =
                GetComponentInChildren<SkinnedMeshRenderer>();
        }

        if (playerRenderer == null)
        {
            Debug.LogError(
                "PlayerSilhouette: No SkinnedMeshRenderer found.",
                this
            );

            enabled = false;
            return;
        }

        if (silhouetteShader == null)
        {
            Debug.LogError(
                "PlayerSilhouette: Silhouette Shader is not assigned!",
                this
            );

            enabled = false;
            return;
        }

        silhouetteObject =
            new GameObject("Player Silhouette");

        silhouetteObject.transform.SetParent(
            playerRenderer.transform,
            false
        );

        silhouetteRenderer =
            silhouetteObject.AddComponent<SkinnedMeshRenderer>();

        silhouetteRenderer.sharedMesh =
            playerRenderer.sharedMesh;

        silhouetteRenderer.bones =
            playerRenderer.bones;

        silhouetteRenderer.rootBone =
            playerRenderer.rootBone;

        silhouetteRenderer.localBounds =
            playerRenderer.localBounds;

        silhouetteRenderer.updateWhenOffscreen = true;

        silhouetteMaterial =
            new Material(silhouetteShader);

        silhouetteMaterial.name =
            "Player Silhouette Runtime";

        silhouetteMaterial.enableInstancing = true;

        Material[] materials =
            new Material[
                playerRenderer.sharedMaterials.Length
            ];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = silhouetteMaterial;
        }

        silhouetteRenderer.sharedMaterials = materials;

        silhouetteRenderer.shadowCastingMode =
            ShadowCastingMode.Off;

        silhouetteRenderer.receiveShadows = false;

        silhouetteRenderer.enabled = true;

        silhouetteMaterial.SetColor(
            BaseColor,
            silhouetteColor
        );
    }

    private void OnDestroy()
    {
        if (silhouetteMaterial != null)
            Destroy(silhouetteMaterial);

        if (silhouetteObject != null)
            Destroy(silhouetteObject);
    }
}