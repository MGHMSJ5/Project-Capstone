using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class is used to make a 'nested' list. So that in the inspector, multiple things can be 'connected'
// It is used to set the repair resource that is needed to repair the object. // Thomas - "repar" to repair
[System.Serializable]
public class RepairTypeAmount
{
    public RepairTypesOptions repairType; // Type of resource
    public int amount; // Amount that is needed to repair the object
    public Sprite icon; // Sprite that will appear on the UI that appears when the player is near the repair item

    [HideInInspector]
    public TextMeshProUGUI amountText; // Text that is references by getting the child
    [HideInInspector]
    public int currentAmount; // The current amount of the type of resources the player currently has
}

public class MinorRepair : BaseInteract
{
    [Tooltip("IMPORTANT! This action will run when the player has the right amount of resources and repairs. A different script with the unique 'result of repairing' code needs to subscribe to this event. The purpose of this is that scripts will be ordened. An example of this script is MinorRepairEXAMPLE.cs")]
    public event Action RepairAction;
    [Header("Repair Variables")]
    [Tooltip("Use the list to set what type of repair source the repair needs, and how many. Put in the icon what the image next to the text needs to be in the canvas. Make sure to not have duplicate sources in the list! The MAX is 2 repair types")]
    [SerializeField] protected List<RepairTypeAmount> _repairTypeAmount = new List<RepairTypeAmount>();
    // Used to store the UI text in that is then used to change the text when the player enters the trigger (updates the text based on the player's current resource amount
    private List<GameObject> _childTextUI = new List<GameObject>();
    
    [Header("Repair UI")]
    protected GameObject _canvas;
    // max repair types set. If this goes past 3, then the if statement in SetResourceUI needs to be changed! And also check if the UI (on the canvas) is still correct
    private int _maxRepairTypes = 2;

    [HideInInspector]
    public bool HasBeenRepaired = false;

    [SerializeField]
    private ParticleSystem _repairParticle;
    protected override void Start()
    {
        base.Start();

        // Get the Canvas and deactivte it
        _canvas = transform.GetChild(0).gameObject;
        _canvas.SetActive(false);
        // Return if the amount of possible repair sources is lower than the needed resources for repair in the inspector
        if (_repairTypeAmount.Count > _maxRepairTypes)
        {
            Debug.LogError("To many repair resources set on " + this.name);
            return;
        }
        // Set the UI based on the repair resource needed and update the UI
        SetResourceUI();
        UpdateRepairUIText();
    }

    protected override void InteractFunction()
    {
        base.InteractFunction();
        if (CanRepair())
        {   // Repair item
            Repair();
            // set to true so that the player can not interact with it again
            _hasInteracted = true;
        }
        else
        { // Can not repair item
          //Activate unrepairable sound
            SoundManager.PlaySound(SoundType.UNREPAIRABLE);
            SetInteract(true);
        }
        
    }
    protected virtual void Repair()
    {
        //Activate repair sound if repair is activated
        SoundManager.PlaySound(SoundType.REPAIR);

        // Play the perticle system
        _repairParticle?.Play();

        HasBeenRepaired = true;
        // Invoke the action. Functions subscribed to this event will then also be invoked.
        RepairAction?.Invoke();
        
        // Desctivate the canvas and remove the amount of resources needed for the repairing from the repair sources (in RepairResource script)
        _canvas.SetActive(false);
        int totalRemovedResources = 0;
        // Update the resources
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            totalRemovedResources += _repairTypeAmount[i].amount;
            RepairResources.RemoveResourceAmount(_repairTypeAmount[i].repairType, _repairTypeAmount[i].amount);
        }
        // Update the UI and play the animation of the UI source
        _UICanvas.ChangeUI("...", "- " + totalRemovedResources);
        // Run the funtion to change UI and add to current screw amount
        _UICanvas.ChangeResourcesUI();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger + make sure that the player can't interact again if ticked _interactionOnce
        if (other.gameObject.tag == "Player" && !_hasInteracted)
        {   // Get the current amount of repair resources and update the UI
            UpdateRepairUIText();
            SetInteract(true);
            _canvas.SetActive(true);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger
        if (other.gameObject.tag == "Player")
        {
            SetInteract(false);
            _canvas.SetActive(false);
        }
    }
    protected virtual bool CanRepair()
    {
        // Check for if the player has enough repair resources compared to what is needed
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            if (_repairTypeAmount[i].currentAmount >= _repairTypeAmount[i].amount)
            {
                continue;
            }
            else
            {
                // Return if the player doesnt have enough resources
                return false;
            }
        }
        return true;
    }

    private void UpdateRepairUIText()
    {   // Update the UI by updating the amount that the player currently has, and updating the text
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            _repairTypeAmount[i].currentAmount = RepairResources.GetResourceAmount(_repairTypeAmount[i].repairType);
            string newText = _repairTypeAmount[i].currentAmount + "/" + _repairTypeAmount[i].amount;
            _repairTypeAmount[i].amountText.text = newText;
        }
    }
    protected virtual void SetResourceUI()
    {
        // Duplicate the resource text on the canvas that appears when the player is near.
        // It will be duplicated if more than 1 (so 2) repair types are set for the player to get
        Transform resourceAmountObject = _canvas.transform.GetChild(1);
        GameObject original = resourceAmountObject.GetChild(0).gameObject;
        _childTextUI.Add(original);
        if (_repairTypeAmount.Count > 1)
        {
            GameObject duplicate = Instantiate(original, original.transform.position, original.transform.rotation);
            duplicate.transform.SetParent(resourceAmountObject, worldPositionStays: true);
            _childTextUI.Add(duplicate);
        }
        // Go through the list of the repair amount, reference the text, and set the right icon
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            _repairTypeAmount[i].amountText = _childTextUI[i].GetComponent<TextMeshProUGUI>();
            Image repairTypeImage = _childTextUI[i].transform.GetChild(0).GetComponent<Image>();
            repairTypeImage.sprite = _repairTypeAmount[i].icon;
        }
    }
}
