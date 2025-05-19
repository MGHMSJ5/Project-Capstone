using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ReachHighestPointStep : QuestStep
{

    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPulse>().hasPulseAbility = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
}
