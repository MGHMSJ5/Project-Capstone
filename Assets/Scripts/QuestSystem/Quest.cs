using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static QuestSteps;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]

public class Quest : ScriptableObject
{
    public string questName;
    public List<QuestStep> steps;

    public void CompleteStep(string stepID)
    {
        var step = steps.Find(s => s.stepID == stepID);
        if (step != null && !step.isCompleted)
        {
            step.isCompleted = true;
            Debug.Log($"Step completed: {step.description}");

            // Deactivate related steps
            foreach (var id in step.deactivateStepsOnComplete)
            {
                var toDisable = steps.Find(s => s.stepID == id);
                if (toDisable != null) toDisable.isCompleted = true;
            }

            QuestUI.Instance.UpdateUI(this);
        }
    }

    public bool IsStepActive(string stepID)
    {
        var step = steps.Find(s => s.stepID == stepID);
        return step != null && !step.isCompleted;
    }
}
