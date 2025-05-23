using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TalkToWorkshopNPCStep : QuestStep
{
    private BaseInteract baseInteract;

    private void Awake()
    {
        baseInteract = GameObject.Find("Neval").GetComponent<BaseInteract>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += GrandAbility;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= GrandAbility;
    }

    //Add that the queststep is finished when talking to NPC
    private void GrandAbility()
    {
        FinishQuestStep();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHover>()._hoverAbilityGranted = true;
        baseInteract.onSubmitPressed -= GrandAbility;

        baseInteract.InvokeSubmitPressed();
    }

}
