using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBenchStep : QuestStep
{
    private BaseInteract baseInteract;
    private void Awake()
    {
        baseInteract = GameObject.Find("CliffBench").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBench;
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
}
