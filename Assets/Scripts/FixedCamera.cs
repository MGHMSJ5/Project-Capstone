using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Transform player;         // Assign your player here
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Camera offset from player
    public bool lockY = true;        // Lock vertical movement?

    private Vector3 initialPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
            enabled = false;
            return;
        }

        // Save the initial camera position relative to player
        initialPosition = player.position + offset;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        if (lockY)
        {
            targetPosition.y = initialPosition.y; // Keep Y constant
        }

        transform.position = targetPosition;

        // Optional: Always face the player from the side
        transform.rotation = Quaternion.Euler(15f, 0f, 0f); // Customize angle as needed
    }
}
