using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGateStep : QuestStep
{
    private MinorRepair minorRepair;
    private void Awake()
    {
        minorRepair = GameObject.Find("RepairableGate").GetComponent<MinorRepair>();
    }
    private void OnEnable()
    {
        minorRepair.RepairAction += FixGate;
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixGate;
    }

    //Add that the queststep is finished when interacting with the gate
    private void FixGate()
    {
        FinishQuestStep();
        minorRepair.RepairAction -= FixGate;

    }
}
