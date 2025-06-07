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
    [Tooltip("Do NOT change this variable in the inspector to something else. This variable is in the inspector to check for what state a quest is in and if the dialogue is correct.")]
    [SerializeField]
    private QuestState _currentStateOfQuest = QuestState.FINISHED;

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
            if (_currentStateOfQuest != _questPoints[_questIndex].currentQuestState)
            {
                UpdateDialogue();
            }
        }
    }

    private void UpdateDialogue()
    {
        _currentStateOfQuest = _questPoints[_questIndex].currentQuestState;

        TextAsset pcJSON = null;
        TextAsset controllerJSON = null;

        var pc = inkJSONs_PC[_questIndex];
        var controller = inkJSONs_Controller[_questIndex];

        if (_currentStateOfQuest.Equals(QuestState.REQUIREMENTS_NOT_MET))
        {
            pcJSON = pc.inkJSON_BeforeQuest;
            controllerJSON = controller.inkJSON_BeforeQuest;
        }
        if (_currentStateOfQuest.Equals(QuestState.CAN_START))
        {
            pcJSON = pc.inkJSON_StartQuest;
            controllerJSON = controller.inkJSON_StartQuest;
        }
        if (_currentStateOfQuest.Equals(QuestState.IN_PROGRESS))
        {
            pcJSON = pc.inkJSON_DuringQuest;
            controllerJSON = controller.inkJSON_DuringQuest;
        }
        if (_currentStateOfQuest.Equals(QuestState.CAN_FINISH))
        {
            pcJSON = pc.inkJSON_FinishQuest;
            controllerJSON = controller.inkJSON_FinishQuest;
        }
        if (_currentStateOfQuest.Equals(QuestState.FINISHED))
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
