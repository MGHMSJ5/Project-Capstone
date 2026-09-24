using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class PlanetVegetationGPU : MonoBehaviour
{
    [Serializable]
    public class VegetationInstance
    {
        public Matrix4x4 matrix;
        public int sourceIndex;
    }

    [Serializable]
    public class DrawBatch
    {
        public Mesh mesh;
        public Material material;
        public int subMeshIndex;
        public List<Matrix4x4> matrices = new List<Matrix4x4>();
    }


    // ============================================================
    // PLANET
    // ============================================================

    [Header("Planet")]
    public Collider planetCollider;

    public Transform planetCenter;


    // ============================================================
    // VEGETATION
    // ============================================================

    [Header("Vegetation")]
    [Tooltip("Drag your asset-pack vegetation prefabs here.")]
    public GameObject[] prefabs;

    [Min(1)]
    public int amount = 500;


    // ============================================================
    // MASK
    // ============================================================

    [Header("Vegetation Mask")]
    [Tooltip("Black = no vegetation. White = vegetation allowed.")]
    public Texture2D vegetationMask;

    [Range(0f, 1f)]
    [Tooltip("Mask values below this are blocked.")]
    public float maskThreshold = 0.5f;

    [Tooltip("Invert the mask.")]
    public bool invertMask = false;


    // ============================================================
    // SLOPE
    // ============================================================

    [Header("Slope")]
    [Range(0f, 90f)]
    public float maxSlope = 60f;


    // ============================================================
    // SCALE
    // ============================================================

    [Header("Scale")]
    public float minScale = 0.8f;

    public float maxScale = 1.2f;


    // ============================================================
    // ROTATION
    // ============================================================

    [Header("Rotation")]
    public bool randomRotation = true;


    // ============================================================
    // PLACEMENT
    // ============================================================

    [Header("Placement")]
    public float surfaceOffset = 0.01f;

    public float rayStartDistance = 1000f;

    public float raycastDistance = 2000f;

    [Min(1)]
    public int attemptsPerObject = 10;


    // ============================================================
    // RANDOM
    // ============================================================

    [Header("Random Seed")]
    public int seed = 12345;


    // ============================================================
    // DEBUG
    // ============================================================

    [Header("Debug")]
    public bool showGizmos = false;


    // ============================================================
    // GENERATED DATA
    // ============================================================

    [SerializeField]
    private List<VegetationInstance> instances =
        new List<VegetationInstance>();


    private List<DrawBatch> drawBatches =
        new List<DrawBatch>();


    private bool batchesDirty = true;


    // Unity's DrawMeshInstanced limit
    private const int MAX_INSTANCES_PER_BATCH = 1023;


    // ============================================================
    // GENERATE
    // ============================================================

    public void Generate()
    {
        if (planetCollider == null)
        {
            Debug.LogError(
                "Planet Vegetation: Planet Collider is not assigned."
            );

            return;
        }


        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError(
                "Planet Vegetation: No vegetation prefabs assigned."
            );

            return;
        }


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


        if (planetCenter == null)
        {
            planetCenter = planetCollider.transform;
        }


        if (vegetationMask != null &&
            !vegetationMask.isReadable)
        {
            Debug.LogError(
                "Planet Vegetation: Vegetation Mask must have " +
                "'Read/Write Enabled' turned on in its import settings."
            );

            return;
        }


        // Clear previous generated data

        Clear();


        // Make sure materials can use instancing

        ValidateMaterials();


        // Random seed

        UnityEngine.Random.InitState(seed);


        int placed = 0;
        int attempts = 0;

        int maximumAttempts =
            amount *
            Mathf.Max(1, attemptsPerObject);


        while (
            placed < amount &&
            attempts < maximumAttempts
        )
        {
            attempts++;


            // ----------------------------------------------------
            // RANDOM DIRECTION
            // ----------------------------------------------------

            Vector3 direction =
                UnityEngine.Random.onUnitSphere;


            // ----------------------------------------------------
            // RAY START
            // ----------------------------------------------------

            Vector3 rayStart =
                planetCenter.position +
                direction *
                rayStartDistance;


            Ray ray =
                new Ray(
                    rayStart,
                    -direction
                );


            // ----------------------------------------------------
            // RAYCAST
            // ----------------------------------------------------

            if (
                !planetCollider.Raycast(
                    ray,
                    out RaycastHit hit,
                    raycastDistance
                )
            )
            {
                continue;
            }


            // ----------------------------------------------------
            // SURFACE NORMAL
            // ----------------------------------------------------

            Vector3 planetUp =
                (
                    hit.point -
                    planetCenter.position
                ).normalized;


            float surfaceAngle =
                Vector3.Angle(
                    hit.normal,
                    planetUp
                );


            if (surfaceAngle > maxSlope)
            {
                continue;
            }


            // ----------------------------------------------------
            // MASK
            // ----------------------------------------------------

            if (vegetationMask != null)
            {
                Vector2 uv =
                    hit.textureCoord;


                Color maskPixel =
                    vegetationMask.GetPixelBilinear(
                        uv.x,
                        uv.y
                    );


                float maskValue =
                    maskPixel.grayscale;


                bool allowed =
                    maskValue >= maskThreshold;


                if (invertMask)
                {
                    allowed = !allowed;
                }


                if (!allowed)
                {
                    continue;
                }
            }


            // ----------------------------------------------------
            // PICK PREFAB
            // ----------------------------------------------------

            int prefabIndex =
                UnityEngine.Random.Range(
                    0,
                    prefabs.Length
                );


            GameObject prefab =
                prefabs[prefabIndex];


            if (prefab == null)
            {
                continue;
            }


            // ----------------------------------------------------
            // CHECK PREFAB HAS RENDERERS
            // ----------------------------------------------------

            MeshFilter[] meshFilters =
                prefab.GetComponentsInChildren<MeshFilter>(
                    true
                );


            bool hasMesh =
                false;


            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (
                    meshFilter != null &&
                    meshFilter.sharedMesh != null
                )
                {
                    hasMesh = true;
                    break;
                }
            }


            if (!hasMesh)
            {
                continue;
            }


            // ----------------------------------------------------
            // POSITION
            // ----------------------------------------------------

            Vector3 position =
                hit.point +
                hit.normal *
                surfaceOffset;


            // ----------------------------------------------------
            // ROTATION
            // ----------------------------------------------------

            Quaternion rotation =
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal
                );


            if (randomRotation)
            {
                float angle =
                    UnityEngine.Random.Range(
                        0f,
                        360f
                    );


                rotation =
                    Quaternion.AngleAxis(
                        angle,
                        hit.normal
                    ) *
                    rotation;
            }


            // ----------------------------------------------------
            // SCALE
            // ----------------------------------------------------

            float scale =
                UnityEngine.Random.Range(
                    minScale,
                    maxScale
                );


            // ----------------------------------------------------
            // STORE INSTANCE
            // ----------------------------------------------------

            VegetationInstance instance =
                new VegetationInstance();


            instance.matrix =
                Matrix4x4.TRS(
                    position,
                    rotation,
                    Vector3.one * scale
                );


            instance.sourceIndex =
                prefabIndex;


            instances.Add(instance);


            placed++;
        }


        batchesDirty = true;


        BuildDrawBatches();


        Debug.Log(
            $"Planet Vegetation GPU: Generated {placed} instances after {attempts} attempts."
        );


        if (placed < amount)
        {
            Debug.LogWarning(
                "Planet Vegetation GPU: Could not place the requested amount. " +
                "Try increasing Max Slope, Attempts Per Object, " +
                "or make more of the mask white."
            );
        }


