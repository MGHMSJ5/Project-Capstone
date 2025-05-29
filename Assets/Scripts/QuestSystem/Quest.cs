using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    //static info
    public QuestInfoSO info;

    public int CurrentQuestStepIndex => currentQuestStepIndex;

    //state info
    public QuestState state;
    private int currentQuestStepIndex;

    public int GetCurrentStepIndex() => currentQuestStepIndex;
    public void SetCurrentStepIndex(int index) => currentQuestStepIndex = index;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstanciateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null)
        {
            //Next line could be changed to object pooling IF performance is an issue
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that " + "there is no current step: QuestId" + info.id + ", stepIndex=" + currentQuestStepIndex);
        }
        return questStepPrefab;
    }
}