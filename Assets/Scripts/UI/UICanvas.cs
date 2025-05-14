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

    private UIChangeSubject _UIChangeSubject;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _UIChangeSubject = GameObject.Find("UIChangeManager").GetComponent<UIChangeSubject>();
    }

    private void OnEnable()
    {
        _UIChangeSubject.UISwitch += UIChange;
    }

    private void OnDisable()
    {
        _UIChangeSubject.UISwitch -= UIChange;
    }

    void Start()
    {
        
        _interactButton.SetActive(false);
        _collectButton.SetActive(false);
        SetUIScrewAmount();
    }

    public void ToolBoxPopUp()
    {   // Play the animation (resource text pop up & screw amount pop up
        _animator.SetTrigger("AddScrews"); // Will maybe be used for a later mechanic that will be added (screw amount will appear when player stands still)
        _animator.Play("ToolBoxPopup");
        AddToRepairResource();
    }

    
    private void AddToRepairResource()
    {
        // Elise: here it first removed the space from '+ 2'. Otherwise the Parse() doesn't work
        string s = _screwAddedText.text.Replace(" ", "");
        //Lea: Please clarify what you mean with the term Parse
        // Elise: Parse() here makes it so that the string s's numbers will be converted to int. 
        // Elise: So '+2' for example will be '2'
        int i = int.Parse(s);
        RepairResources.AddResourceAmount(RepairTypesOptions.Screws, i);
    }
    // Called from animation (ANIM_CanvasScrewAdd) that'll add to the repair source
    // Update the resource amount for the UI
    private void SetUIScrewAmount()
    {
        _screwAmount.text = "X " + RepairResources.GetResourceAmount(RepairTypesOptions.Screws);
    }

    // Activate or deactivate the controller and keyboard button UI depending on if the controller has been plugged in
    private void UIChange(bool isController)
    {
        // Keyboard UI (1st child)
        _interactButton.transform.GetChild(0).gameObject.SetActive(!isController);
        _collectButton.transform.GetChild(0).gameObject.SetActive(!isController);

        // Button UI (2dn child)
        _interactButton.transform.GetChild(1).gameObject.SetActive(isController);
        _collectButton.transform.GetChild(1).gameObject.SetActive(isController);
    }
}
