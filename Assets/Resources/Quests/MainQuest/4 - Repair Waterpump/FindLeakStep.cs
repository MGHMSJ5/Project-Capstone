using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class FindLeakStep : QuestStep
{
    private MajorRepair minorRepair;

    private void Awake()
    {
        minorRepair = GameObject.Find("WaterPumpPipe").GetComponent<MajorRepair>();
    }

    private void OnEnable()
    {
        Invoke("CheckIfRepaired", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }

    private void CheckIfRepaired()
    {
        print("On Enable of FindLeakStep has happened. We are not happy :(");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
    }
}
