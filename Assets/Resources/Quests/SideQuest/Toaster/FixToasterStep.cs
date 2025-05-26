using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixToasterStep : QuestStep
{
    private BaseInteract baseInteract;
    private void Awake()
    {
        baseInteract = GameObject.Find("ToasterInteractExample").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixToaster;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixToaster;
    }

    //Add that the queststep is finished when interacting with the gate
    private void FixToaster()
    {
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixToaster;

        baseInteract.InvokeSubmitPressed();
    }
}
