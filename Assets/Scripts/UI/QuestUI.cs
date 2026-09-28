using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("Main Quests")]
    [SerializeField]
    private QuestUIHandling mainQuest;

    [Header("Sidequests")]
    [SerializeField]
    private List<QuestUIHandling> sideQuests = new List<QuestUIHandling>();  

    public void StartQuest(string title, string description)
    {
        mainQuest.StartQuest(title, description);
    }

    public void UpdateQuest(string title, string description)
    {
        mainQuest.UpdateQuest(title, description);
    }

    public void FinishedQuest()
    {
        mainQuest.FinishedQuest();
    }

    public void StartSideQuest(string title, string description, string questID)
    {
        for (int i = 0; i < sideQuests.Count; i++)
        {
            if (sideQuests[i].sideQuestInfo.id == questID)
            {
                sideQuests[i].StartQuest(title, description);
                return;
            }
        }
    }

    public void UpdateSideQuest(string title, string description, string questID)
    {
        for (int i = 0; i < sideQuests.Count; i++)
        {
            if (sideQuests[i].sideQuestInfo.id == questID)
            {
                sideQuests[i].UpdateQuest(title, description);
                return;
            }
        }
    }
}
