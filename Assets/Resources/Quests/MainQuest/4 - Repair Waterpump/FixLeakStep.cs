using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLeakStep : QuestStep
{
    private BaseInteract baseInteract;
    //private GameObject pipeCanvas;

    private void Awake()
    {
        baseInteract = GameObject.Find("WaterPumpPipe").GetComponent<BaseInteract>();
        //pipeCanvas = GameObject.Find("WaterPumpPipe").transform.GetChild(0).GetComponent<GameObject>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixWaterpumpPipe;
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
}
