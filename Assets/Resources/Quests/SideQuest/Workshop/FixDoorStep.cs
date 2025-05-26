using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDoorStep : QuestStep
{
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;

    private void Awake()
    {
        baseInteract = GameObject.Find("WorkshopDoor").GetComponent<BaseInteract>();
        minorRepair = GameObject.Find("WorkshopDoor").GetComponent<MinorRepair>();
    }
    private void OnEnable()
    {
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
