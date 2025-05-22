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
        baseInteract.onSubmitPressed += FixWaterpumpPipe;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixWaterpumpPipe;
    }

    //Add that the queststep is finished when interacting with the leak
    private void FixWaterpumpPipe()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixWaterpumpPipe;

        baseInteract.InvokeSubmitPressed();
    }

    private void CheckIfDone()
    {
        print("This was called. Be happy if this works!");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
    }
}
