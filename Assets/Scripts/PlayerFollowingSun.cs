using UnityEngine;

public class PlayerFollowingSun : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform planet;

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

    void LateUpdate()
    {
        if (player == null || planet == null)
            return;

        // Direction from planet center toward player.
        Vector3 playerNormal =
            (player.position - planet.position).normalized;

        // Use the player's local directions to create an offset.
        Vector3 offset =
            player.right * sideOffset +
            planet.up * verticalOffset;

        offset = offset.normalized;

        // Move the virtual sun away from the player,
        // with the offset making the lighting less direct.
        Vector3 sunDirection =
            (playerNormal + offset * 0.5f).normalized;

        Vector3 sunPosition =
            planet.position + sunDirection * lightDistance;

        // Position the directional light.
        transform.position = sunPosition;

        // Point the light toward the planet.
        Quaternion targetRotation =
            Quaternion.LookRotation(planet.position - sunPosition);

        // Smoothly rotate toward the target.
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}