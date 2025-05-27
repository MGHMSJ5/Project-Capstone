using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWaterPump : QuestStep
{
    private BaseInteract baseInteract;

    private void Awake()
    {
        baseInteract = GameObject.Find("WaterPump").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixWaterpump;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixWaterpump;
    }

    //Add that the queststep is finished when interacting with the waterpump
    private void FixWaterpump()
    {
        ScriptedEvents.Instance.TeleportChoboToKettle();
        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixWaterpump;

        baseInteract.InvokeSubmitPressed();
    }
}
