using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDoorStep : QuestStep
{
    private Collider _repairCollider;
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;

    private void Awake()
    {
        GameObject workshopDoor = GameObject.Find("WorkshopDoor");
        _repairCollider = workshopDoor.GetComponent<Collider>();
        baseInteract = workshopDoor.GetComponent<BaseInteract>();
        minorRepair = workshopDoor.GetComponent<MinorRepair>();
        
    }
    private void OnEnable()
    {
        _repairCollider.enabled = true;
        baseInteract.onSubmitPressed += FixDoor;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixDoor;
    }

    //Add that the queststep is finished when interacting with the door
    private void FixDoor()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixDoor;

        baseInteract.InvokeSubmitPressed();
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
