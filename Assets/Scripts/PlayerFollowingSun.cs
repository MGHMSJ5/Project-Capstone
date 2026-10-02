using UnityEngine;

public class PlayerFollowingSun : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform planet;

    [Header("Day / Night System")]
    [Tooltip("Reference to the DayNightSystem controlling the world's lighting.")]
    public DayNightSystem dayNightSystem;

    [Header("Static Level Detection")]
    [Tooltip("Collider representing the playable area of CaveLevel.")]
    public Collider caveLevelCollider;

    [Tooltip("Collider representing the playable area of FactoryLevel.")]
    public Collider factoryLevelCollider;

    [Tooltip("Rotation used while inside CaveLevel or FactoryLevel.")]
    public Vector3 staticLightRotation =
        new Vector3(45f, -30f, 0f);

    [Header("Light Position")]
    [Tooltip("How far the virtual sun is from the planet.")]
    public float lightDistance = 100f;

    [Tooltip("How far the light is offset sideways from the player.")]
    [Range(0f, 1f)]
    public float sideOffset = 0.4f;

    [Tooltip("How far the light is offset above/below the player.")]
    [Range(-1f, 1f)]
    public float verticalOffset = 0.2f;

    [Header("Movement")]
    [Tooltip("How quickly the light follows the player.")]
    public float rotationSpeed = 5f;

    private bool isInStaticLevel = false;
    private bool wasInStaticLevel = false;

    // ============================================================
    // UPDATE
    // ============================================================

    void LateUpdate()
    {
        if (player == null || planet == null)
            return;

        // ========================================================
        // CHECK LEVELS
        // ========================================================

        bool isInCave =
            IsInsideLevel(caveLevelCollider);

        bool isInFactory =
            IsInsideLevel(factoryLevelCollider);

        // Either CaveLevel OR FactoryLevel counts as
        // a static level.
        bool shouldBeStatic =
            isInCave ||
            isInFactory;

        isInStaticLevel =
            shouldBeStatic;

        // ========================================================
        // NOTIFY DAY/NIGHT SYSTEM
        // ========================================================

        // Only notify DayNightSystem when the state changes.
        if (isInStaticLevel != wasInStaticLevel)
        {
            wasInStaticLevel =
                isInStaticLevel;

            if (dayNightSystem != null)
            {
                dayNightSystem.SetInStaticLevel(
                    isInStaticLevel
                );
            }
        }

        // ========================================================
        // STATIC 3D LEVEL LIGHTING
        // ========================================================

        if (isInStaticLevel)
        {
            // CaveLevel or FactoryLevel.
            //
            // Keep the sun at the fixed rotation.
            transform.rotation =
                Quaternion.Euler(
                    staticLightRotation
                );

            return;
        }

        // ========================================================
        // PLANET LIGHTING
        // ========================================================

        // Direction from planet center toward player.
        Vector3 playerNormal =
            (player.position - planet.position).normalized;

        // Offset the sun slightly to the side/above.
        Vector3 offset =
            player.right * sideOffset +
            planet.up * verticalOffset;

        offset =
            offset.normalized;

        Vector3 sunDirection =
            (
                playerNormal +
                offset * 0.5f
            ).normalized;

        Vector3 sunPosition =
            planet.position +
            sunDirection * lightDistance;

        // Move the virtual sun.
        transform.position =
            sunPosition;

        // Point the light toward the planet.
        Quaternion targetRotation =
            Quaternion.LookRotation(
                planet.position - sunPosition
            );

        // Smoothly follow the player.
        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    // ============================================================
    // LEVEL DETECTION
    // ============================================================

    private bool IsInsideLevel(Collider levelCollider)
    {
        if (levelCollider == null)
            return false;

        return levelCollider.bounds.Contains(
            player.position
        );
    }
}