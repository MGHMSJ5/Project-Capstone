using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FollowNPC : QuestStep
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ScriptedEvents.Instance.EnableScriptChobo();
            FinishQuestStep();
            Debug.Log("Queststep completed");
        }
    }
}
