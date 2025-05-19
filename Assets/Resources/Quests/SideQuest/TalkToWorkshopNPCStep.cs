using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TalkToWorkshopNPCStep : QuestStep
{
    private void OnEnable()
    {
        GameObject.Find("Neval").GetComponent<BaseInteract>().onSubmitPressed += GrandAbility;
    }

    private void OnDisable()
    {
        GameObject.Find("Neval").GetComponent<BaseInteract>().onSubmitPressed -= GrandAbility;
    }

    //Add that the queststep is finished when talking to NPC
    private void GrandAbility()
    {
        FinishQuestStep();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHover>()._hoverAbilityGranted = true;
    }

}
