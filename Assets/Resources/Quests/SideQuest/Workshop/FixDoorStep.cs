using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDoorStep : QuestStep
{
    private Collider _repairCollider;
    private MinorRepair minorRepair;

    private void Awake()
    {
        GameObject workshopDoor = GameObject.Find("WorkshopDoor");
        _repairCollider = workshopDoor.GetComponent<Collider>();
        minorRepair = workshopDoor.GetComponent<MinorRepair>();
        
    }
    private void OnEnable()
    {
        _repairCollider.enabled = true;
        minorRepair.RepairAction += FixDoor;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixDoor;
    }

    //Add that the queststep is finished when interacting with the door
    private void FixDoor()
    {
        FinishQuestStep();
        minorRepair.RepairAction -= FixDoor;
    }

    private void CheckIfDone()
    {
        print("Function was called. Did it work?");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
        print("Hell yeah, it worked!");
    }
}
