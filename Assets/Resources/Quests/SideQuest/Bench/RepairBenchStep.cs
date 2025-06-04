using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBenchStep : QuestStep
{
    private MinorRepair minorRepair;
    private void Awake()
    {
        minorRepair = GameObject.Find("CliffBench").GetComponent<MinorRepair>();

    }
    private void OnEnable()
    {
        minorRepair.RepairAction += FixBench;
        Invoke("CheckIfDone", 1f);
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixBench;
    }

    //Add that the queststep is finished when interacting with the bench
    private void FixBench()
    {
        FinishQuestStep();
        minorRepair.RepairAction -= FixBench;
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
