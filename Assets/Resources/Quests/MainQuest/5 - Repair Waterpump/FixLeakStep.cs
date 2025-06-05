using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLeakStep : QuestStep
{
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;

    private void Awake()
    {
        baseInteract = GameObject.Find("WaterPumpPipe").GetComponent<BaseInteract>();
        minorRepair = GameObject.Find("WaterPumpPipe").GetComponent<MinorRepair>();
    }
    private void OnEnable()
    {
        minorRepair.RepairAction += FixWaterpumpPipe;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixWaterpumpPipe;
    }

    //Add that the queststep is finished when interacting with the leak
    private void FixWaterpumpPipe()
    {
        FinishQuestStep();
        minorRepair.RepairAction -= FixWaterpumpPipe;
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
