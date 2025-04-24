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
    public GameObject platformToMove;  // Plug in the platform that Pulse will interact with.
    public GameObject cable; // Plug in the cable that Pulse will interact with.
    public Material activatedCableMaterial; // Color of cable (activated).
    public Material defaultCableMaterial; // Color of cable (deactivated).

    private bool isMoving = false;
    private bool movingTowardsTarget = true; // Keeps track of current direction
    private Vector3 initialPosition;
    private Coroutine movementCoroutine;

    void Start()
    {
        if (platformToMove != null)
        {
            initialPosition = platformToMove.transform.position;
        }
        else
        {
            Debug.LogError("Platform not assigned!");
        }

        if (cable != null && defaultCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = defaultCableMaterial;
        }
    }

    public void ActivatePlatform()
    {
        if (isMoving)
        {
            StopMovement();
            DeactivateCable();
        }
        else
        {
            StartMovement();
            ActivateCable();
        }
    }

    private void StartMovement()
    {
        isMoving = true;

        switch (platformDirection)
        {
            case PlatformDirection.UpDown:
                movementCoroutine = StartCoroutine(MoveUpDown());
                break;
            case PlatformDirection.LeftRight:
                movementCoroutine = StartCoroutine(MoveLeftRight());
                break;
            case PlatformDirection.ForwardBackward:
                movementCoroutine = StartCoroutine(MoveForwardBackward());
                break;
        }
    }

    private void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        isMoving = false;
        Debug.Log("Platform movement paused.");
    }

    IEnumerator MoveUpDown()
    {
        float targetY = initialPosition.y + moveDistance;

        while (isMoving)
        {
            Vector3 upPos = new Vector3(initialPosition.x, targetY, initialPosition.z);

            if (movingTowardsTarget)
            {
                while (Mathf.Abs(platformToMove.transform.position.y - upPos.y) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, upPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = false;
            }
            else
            {
                while (Mathf.Abs(platformToMove.transform.position.y - initialPosition.y) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, initialPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = true;
            }
        }
    }

    IEnumerator MoveLeftRight()
    {
        float targetX = initialPosition.x + moveDistance;

        while (isMoving)
        {
            Vector3 rightPos = new Vector3(targetX, initialPosition.y, initialPosition.z);

            if (movingTowardsTarget)
            {
                while (Mathf.Abs(platformToMove.transform.position.x - rightPos.x) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, rightPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = false;
            }
            else
            {
                while (Mathf.Abs(platformToMove.transform.position.x - initialPosition.x) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, initialPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = true;
            }
        }
    }

    IEnumerator MoveForwardBackward()
    {
        float targetZ = initialPosition.z + moveDistance;

        while (isMoving)
        {
            Vector3 forwardPos = new Vector3(initialPosition.x, initialPosition.y, targetZ);

            if (movingTowardsTarget)
            {
                while (Mathf.Abs(platformToMove.transform.position.z - forwardPos.z) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, forwardPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = false;
            }
            else
            {
                while (Mathf.Abs(platformToMove.transform.position.z - initialPosition.z) > 0.1f && isMoving)
                {
                    platformToMove.transform.position = Vector3.MoveTowards(
                        platformToMove.transform.position, initialPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                movingTowardsTarget = true;
            }
        }
    }

    private void ActivateCable()
    {
        if (cable != null && activatedCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = activatedCableMaterial;
        }
    }

    private void DeactivateCable()
    {
        if (cable != null && defaultCableMaterial != null)
        {
            cable.GetComponent<Renderer>().material = defaultCableMaterial;
        }
    }
}
