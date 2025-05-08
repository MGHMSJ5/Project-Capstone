using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is supposed to be inherited by another script and not used alone, therefore it is an abstract class.
public abstract class QuestStep : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField]
    private QuestInfoSO questInfo;
    [SerializeField] public string description;

    private bool isFinished = false;
    private string questId;

    private void Awake()
    {
        questId = questInfo.id;
    }
    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            print(questId);
            isFinished = true;
            GameEventsManager.instance.questEvents.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }
    }
}
