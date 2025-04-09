using TMPro;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GameObject InteractButton => _interactButton;
    public GameObject CollectButton => _collectButton;
    public TextMeshProUGUI PopUpText => _popUpText;
    public TextMeshProUGUI ScrewAddedText => _screwAddedText;

    [SerializeField]
    private GameObject _interactButton;
    [SerializeField]
    private GameObject _collectButton;
    [SerializeField]
    private TextMeshProUGUI _popUpText;
    [SerializeField]
    private TextMeshProUGUI _screwAmount;
    [SerializeField]
    private TextMeshProUGUI _screwAddedText;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _interactButton.SetActive(false);
        _collectButton.SetActive(false);
        SetUIScrewAmount();
    }

    public void ToolBoxPopUp()
    {
        _animator.SetTrigger("AddScrews");
        _animator.Play("ToolBoxPopup");
    }

    // Called from animation that'll add to the repair source
    public void AddToRepairResource()
    {
        string s = _screwAddedText.text.Replace(" ", "");
        int i = int.Parse(s);
        RepairResources.AddScrews(i);
        SetUIScrewAmount();
    }

    private void SetUIScrewAmount()
    {
        _screwAmount.text = "X " + RepairResources.GetScrewAmount();
    }
}
