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
        Invoke("CheckIfDone", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }

    private void CheckIfDone()
    {
        print("Function was called. Did it work?");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
        print("Hell yeah, it worked!");
    }
}
