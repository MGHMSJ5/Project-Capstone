using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("Main Quests")]
    [SerializeField]
    private QuestUIHandling mainQuest;

    [Header("Sidequests")]
    [SerializeField]
    private List<QuestUIHandling> sideQuests = new List<QuestUIHandling>();

    [SerializeField]
    private List<float> sideQuestLocations = new List<float>();
    private int startPosIndex = 0;

    public void StartQuest(string title, string description)
    {
        mainQuest.StartQuest(title, description, 0);
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
                sideQuests[i].StartQuest(title, description, sideQuestLocations[startPosIndex]);
                startPosIndex++;
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

    public void FinishedSideQuest(string questID)
    {
        for (int i = 0; i < sideQuests.Count; i++)
        {
            if (sideQuests[i].sideQuestInfo.id == questID)
            {
                sideQuests[i].FinishedQuest();
                startPosIndex--;
                return;
            }
        }
    }
}
