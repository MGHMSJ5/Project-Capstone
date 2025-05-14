using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResourcesWaterStep : QuestStep
{    
    private int screwsCollected = RepairResources.GetResourceAmount(RepairTypesOptions.Screws);
    private int screwsToComplete = 5;
    //TODO - Repeat the same for the secondary repair resource and change the numbers accordingly.
    private void ScrewCollected()
    {
        if (screwsCollected <= screwsToComplete)
        {
            screwsCollected++;
        }

        if(screwsCollected >= screwsToComplete)
        {
            FinishQuestStep();
        }
    }



}
