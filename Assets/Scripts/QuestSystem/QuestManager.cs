using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public Quest activeQuest;

    void Awake() => Instance = this;

    public void StartQuest(Quest quest)
    {
        activeQuest = quest;
        Debug.Log($"Quest Started: {quest.questName}");
        QuestUI.Instance.UpdateUI(quest);
    }
}
