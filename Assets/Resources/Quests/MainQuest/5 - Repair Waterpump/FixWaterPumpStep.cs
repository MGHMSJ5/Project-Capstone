using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWaterPump : QuestStep
{
    private MinorRepair minorRepair;

    private void Awake()
    {
        minorRepair = GameObject.Find("WaterPump").GetComponent<MinorRepair>();
    }
    private void OnEnable()
    {
        minorRepair.RepairAction += FixWaterpump;
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixWaterpump;
    }

    //Add that the queststep is finished when interacting with the waterpump
    private void FixWaterpump()
    {
        ScriptedEvents.Instance.TeleportChoboToKettle();
        FinishQuestStep();
        minorRepair.RepairAction -= FixWaterpump;
    }
}
