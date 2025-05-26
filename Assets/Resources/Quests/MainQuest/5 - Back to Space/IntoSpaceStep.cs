using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoSpaceStep : QuestStep
{
    private BaseInteract baseInteract;

private void Awake()
{
    baseInteract = GameObject.Find("PatchSpaceship").GetComponent<BaseInteract>();
}
private void OnEnable()
{
    baseInteract.onSubmitPressed += IntoSpace;
}

private void OnDisable()
{
    baseInteract.onSubmitPressed -= IntoSpace;
}

//Add that the queststep is finished when talking to NPC
private void IntoSpace()
{
    FinishQuestStep();
    baseInteract.onSubmitPressed -= IntoSpace;

    baseInteract.InvokeSubmitPressed();
}
}
