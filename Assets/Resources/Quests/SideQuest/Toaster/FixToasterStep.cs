using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixToasterStep : QuestStep
{
    private NPCInteract npcInteract;
    private void Awake()
    {
        npcInteract = GameObject.Find("ToasterInteract").GetComponent<NPCInteract>();
    }
    private void OnEnable()
    {
        npcInteract.DoneTalkingEvent += FixToaster;
    }

    private void OnDisable()
    {
        npcInteract.DoneTalkingEvent -= FixToaster;
    }

    //Add that the queststep is finished when interacting with the toaster
    private void FixToaster()
    {
        FinishQuestStep();
        npcInteract.DoneTalkingEvent -= FixToaster;
    }
}