#if UNITY_EDITOR

        EditorUtility.SetDirty(this);

#endif
    }


    // ============================================================
    // BUILD DRAW BATCHES
    // ============================================================

    private void BuildDrawBatches()
    {
        drawBatches.Clear();


        if (instances == null ||
            instances.Count == 0)
        {
            batchesDirty = false;
            return;
        }


        for (
            int instanceIndex = 0;
            instanceIndex < instances.Count;
            instanceIndex++
        )
        {
            VegetationInstance instance =
                instances[instanceIndex];


            if (
                instance.sourceIndex < 0 ||
                instance.sourceIndex >= prefabs.Length
            )
            {
                continue;
            }


            GameObject prefab =
                prefabs[instance.sourceIndex];


            if (prefab == null)
            {
                continue;
            }


            MeshFilter[] meshFilters =
                prefab.GetComponentsInChildren<MeshFilter>(
                    true
                );


            foreach (
                MeshFilter meshFilter
                in meshFilters
            )
            {
                if (
                    meshFilter == null ||
                    meshFilter.sharedMesh == null
                )
                {
                    continue;
                }


                MeshRenderer meshRenderer =
                    meshFilter.GetComponent<MeshRenderer>();


                if (meshRenderer == null)
                {
                    continue;
                }


                Material[] materials =
                    meshRenderer.sharedMaterials;


                Mesh mesh =
                    meshFilter.sharedMesh;


                // ------------------------------------------------
                // WORLD TRANSFORM OF PREFAB PART
                // ------------------------------------------------

                Matrix4x4 localMatrix =
                    GetRelativeMatrix(
                        prefab.transform,
                        meshFilter.transform
                    );


                Matrix4x4 finalMatrix =
                    instance.matrix *
                    localMatrix;


                // ------------------------------------------------
                // MATERIALS / SUBMESHES
                // ------------------------------------------------

                int subMeshCount =
                    Mathf.Min(
                        mesh.subMeshCount,
                        materials.Length
                    );


                for (
                    int subMeshIndex = 0;
                    subMeshIndex < subMeshCount;
                    subMeshIndex++
                )
                {
                    Material material =
                        materials[subMeshIndex];


                    if (material == null)
                    {
                        continue;
                    }


                    DrawBatch batch =
                        FindOrCreateBatch(
                            mesh,
                            material,
                            subMeshIndex
                        );


                    batch.matrices.Add(
                        finalMatrix
                    );
                }
            }
        }


        batchesDirty = false;
    }


    // ============================================================
    // RELATIVE MATRIX
    // ============================================================

    private Matrix4x4 GetRelativeMatrix(
        Transform root,
        Transform child
    )
    {
        if (root == child)
        {
            return Matrix4x4.identity;
        }


        return root.worldToLocalMatrix *
               child.localToWorldMatrix;
    }


    // ============================================================
    // FIND / CREATE BATCH
    // ============================================================

    private DrawBatch FindOrCreateBatch(
        Mesh mesh,
        Material material,
        int subMeshIndex
    )
    {
        foreach (DrawBatch batch in drawBatches)
        {
            if (
                batch.mesh == mesh &&
                batch.material == material &&
                batch.subMeshIndex == subMeshIndex
            )
            {
                return batch;
            }
        }


        DrawBatch newBatch =
            new DrawBatch();


        newBatch.mesh =
            mesh;


        newBatch.material =
            material;


        newBatch.subMeshIndex =
            subMeshIndex;


        drawBatches.Add(
            newBatch
        );


        return newBatch;
    }


    // ============================================================
    // RENDER
    // ============================================================

    private void Update()
    {
        if (batchesDirty)
        {
            BuildDrawBatches();
        }


        RenderVegetation();
    }


    private void RenderVegetation()
    {
        if (
            drawBatches == null ||
            drawBatches.Count == 0
        )
        {
            return;
        }


        foreach (DrawBatch batch in drawBatches)
        {
            if (
                batch.mesh == null ||
                batch.material == null ||
                batch.matrices == null ||
                batch.matrices.Count == 0
            )
            {
                continue;
            }


            int total =
                batch.matrices.Count;


            int start =
                0;


            while (start < total)
            {
                int count =
                    Mathf.Min(
                        MAX_INSTANCES_PER_BATCH,
                        total - start
                    );


                Matrix4x4[] matrices =
                    new Matrix4x4[count];


                for (int i = 0; i < count; i++)
                {
                    matrices[i] =
                        batch.matrices[start + i];
                }


                Graphics.DrawMeshInstanced(
                    batch.mesh,
                    batch.subMeshIndex,
                    batch.material,
                    matrices
                );


                start += count;
            }
        }
    }


    // ============================================================
    // MATERIAL VALIDATION
    // ============================================================

    private void ValidateMaterials()
    {
        HashSet<Material> checkedMaterials =
            new HashSet<Material>();


        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null)
            {
                continue;
            }


            MeshRenderer[] renderers =
                prefab.GetComponentsInChildren<MeshRenderer>(
                    true
                );


            foreach (MeshRenderer renderer in renderers)
            {
                foreach (
                    Material material
                    in renderer.sharedMaterials
                )
                {
                    if (
                        material == null ||
                        checkedMaterials.Contains(material)
                    )
                    {
                        continue;
                    }


                    checkedMaterials.Add(
                        material
                    );


                    if (!material.enableInstancing)
                    {
                        Debug.LogWarning(
                            $"Planet Vegetation GPU: " +
                            $"Material '{material.name}' does not have GPU Instancing enabled."
                        );
                    }
                }
            }
        }
    }


    // ============================================================
    // CLEAR
    // ============================================================

    public void Clear()
    {
        instances.Clear();

        drawBatches.Clear();

        batchesDirty = true;


#if UNITY_EDITOR

        EditorUtility.SetDirty(this);

#endif
    }


    // ============================================================
    // GIZMOS
    // ============================================================

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
        {
            return;
        }


        if (instances == null)
        {
            return;
        }


        Gizmos.matrix =
            Matrix4x4.identity;


        foreach (
            VegetationInstance instance
            in instances
        )
        {
            Vector3 position =
                instance.matrix.GetColumn(3);


            Gizmos.DrawWireSphere(
                position,
                0.05f
            );
        }
    }
}


// ================================================================
// CUSTOM INSPECTOR
// ================================================================

#if UNITY_EDITOR

[CustomEditor(typeof(PlanetVegetationGPU))]
public class PlanetVegetationGPUEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        GUILayout.Space(15);


        PlanetVegetationGPU vegetation =
            (PlanetVegetationGPU)target;


        // --------------------------------------------------------
        // GENERATE
        // --------------------------------------------------------

        GUI.backgroundColor =
            Color.green;


        if (
            GUILayout.Button(
                "GENERATE VEGETATION",
                GUILayout.Height(40)
            )
        )
        {
            Undo.RecordObject(
                vegetation,
                "Generate GPU Vegetation"
            );


            vegetation.Generate();
        }


        // --------------------------------------------------------
        // CLEAR
        // --------------------------------------------------------

        GUI.backgroundColor =
            Color.red;


        if (
            GUILayout.Button(
                "CLEAR VEGETATION",
                GUILayout.Height(35)
            )
        )
        {
            Undo.RecordObject(
                vegetation,
                "Clear GPU Vegetation"
            );


            vegetation.Clear();
        }


        GUI.backgroundColor =
            Color.white;
    }
}

#endif