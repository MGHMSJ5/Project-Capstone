using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GameObject InteractButton => _interactButton;
    public GameObject CollectButton => _collectButton;

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
    {   // Play the animation (resource text pop up & screw amount pop up
        _animator.SetTrigger("AddScrews"); // Will maybe be used for a later mechanic that will be added (screw amount will appear when player stands still)
        _animator.Play("ToolBoxPopup");
        // Elise: here it first removed the space from '+ 2'. Otherwise the Parse() doesn't work
        string s = _screwAddedText.text.Replace(" ", "");
        //Lea: Please clarify what you mean with the term Parse
        // Elise: Parse() here makes it so that the string s's numbers will be converted to int. 
        // Elise: So '+2' for example will be '2'
        int i = int.Parse(s);
        AddToRepairResource(i);
    }

    public void NPCGivesResource(int givenAmount)
    {
        _animator.SetTrigger("AddScrews"); // Will maybe be used for a later mechanic that will be added (screw amount will appear when player stands still)
        _animator.Play("ResourceUpdatePopup");
        AddToRepairResource(givenAmount);
    }

    public void ChangeUI(string popUpText, string screwUpdateText)
    {
        _popUpText.text = popUpText;
        _screwAddedText.text = screwUpdateText;
    }

    private void AddToRepairResource(int amount)
    {   
        RepairResources.AddResourceAmount(RepairTypesOptions.Screws, amount);
        GameEventsManager.instance.InvokeToolboxEvent();
    }
    // Called from animation (ANIM_CanvasScrewAdd) that'll add to the repair source
    // Update the resource amount for the UI
    private void SetUIScrewAmount()
    {
        _screwAmount.text = "X " + RepairResources.GetResourceAmount(RepairTypesOptions.Screws);
    }
}
