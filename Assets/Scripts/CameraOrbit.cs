using UnityEngine;

public class CameraOrbitWithOffset : MonoBehaviour
{
    [Tooltip("The object to orbit around")]
    public Transform target;

    [Tooltip("Speed of the orbit in degrees per second")]
    public float orbitSpeed = 10f;

    [Tooltip("Distance from the target on the XZ plane")]
    public float distance = 10f;

    [Header("Base Camera Position")]
    public float baseX = 0f;
    public float baseY = 75f;
    public float baseZ = 0f;

    private float currentAngle = 0f;

    void Update()
    {
        if (target == null) return;

        // Updates angle of planet
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // radius
        float radians = currentAngle * Mathf.Deg2Rad;

        // Orbit position
        float offsetX = Mathf.Cos(radians) * distance;
        float offsetZ = Mathf.Sin(radians) * distance;

        Vector3 orbitCenter = target.position;

        Vector3 newPosition = new Vector3(baseX + offsetX, baseY, baseZ + offsetZ);

        transform.position = newPosition;
        transform.LookAt(orbitCenter);
    }
}