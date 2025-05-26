using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDoorStep : QuestStep
{
    private BaseInteract baseInteract;
    private void Awake()
    {
        baseInteract = GameObject.Find("WorkshopDoor").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixDoor;
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
}
