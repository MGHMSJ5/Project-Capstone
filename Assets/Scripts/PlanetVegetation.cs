using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetVegetation : MonoBehaviour
{
    [Header("Planet")]
    [Tooltip("The Mesh Collider of your planet.")]
    public Collider planetCollider;

    [Tooltip("Center of the planet. Usually the planet transform.")]
    public Transform planetCenter;

    [Header("Vegetation")]
    [Tooltip("Prefabs that will be randomly selected.")]
    public GameObject[] prefabs;

    [Tooltip("Number of vegetation objects to place.")]
    [Min(1)]
    public int amount = 500;

    [Header("Slope")]
    [Tooltip("Maximum slope vegetation can grow on.")]
    [Range(0f, 90f)]
    public float maxSlope = 60f;

    [Header("Scale")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [Header("Rotation")]
    [Tooltip("Give each plant a random rotation around the surface normal.")]
    public bool randomRotation = true;

    [Header("Placement")]
    [Tooltip("Small offset from the surface to prevent z-fighting.")]
    public float surfaceOffset = 0.01f;

    [Tooltip("How far outside the planet rays start.")]
    public float rayStartDistance = 1000f;

    [Tooltip("Maximum distance of each ray.")]
    public float raycastDistance = 2000f;

    [Tooltip("Number of failed attempts allowed per successful placement.")]
    [Min(1)]
    public int attemptsPerObject = 10;

    [Header("Random Seed")]
    public int seed = 12345;

    [Header("Generated Objects")]
    public Transform vegetationParent;


    // =========================================================
    // GENERATE
    // =========================================================

    public void Generate()
    {
        // Check planet collider
        if (planetCollider == null)
        {
            Debug.LogError(
                "Planet Vegetation: Planet Collider is not assigned."
            );

            return;
        }

        // Check prefabs
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError(
                "Planet Vegetation: No vegetation prefabs assigned."
            );

            return;
        }

        // Check for null prefabs
        bool hasValidPrefab = false;

        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
            {
                hasValidPrefab = true;
                break;
            }
        }

        if (!hasValidPrefab)
        {
            Debug.LogError(
                "Planet Vegetation: All prefab slots are empty."
            );

            return;
        }

        // Find planet center if one isn't assigned
        if (planetCenter == null)
        {
            planetCenter = planetCollider.transform;
        }

        // Clear old vegetation
        Clear();

        // Create vegetation parent
        GameObject parentObject = new GameObject("Vegetation");

        vegetationParent = parentObject.transform;

        vegetationParent.SetParent(transform);

        vegetationParent.localPosition = Vector3.zero;
        vegetationParent.localRotation = Quaternion.identity;
        vegetationParent.localScale = Vector3.one;


        // Set random seed
        Random.InitState(seed);


        int placed = 0;
        int attempts = 0;

        int maximumAttempts =
            amount * Mathf.Max(1, attemptsPerObject);


        Bounds bounds = planetCollider.bounds;


        // =====================================================
        // RANDOMLY SHOOT RAYS TOWARD PLANET
        // =====================================================

        while (placed < amount && attempts < maximumAttempts)
        {
            attempts++;


            // Random direction from planet center
            Vector3 direction = Random.onUnitSphere;


            // Start point outside the planet
            Vector3 rayStart =
                planetCenter.position +
                direction * rayStartDistance;


            // Shoot toward planet
            Vector3 rayDirection = -direction;


            Ray ray = new Ray(
                rayStart,
                rayDirection
            );


            // Raycast
            if (!planetCollider.Raycast(
                    ray,
                    out RaycastHit hit,
                    raycastDistance))
            {
                continue;
            }


            // =================================================
            // SLOPE CHECK
            // =================================================

            float slope =
                Vector3.Angle(
                    hit.normal,
                    -rayDirection
                );


            // The angle calculation above determines how much
            // the surface differs from the incoming ray.
            //
            // For a planet, we primarily want to reject very
            // steep surfaces relative to the planet's local up.


            Vector3 planetUp =
                (hit.point - planetCenter.position).normalized;


            float surfaceAngle =
                Vector3.Angle(
                    hit.normal,
                    planetUp
                );


            if (surfaceAngle > maxSlope)
            {
                continue;
            }


            // =================================================
            // SELECT PREFAB
            // =================================================

            GameObject prefab = null;

            // Try a few times to find a non-null prefab
            for (int i = 0; i < 10; i++)
            {
                GameObject candidate =
                    prefabs[
                        Random.Range(
                            0,
                            prefabs.Length
                        )
                    ];

                if (candidate != null)
                {
                    prefab = candidate;
                    break;
                }
            }


            if (prefab == null)
            {
                continue;
            }


            // =================================================
            // POSITION
            // =================================================

            Vector3 position =
                hit.point +
                hit.normal * surfaceOffset;


            // =================================================
            // ROTATION
            // =================================================

            // Make the prefab's Y axis follow the surface normal.
            Quaternion rotation =
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal
                );


            // Random rotation around the surface normal
            if (randomRotation)
            {
                float randomAngle =
                    Random.Range(
                        0f,
                        360f
                    );


                rotation =
                    Quaternion.AngleAxis(
                        randomAngle,
                        hit.normal
                    ) * rotation;
            }


            // =================================================
            // SCALE
            // =================================================

            float scale =
                Random.Range(
                    minScale,
                    maxScale
                );


            // =================================================
            // CREATE INSTANCE
            // =================================================

            GameObject instance =
                Instantiate(prefab);


            if (instance == null)
            {
                Debug.LogWarning(
                    "Planet Vegetation: Failed to instantiate prefab."
                );

                continue;
            }


            // Parent it
            instance.transform.SetParent(
                vegetationParent
            );


            // Position
            instance.transform.position =
                position;


            // Rotation
            instance.transform.rotation =
                rotation;


            // Scale
            instance.transform.localScale =
                Vector3.one * scale;


            placed++;
        }


        // =====================================================
        // RESULT
        // =====================================================

        Debug.Log(
            "Planet Vegetation: Generated " +
            placed +
            " objects after " +
            attempts +
            " attempts."
        );


        if (placed < amount)
        {
            Debug.LogWarning(
                "Planet Vegetation: Could not place the requested " +
                amount +
                " of objects. Try increasing Max Slope or " +
                "Attempts Per Object."
            );
        }
    }


    // =========================================================
    // CLEAR
    // =========================================================

    public void Clear()
    {
        // Look for existing vegetation parent
        if (vegetationParent == null)
        {
            Transform existing =
                transform.Find("Vegetation");


            if (existing != null)
            {
                vegetationParent =
                    existing;
            }
        }


        if (vegetationParent == null)
        {
            return;
        }


#if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            DestroyImmediate(
                vegetationParent.gameObject
            );
        }
        else
        {
            Destroy(
                vegetationParent.gameObject
            );
        }

