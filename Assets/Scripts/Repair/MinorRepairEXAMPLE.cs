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

    // Add variables here if necessary
    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("Repair");
        // To interact again while still in the collider:
        //SetInteract(true);
    }
}
