using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GameObject InteractButton => _interactButton;
    public GameObject CollectButton => _collectButton;

    [SerializeField]
    private GameObject _interactButton;
    [SerializeField]
    private GameObject _collectButton;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _interactButton.SetActive(false);
        _collectButton.SetActive(false);
    }

    public void ToolBoxPopUp()
    {
        _animator.Play("Anim_ToolBoxPopup");
    }
}
