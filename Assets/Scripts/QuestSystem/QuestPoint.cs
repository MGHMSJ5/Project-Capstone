using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    public QuestInfoSO questInfoForPoint;
    [Header("Optional Second Quest")]
    [SerializeField] private QuestPoint secondQuest;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    [HideInInspector] public string questId;
    [HideInInspector]
    public QuestState currentQuestState;

    private BaseInteract _baseInteract;
    private NPCInteract _npcInteract;

    private QuestUI _questUI;
    [HideInInspector]
    public bool startedQuestDialogue = false;
    [HideInInspector]
    public bool finishedQuestDialgue = false;

    [Header("Events for if the Quest can be started or finished")]
    public UnityEvent CanStartEvent;
    public UnityEvent CanFinishEvent;
    private bool _hasInvoked = false; // Bool that will keep track if one of the Can...Events have been invoked so that it only happens once

    [Header("Events at start and end")]
    public UnityEvent StartQuestEvent;
    public UnityEvent StartQuestAfterDialogueEvent;
    public UnityEvent FinishQuestEvent;
    public UnityEvent FinishQuestAfterDialogueEvent;

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
        _questUI = GameObject.Find("QuestUI").GetComponent<QuestUI>();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        _baseInteract.onSubmitPressed -= SubmitPressed;
    }

    private void Update()
    {   // If the player has started the quest UI, and the dialogue is finished
        if (startedQuestDialogue && !_npcInteract.DialogueHasInteracted)
        {
            StartQuestAfterDialogueEvent?.Invoke();
            _questUI.StartQuestAfterDialogue(questInfoForPoint);
            startedQuestDialogue = false;
        }
        // If the player has finished the quest UI, and the dialogue is finished
        if (finishedQuestDialgue && !_npcInteract.DialogueHasInteracted)
        {
            FinishQuestAfterDialogueEvent?.Invoke();
            finishedQuestDialgue = false;
        }

        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint && !_hasInvoked)
        {
            _hasInvoked = true;
            CanStartEvent?.Invoke();
        }
        if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint && !_hasInvoked)
        {
            _hasInvoked = true;
            CanFinishEvent?.Invoke();
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
            StartQuestEvent?.Invoke();

            _questUI.ChangeQuestDisplayName(questInfoForPoint);
            _questUI.ShowQuestUI(true, this, _npcInteract);
            
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
            FinishQuestEvent?.Invoke();

            _questUI.ChangeQuestDisplayName(questInfoForPoint);
            _questUI.ShowQuestUI(false, this, _npcInteract);

            if (secondQuest != null)
            {
                string id = secondQuest.questId;
                GameEventsManager.instance.questEvents.StartQuest(id);
                secondQuest.StartQuestEvent?.Invoke();

                _questUI.ChangeQuestDisplayName(secondQuest.questInfoForPoint);
                _questUI.ShowQuestUI(true, secondQuest, secondQuest.GetComponent<NPCInteract>());
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            _hasInvoked = false;
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
}