#else

        Destroy(
            vegetationParent.gameObject
        );

#endif


        vegetationParent = null;
    }
}


// =============================================================
// CUSTOM INSPECTOR
// =============================================================

#if UNITY_EDITOR

[CustomEditor(typeof(PlanetVegetation))]
public class PlanetVegetationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        GUILayout.Space(15);


        PlanetVegetation vegetation =
            (PlanetVegetation)target;


        // =====================================================
        // GENERATE BUTTON
        // =====================================================

        GUI.backgroundColor = Color.green;


        if (GUILayout.Button(
                "GENERATE VEGETATION",
                GUILayout.Height(40)))
        {
            Undo.RegisterCompleteObjectUndo(
                vegetation,
                "Generate Vegetation"
            );


            vegetation.Generate();


            EditorUtility.SetDirty(
                vegetation
            );
        }


        // =====================================================
        // CLEAR BUTTON
        // =====================================================

        GUI.backgroundColor = Color.red;


        if (GUILayout.Button(
                "CLEAR VEGETATION",
                GUILayout.Height(35)))
        {
            Undo.RegisterCompleteObjectUndo(
                vegetation,
                "Clear Vegetation"
            );


            vegetation.Clear();


            EditorUtility.SetDirty(
                vegetation
            );
        }


        GUI.backgroundColor = Color.white;
    }
}

#endif