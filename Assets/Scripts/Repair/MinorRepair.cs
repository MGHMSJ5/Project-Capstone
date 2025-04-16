using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RepairTypeAmount
{
    public RepairTypesOptions repairType;
    public int amount;
    public Sprite icon;

    [HideInInspector]
    public TextMeshProUGUI amountText;
    [HideInInspector]
    public int currentAmount;
}

public class MinorRepair : BaseInteract
{
    [Tooltip("IMPORTANT! This action will run when the player has the right amount of resources and repairs. A different script with the unique 'result of repairing' code needs to subscribe to this event. The purpose of this is that scripts will be ordened")]
    public event Action RepairAction;
    [Header("Repair Variables")]
    [Tooltip("Use the list to set what type of repair source the repair needs, and how many. Put in the icon what the image next to the text needs to be in the canvas. Make sure to not have duplicate sources in the list! The MAX is 2 repair types")]
    //[SerializeField] private List<RepairTypes> _repairTypes = new List<RepairTypes>();
    [SerializeField] private List<RepairTypeAmount> _repairTypeAmount = new List<RepairTypeAmount>();
    private List<GameObject> _childTextUI = new List<GameObject>();
    
    [Header("Repair UI")]
    private GameObject _canvas;
    // max repair types set. If this goes past 3, then the if statement in start needs to be changed
    private int _maxRepairTypes = 2;

    protected override void Start()
    {
        base.Start();
        // Get the Canvas and deactivte it
        _canvas = transform.GetChild(0).gameObject;
        _canvas.SetActive(false);

        if (_repairTypeAmount.Count > _maxRepairTypes)
        {
            return;
        }

        // Duplicate the resource text on the canvas that appears when the player is near.
        // It will be duplicated if more than 1 (so 2) repair types are set for the player to get
        Transform resourceAmountObject = _canvas.transform.GetChild(1);
        GameObject original = resourceAmountObject.GetChild(0).gameObject;
        _childTextUI.Add(original);
        if (_repairTypeAmount.Count > 1)
        {
            GameObject duplicate =  Instantiate(original, original.transform.position, original.transform.rotation);
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
        UpdateRepairUIText();
    }

    protected override void InteractFunction()
    {
        base.InteractFunction();
        // Check for if the player has enough repair resources compared to what is needed
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            if (_repairTypeAmount[i].currentAmount >= _repairTypeAmount[i].amount)
            {
                continue;
            }
            else
            {
                _hasInteracted = false;
                // To interact again while still in the collider:
                SetInteract(true);
                // Return if the player doesnt have enough resources
                return;
            }
        }
        Repair();
    }

    private void Repair()
    {
        // Invoke the action. Functions subscribed to this event will then also be invoked.
        RepairAction?.Invoke();

        // Desctivate the canvas and remove the amount of resources needed for the repairing from the repair sources (in RepairResource script)
        _canvas.SetActive(false);
        for (int i = 0; i < _repairTypeAmount.Count; ++i)
        {
            RepairResources.RemoveResourceAmount(_repairTypeAmount[i].repairType, _repairTypeAmount[i].amount);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger + make sure that the player can't interact again if ticked _interactionOnce
        if (other.gameObject.tag == "Player" && !_hasInteracted)
        {
            SetInteract(true);
            _canvas.SetActive(true);
            UpdateRepairUIText();
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

    private void UpdateRepairUIText()
    {   // Update the UI by updating the amount that the player currently has, and updating the text
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            _repairTypeAmount[i].currentAmount = RepairResources.GetResourceAmount(_repairTypeAmount[i].repairType);
            string newText = _repairTypeAmount[i].currentAmount + "/" + _repairTypeAmount[i].amount;
            _repairTypeAmount[i].amountText.text = newText;
        }
    }
}
