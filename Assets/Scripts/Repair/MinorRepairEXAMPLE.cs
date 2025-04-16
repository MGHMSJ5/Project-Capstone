using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}

public class MinorRepairEXAMPLE : BaseInteract
{
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
        _canvas = transform.GetChild(0).gameObject;
        _canvas.SetActive(false);

        if (_repairTypeAmount.Count > _maxRepairTypes)
        {
            return;
        }

        // Duplicate the resource text on the canvas that appears when the player is near.
        // It will be duplicated if more than 1 (so 2) repair types are set for the player to get
        if (_repairTypeAmount.Count > 1)
        {
            Transform resourceAmountObject = _canvas.transform.GetChild(1);
            GameObject original = resourceAmountObject.GetChild(0).gameObject;
            GameObject duplicate =  Instantiate(original, original.transform.position, original.transform.rotation);
            duplicate.transform.SetParent(resourceAmountObject, worldPositionStays: true);
            _childTextUI.Add(original);
            _childTextUI.Add(duplicate);
        }
        // Go through the list of the repair amount, reference the text, and set the right icon
        for (int i = 0; _repairTypeAmount.Count > i; i++)
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
        print("Repair");
        // To interact again while still in the collider:
        //SetInteract(true);
    }
    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        _canvas.SetActive(true);
        UpdateRepairUIText();
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        _canvas.SetActive(false);
    }

    private void UpdateRepairUIText()
    {
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            int currentAmount = RepairResources.GetResourceAmount(_repairTypeAmount[i].repairType);
            string newText = currentAmount + "/" + _repairTypeAmount[i].amount;
            _repairTypeAmount[i].amountText.text = newText;
        }
    }
}
