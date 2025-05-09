using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardEvents
{
    public event Action<object> onItemGained;

    public void ItemGained(GameObject[] questReward)
    {
        if (onItemGained != null)
        {
            onItemGained(questReward);
        }
    }

    public event Action<int> onScrewsGained;

    public void ScrewsGained(int goldReward)
    {
        if(onScrewsGained != null)
        {
            onScrewsGained(goldReward);
        }
    }
}
