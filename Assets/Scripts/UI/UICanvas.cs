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

    private UIChangeSubject _UIChangeSubject;

    [Header("Idle Resource Appear")]
    private PlayerController _playerController;
    private float counter = 0.0f;
    private float maxWaitTime = 3.0f;
    [SerializeField]
    private bool counterPassed = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _UIChangeSubject = GameObject.Find("UIChangeManager").GetComponent<UIChangeSubject>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    private void Update()
    {
        // Make the current repair resources appear if the player is standing still for X seconds
        //icheck if the player is idle and not in dialogue and the quest step Ui popup is not doing its animation
        if (_playerController.PlayerStateMachine.CurrentState == _playerController.PlayerStateMachine.idleState && !DialogueManager.GetInstance().dialogueIsPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("ToolBoxPopup") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("ANIM_CanvasScrewAdd"))
        {
            if (counter < maxWaitTime)
            {
                counter += Time.deltaTime;
            }
        }
        else
        //reset the timer if idle is interrupted or player enters dialogue
        {
            if (counterPassed)
            {
                _animator.SetTrigger("ContinuePopUp");
            }
            counter = 0.0f;
            counterPassed = false;            
        }
        //check if the counter has passd the time and active the idle UI
        if (counter > maxWaitTime && !counterPassed)
        {
            counterPassed = true;
            _animator.Play("ResourceUpdatePopup");
        }
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

    public void ChangeResourcesUI()
    {
        _animator.SetTrigger("AddScrews"); // Will maybe be used for a later mechanic that will be added (screw amount will appear when player stands still)
        _animator.Play("ResourceUpdatePopup");
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
