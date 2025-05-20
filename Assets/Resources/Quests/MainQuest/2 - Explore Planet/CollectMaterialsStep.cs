using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CollectMaterialsStep : QuestStep
{    
    private int screwsCollected;
    private int screwsToComplete = 5;
    //TODO - Repeat the same for the oil and change the numbers accordingly.

    private void OnEnable()
    {
        GameObject.Find("RepairableGate").GetComponent<Collider>().enabled = true;
        GameEventsManager.instance.toolboxOpened += ScrewCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.toolboxOpened -= ScrewCollected;
    }

    public void ScrewCollected()
    {
        screwsCollected = RepairResources.GetResourceAmount(RepairTypesOptions.Screws);
        Debug.Log("Function called.");
        if (screwsCollected <= screwsToComplete)
        {
            Debug.Log("Screws collected.");
            Debug.Log("Collected: " +  screwsCollected + " Needed: " + screwsToComplete);
        }
        
        if (screwsCollected >= screwsToComplete)
        {
            FinishQuestStep();
        }
    }
}
