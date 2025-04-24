using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMaterialsStep : QuestStep
{    
    private int screwsCollected = RepairResources.GetScrewAmount();
    private int screwsToComplete = 5;
    //TODO - Repeat the same for the oil and change the numbers accordingly.
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
