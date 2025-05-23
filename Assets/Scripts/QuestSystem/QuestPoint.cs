using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;
    [Header("Optional Second Quest")]
    [SerializeField] private QuestPoint secondQuest;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    [HideInInspector] public string questId;
    private QuestState currentQuestState;

    private BaseInteract _baseInteract;
    private NPCInteract _npcInteract;

    private QuestUI _questUI;
    private bool _startedQuestDialogue = false;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        _baseInteract = GetComponent<BaseInteract>();
        _npcInteract = GetComponent<NPCInteract>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        _baseInteract.onSubmitPressed += SubmitPressed;
        _questUI = GameObject.Find("Canvas").GetComponent<QuestUI>();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        _baseInteract.onSubmitPressed -= SubmitPressed;
    }

    private void Update()
    {   // If the player has started the quest UI, and the dialogue is finished
        if (_startedQuestDialogue && !_npcInteract.DialogueHasInteracted)
        {
            if (!questInfoForPoint.isSideQuest)
            {
                _questUI.StartQuestUI();
            }
            else
            {
                _questUI.StartSidequestUI();
            }
            _startedQuestDialogue = false;
        }
    }
    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        //start or finish quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);

            if (!questInfoForPoint.isSideQuest)
            {
                _questUI.displayNameUI = questInfoForPoint.displayName;
            }
            else
            {
                _questUI.displaySidequestNameUI = secondQuest.questInfoForPoint.displayName;
            }

            ShowQuestUI(true);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);

            ShowQuestUI(false);

            if (secondQuest != null)
            {
                string id = secondQuest.questId;
                GameEventsManager.instance.questEvents.StartQuest(id);

                if (!questInfoForPoint.isSideQuest)
                {
                    _questUI.displayNameUI = secondQuest.questInfoForPoint.displayName;
                }
                else
                {
                    _questUI.displaySidequestNameUI = secondQuest.questInfoForPoint.displayName;
                }

                ShowQuestUI(true);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
    private void ShowQuestUI(bool startedQuest)
    {
        if (questInfoForPoint.isSideQuest)
        {
            return;
        }

        if (startedQuest)
        {
            UIQuestStartHandling();
        }
        else
        {
            if (!questInfoForPoint.isSideQuest)
            {
                _questUI.FinishQuestUI();
            }
            else
            {
                _questUI.FinishSidequestUI();
            }
        }
    }
    private void UIQuestStartHandling()
    {
        // Check if the npc interact script is not null
        if (_npcInteract != null)
        {   // Check if the dialogue has started
            if (_npcInteract.DialogueHasInteracted)
            {   // Set that this dialogue has started
                _startedQuestDialogue = true;
            }
        }
        else
        {   // If there is not npc interact script, then make the UI appear
            if (!questInfoForPoint.isSideQuest)
            {
                _questUI.StartQuestUI();
            }
            else
            {
                _questUI.StartSidequestUI();
            }
        }
    }
}
