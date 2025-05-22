using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBridgeStep : QuestStep
{
    private BaseInteract baseInteract;
    private void Awake()
    {
        baseInteract = GameObject.Find("LandingAreaBridge").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBridge;
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
}
