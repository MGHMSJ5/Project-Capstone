using UnityEngine;
using System.Collections;

public class PanelPulse : MonoBehaviour
{
    public enum PlatformDirection
    {
        UpDown,
        LeftRight,
        ForwardBackward
    }

    public PlatformDirection platformDirection;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public GameObject platformToMove;  // Plug in the plaform that Pulse will interact with.
    public GameObject cable; // Plug in the cable that Pulse will interact with.
    public Material activatedCableMaterial; // Color of cable (activated).
    public Material defaultCableMaterial; // Color of cable (deactivated).

    private bool isMoving = false;  // Checks if connected platform is currently moving.
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Coroutine movementCoroutine;  // Reference for pausing platform movement.

    void Start()
    {
        if (platformToMove != null)
        {
            initialPosition = platformToMove.transform.position;
            targetPosition = initialPosition;
        }
        else
        {
            Debug.LogError("Platform not assigned!");
        }

        // Sets the default material for the cable:
        if (cable != null && defaultCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = defaultCableMaterial;
        }
    }

    public void ActivatePlatform()
    {
        // If the platform is moving, this stops it!
        if (isMoving)
        {
            StopMovement();
            DeactivateCable();  // Resets cable color when the panel is deactivated.
        }
        else
        {
            StartMovement();
            ActivateCable();  // Activates cable color when the panel is activated.
        }
    }

    // Handles platform moveemnt:
    private void StartMovement()
    {
        isMoving = true;  // Marks platform as "moving"

        // Ensures the platform loops back-and-forth:
        if (platformDirection == PlatformDirection.UpDown)
        {
            movementCoroutine = StartCoroutine(MoveUpDown());
        }
        else if (platformDirection == PlatformDirection.LeftRight)
        {
            movementCoroutine = StartCoroutine(MoveLeftRight());
        }
        else if (platformDirection == PlatformDirection.ForwardBackward)
    {
        movementCoroutine = StartCoroutine(MoveForwardBackward());
    }
    }

    // Pauses the platform movement:
    private void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        isMoving = false;  // Marks the platform as currently not moving.
        Debug.Log("Platform movement paused.");
    }

    // Infinite up-down movement.
    IEnumerator MoveUpDown()
    {
        while (isMoving)  // Continues moving as long as it's active.
        {
            // This moves the platform up!
            float targetHeight = initialPosition.y + moveDistance;
            while (Mathf.Abs(platformToMove.transform.position.y - targetHeight) > 0.1f && isMoving)
            {
                platformToMove.transform.position = Vector3.MoveTowards(
                    platformToMove.transform.position,
                    new Vector3(platformToMove.transform.position.x, targetHeight, platformToMove.transform.position.z),
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Moves the platform down:
            while (Mathf.Abs(platformToMove.transform.position.y - initialPosition.y) > 0.1f && isMoving)
            {
                platformToMove.transform.position = Vector3.MoveTowards(
                    platformToMove.transform.position,
                    initialPosition,
                    moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    // Inifinte left-right movement:
    IEnumerator MoveLeftRight()
    {
        while (isMoving)
        {
            // Handles the platform going to the right:
            float targetX = initialPosition.x + moveDistance;
            while (Mathf.Abs(platformToMove.transform.position.x - targetX) > 0.1f && isMoving)
            {
                platformToMove.transform.position = Vector3.MoveTowards(
                    platformToMove.transform.position,
                    new Vector3(targetX, platformToMove.transform.position.y, platformToMove.transform.position.z),
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Left movement for the platform:
            while (Mathf.Abs(platformToMove.transform.position.x - initialPosition.x) > 0.1f && isMoving)
            {
                platformToMove.transform.position = Vector3.MoveTowards(
                    platformToMove.transform.position,
                    initialPosition,
                    moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    IEnumerator MoveForwardBackward() // Makes the platform go forwards and backwards
{
    while (isMoving)
    {
        float targetZ = initialPosition.z + moveDistance;
        while (Mathf.Abs(platformToMove.transform.position.z - targetZ) > 0.1f && isMoving)
        {
            platformToMove.transform.position = Vector3.MoveTowards(
                platformToMove.transform.position,
                new Vector3(platformToMove.transform.position.x, platformToMove.transform.position.y, targetZ),
                moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (Mathf.Abs(platformToMove.transform.position.z - initialPosition.z) > 0.1f && isMoving)
        {
            platformToMove.transform.position = Vector3.MoveTowards(
                platformToMove.transform.position,
                initialPosition,
                moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

    // Changes the cable color when the panel is activated:
    private void ActivateCable()
    {
        if (cable != null && activatedCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = activatedCableMaterial;
        }
    }

    // Reverts the cable color when the panel is deactivated:
    private void DeactivateCable()
    {
        if (cable != null && defaultCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = defaultCableMaterial;
        }
    }
}
