using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBenchStep : QuestStep
{
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;
    private void Awake()
    {
        baseInteract = GameObject.Find("CliffBench").GetComponent<BaseInteract>();
        minorRepair = GameObject.Find("CliffBench").GetComponent<MinorRepair>();

    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBench;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixBench;
    }

    //Add that the queststep is finished when interacting with the bench
    private void FixBench()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixBench;

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
