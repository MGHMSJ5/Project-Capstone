using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RepairTypesOptions
{
    screws,
    tape
}

[System.Serializable]
public class RepairTypeAmount
{
    public RepairTypesOptions repairType;
    public int amount;
}

public class MinorRepairEXAMPLE : BaseInteract
{
    [Header("Repair Variables")]
    [Tooltip("Use the list to set what type of repair source the repair needs, and how many. Make sure to not have duplicate sources in the list!")]
    //[SerializeField] private List<RepairTypes> _repairTypes = new List<RepairTypes>();
    [SerializeField] private List<RepairTypeAmount> _repairTypeAmount = new List<RepairTypeAmount>();

    [Header("Repair UI")]
    private GameObject _canvas;

    protected override void Start()
    {
        base.Start();
        _canvas = transform.GetChild(0).gameObject;
        _canvas.SetActive(false);
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
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        _canvas.SetActive(false);
    }
}
