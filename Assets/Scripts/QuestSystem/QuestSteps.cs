using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSteps : MonoBehaviour
{
    public enum StepType { Trigger, Dialogue, Item }

    [System.Serializable]
    public class QuestStep
    {
        public string stepID;
        public StepType type;
        public string description;
        public bool isCompleted = false;
        public List<string> deactivateStepsOnComplete;
    }
}
