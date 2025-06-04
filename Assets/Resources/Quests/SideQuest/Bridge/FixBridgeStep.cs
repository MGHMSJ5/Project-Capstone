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
        Invoke("CheckIfDone", 1f);
    }

    private void CheckIfDone()
    {
        print("Function was called. Did it work?");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
        else
        {
            minorRepair.RepairAction += FinishQuestStep;
        }
        print("Hell yeah, it worked!");
    }
}
