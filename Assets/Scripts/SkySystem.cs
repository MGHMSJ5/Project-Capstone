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

    [Tooltip("Prevents tiny numerical changes from affecting the sky.")]
    [SerializeField] private float movementThreshold = 0.0001f;

    // Surface normal from the previous frame.
    private Vector3 previousPlanetUp;

    private void Start()
    {
        if (player == null || planet == null || skyDome == null)
            return;

        previousPlanetUp =
            (player.position - planet.position).normalized;
    }

    private void LateUpdate()
    {
        if (player == null || planet == null || skyDome == null)
            return;

        // The sky follows the player.
        skyDome.position = player.position;

        if (followPlanetCurvature)
        {
            RotateSkyToPlanet();
        }
    }

    private void RotateSkyToPlanet()
    {
        // Current surface normal.
        Vector3 currentPlanetUp =
            (player.position - planet.position).normalized;

        // Ignore extremely tiny changes.
        if ((currentPlanetUp - previousPlanetUp).sqrMagnitude <
            movementThreshold * movementThreshold)
        {
            return;
        }

        // Rotate the previous surface normal into the new one.
        Quaternion deltaRotation =
            Quaternion.FromToRotation(
                previousPlanetUp,
                currentPlanetUp
            );

        // Apply the curvature rotation.
        skyDome.rotation =
            deltaRotation * skyDome.rotation;

        // ---------------------------------------------------------
        // FIX ROLL
        // ---------------------------------------------------------

        // Get the sky's current forward direction.
        Vector3 forward = skyDome.forward;

        // Flatten it onto the new planet surface.
        forward =
            Vector3.ProjectOnPlane(
                forward,
                currentPlanetUp
            );

        // If the forward direction is valid,
        // rebuild the rotation using the correct up direction.
        if (forward.sqrMagnitude > 0.000001f)
        {
            forward.Normalize();

            skyDome.rotation =
                Quaternion.LookRotation(
                    forward,
                    currentPlanetUp
                );
        }

        // Remember this frame.
        previousPlanetUp = currentPlanetUp;
    }
}