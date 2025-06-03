using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestStateChange : MonoBehaviour
{
    [Serializable]
    public struct DialoguePerQuest
    {
        public TextAsset inkJSON_BeforeQuest;
        public TextAsset inkJSON_StartQuest;
        public TextAsset inkJSON_DuringQuest;
        public TextAsset inkJSON_FinishQuest;
        public TextAsset inkJSON_AfterQuest;
    }
    [SerializeField]
    private DialoguePerQuest[] inkJSONs_PC;
    [SerializeField]
    private DialoguePerQuest[] inkJSONs_Controller;

    public int _questIndex = 0;
    public QuestState _currentQuestState;

    [SerializeField]
    private QuestPoint[] _questPoints;

    private NPCInteract _NPCInteract;

    void Start()
    {
        if (_questPoints.Length == 0)
        {
            _questPoints = GetComponents<QuestPoint>();
        }
        _NPCInteract = GetComponent<NPCInteract>();
    }

    void Update()
    {
        if (_questIndex < _questPoints.Length)
        {
            if (_currentQuestState != _questPoints[_questIndex].currentQuestState)
            {
                UpdateDialogue();
            }
        }
    }

    private void UpdateDialogue()
    {
        _currentQuestState = _questPoints[_questIndex].currentQuestState;

        TextAsset pcJSON = null;
        TextAsset controllerJSON = null;

        var pc = inkJSONs_PC[_questIndex];
        var controller = inkJSONs_Controller[_questIndex];

        if (_currentQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET))
        {
            pcJSON = pc.inkJSON_BeforeQuest;
            controllerJSON = controller.inkJSON_BeforeQuest;
        }
        if (_currentQuestState.Equals(QuestState.CAN_START))
        {
            pcJSON = pc.inkJSON_StartQuest;
            controllerJSON = controller.inkJSON_StartQuest;
        }
        if (_currentQuestState.Equals(QuestState.IN_PROGRESS))
        {
            pcJSON = pc.inkJSON_DuringQuest;
            controllerJSON = controller.inkJSON_DuringQuest;
        }
        if (_currentQuestState.Equals(QuestState.CAN_FINISH))
        {
            pcJSON = pc.inkJSON_FinishQuest;
            controllerJSON = controller.inkJSON_FinishQuest;
        }
        if (_currentQuestState.Equals(QuestState.FINISHED))
        {
            pcJSON = pc.inkJSON_AfterQuest;
            controllerJSON = controller.inkJSON_AfterQuest;
            _questIndex += 1;
        }
        // If not null, then set the PC and Controller version of the dialogue text and change it
        if (pcJSON != null)
        {
            _NPCInteract.inkJSON_PC_current = pcJSON;
            _NPCInteract.inkJSON_Controller_current = controllerJSON;

            _NPCInteract.SwitchDialogue();
        }
    }
}
