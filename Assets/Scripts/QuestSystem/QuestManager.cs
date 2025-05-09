using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {

        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        //Broadcast the initial state of all quests on startup
        foreach(Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequriementsMet(Quest quest)
    {
        //Start true, with the goal to prove to be false
        bool meetsRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisits)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        //Loop through ALL quests
        foreach(Quest quest in questMap.Values)
        {
            //If they're now meeting the requirements. switch over to the CAN_START state
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequriementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        //Getting the quest by its id
        Quest quest = GetQuestById(id);
        quest.InstanciateCurrentQuestStep(this.transform);

        //Change quest state to in progress
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);

        //Debug message
        Debug.Log("Quest started: " +  id);
    }

    private void AdvanceQuest(string id)
    {
        //Getting the quest by its id
        Quest quest = GetQuestById(id);
        
        //Move quest to next step
        quest.MoveToNextStep();

        //If there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            quest.InstanciateCurrentQuestStep(this.transform);
        }
        //If there are no more quest steps, we have finished the quest
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }

        //Debug message
        Debug.Log("Quest advanced: " + id);
    }

    private void FinishQuest(string id)
    {
        //Getting the quest by its id
        Quest quest = GetQuestById(id);

        //Claim rewards method is called
        ClaimRewards(quest);

        //Change quest state to finished
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

        //Debug message
        Debug.Log("Quest finished: " + id);
    }

    private void ClaimRewards(Quest quest)
    {
        GameEventsManager.instance.rewardEvents.ItemGained(quest.info.questReward); //TODO: How will the upgrades be added to the player? Components? Items? Bool change? Connect it!
        GameEventsManager.instance.rewardEvents.ScrewsGained(quest.info.screwReward); //TODO: Add to UI + actual player inventory
        Debug.Log("Reward claimed");
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        //Load all QuestInfoSo Scriptable Objects under the Assets/Resources/Quests folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach(QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " +  questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    //Catch errors if ever catching a quest id that doesn't exists
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the quest map: " + id);
        }
        return quest;
    }
}
