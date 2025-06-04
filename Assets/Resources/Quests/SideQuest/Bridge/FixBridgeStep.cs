using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBridgeStep : QuestStep
{
    private MinorRepair minorRepair;


    private void Awake()
    {
        minorRepair = GameObject.Find("LandingBridge").GetComponent<MinorRepair>();
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
