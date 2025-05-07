using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Transform player;         // Player gets placed here
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Camera offset from the player
    public bool lockY = true;        // If we truly want a static camera/2D game vibes

    private Vector3 initialPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned for the 2d camera!");
            enabled = false;
            return;
        }

        // Saves the initial camera position relative to the player
        initialPosition = player.position + offset;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        if (lockY)
        {
            targetPosition.y = initialPosition.y; // Keeps Y axis constant
        }

        transform.position = targetPosition;

        transform.rotation = Quaternion.Euler(15f, 0f, 0f); // We can change this as needed.
    }
}