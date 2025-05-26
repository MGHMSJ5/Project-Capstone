using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGateStep : QuestStep
{
    private BaseInteract baseInteract;
    private void Awake()
    {
        baseInteract = GameObject.Find("RepairableGate").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixGate;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixGate;
    }

    //Add that the queststep is finished when interacting with the gate
    private void FixGate()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixGate;

        baseInteract.InvokeSubmitPressed();
    }
}
