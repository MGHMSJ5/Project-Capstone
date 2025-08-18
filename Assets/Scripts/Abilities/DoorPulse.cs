using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DoorPulse : MonoBehaviour
{
    public GameObject door; // Door object goes here
    public Vector3 openOffset = new Vector3(0, 3, 0); // How much door opens/direction
    public GameObject panel; // Panel object goes here
    public Material activatedPanelMaterial; // Color of the panel when activated.
    public float openSpeed = 2f; // Door speed

    private bool isOpened = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        if (door != null)
        {
            initialPosition = door.transform.position;
            targetPosition = initialPosition + openOffset;
        }
        else
        {
            Debug.LogError("Door is not assigned to DoorPulse!");
        }
    }

    public void ActivateDoor()
    {
        if (!isOpened)
        {
            isOpened = true;
            StartCoroutine(OpenDoor());
            ActivatePanel();
        }
    }
    private void ActivatePanel()
    {
        if (panel != null && activatedPanelMaterial != null)
        {
            Renderer renderer = panel.GetComponent<Renderer>();
            Material[] materials = renderer.materials.Clone() as Material[];

            if (materials.Length > 1)
            {
                materials[1] = activatedPanelMaterial;
                renderer.materials = materials;
            }
        }
    }

    private System.Collections.IEnumerator OpenDoor()
    {
        while (Vector3.Distance(door.transform.position, targetPosition) > 0.01f)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position, targetPosition, openSpeed * Time.deltaTime);
            yield return null;
        }

        door.transform.position = targetPosition; // Final position
    }
}