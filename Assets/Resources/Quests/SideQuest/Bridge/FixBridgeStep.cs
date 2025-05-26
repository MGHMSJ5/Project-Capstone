using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBridgeStep : QuestStep
{
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;
    private void Awake()
    {
        baseInteract = GameObject.Find("LandingAreaBridge").GetComponent<BaseInteract>();
        minorRepair = GameObject.Find("LandingAreaBridge").GetComponent<MinorRepair>();

    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBridge;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixBridge;
    }

    //Add that the queststep is finished when interacting with the bridge
    private void FixBridge()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixBridge;

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
