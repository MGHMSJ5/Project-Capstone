using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;
    public string questText;

    void Awake() => Instance = this;

    public void UpdateUI(Quest quest)
    {
        var activeSteps = quest.steps.FindAll(s => !s.isCompleted);
        questText = "Current Objectives:\n";
        foreach (var step in activeSteps)
        {
            questText += $"- {step.description}\n";
        }
    }
}
