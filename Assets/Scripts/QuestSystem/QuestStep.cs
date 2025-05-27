using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is supposed to be inherited by another script and not used alone, therefore it is an abstract class.
public abstract class QuestStep : MonoBehaviour
{
    [SerializeField] public string description;

    private bool isFinished = false;
    private string questId;

    private QuestUI _questUI;
    private QuestManager _questManager;

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
        _questUI = GameObject.Find("Canvas").GetComponent<QuestUI>();
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        //get the quest by searching for the quest via the questId
        Quest quest = _questManager.GetQuestById(questId);
        //MAke the questUI appear with the correct name and description from the quest step
        if (quest.CurrentQuestStepIndex == 0)
        {
            if (quest.info.isSideQuest)
            {
                _questUI.StepSidequestUI(quest.info.displayName, description, true);
            }
            else
            {
                _questUI.StepQuestUI(quest.info.displayName, description, true);
            }
        }
        else
        {
            if (quest.info.isSideQuest)
            {
                _questUI.StepSidequestUI(quest.info.displayName, description, false);
            }
            else
            {
                _questUI.StepQuestUI(quest.info.displayName, description, false);
            }
        }
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
