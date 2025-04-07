using UnityEngine;

public class UIInteract : MonoBehaviour
{
    public GameObject InteractButton => interactButton;
    public GameObject ColectButton => collectButton;
    [SerializeField]
    private GameObject interactButton;
    [SerializeField]
    private GameObject collectButton;

    void Start()
    {
        interactButton.SetActive(false);
        collectButton.SetActive(false);
    }
}
