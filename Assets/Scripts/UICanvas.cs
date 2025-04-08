using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GameObject InteractButton => interactButton;
    public GameObject CollectButton => collectButton;
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
