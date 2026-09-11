using UnityEngine;

public class SkySystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform planet;

    [Header("Sky")]
    [SerializeField] private Transform skyDome;

    [Header("Settings")]
    [SerializeField] private bool followPlanetCurvature = true;

    private void LateUpdate()
    {
        if (player == null || planet == null || skyDome == null)
            return;

        // The sky follows the player.
        skyDome.position = player.position;

        if (followPlanetCurvature)
        {
            RotateSkyToPlanet(player.position);
        }
    }

    private void RotateSkyToPlanet(Vector3 playerPosition)
    {
        // Direction from planet center to player.
        Vector3 planetUp =
            (playerPosition - planet.position).normalized;

        // Rotate the sky so its local Y axis points away
        // from the center of the planet.
        skyDome.rotation =
            Quaternion.FromToRotation(Vector3.up, planetUp);
    }
}