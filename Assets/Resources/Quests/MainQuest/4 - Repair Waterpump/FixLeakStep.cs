using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLeakStep : QuestStep
{
    private BaseInteract baseInteract;

    private void Awake()
    {
        baseInteract = GameObject.Find("WaterPumpPipe").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixWaterpumpPipe;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixWaterpumpPipe;
    }

    //Add that the queststep is finished when talking to NPC
    private void FixWaterpumpPipe()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixWaterpumpPipe;

        baseInteract.InvokeSubmitPressed();
    }
}
