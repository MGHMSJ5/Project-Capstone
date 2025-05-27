using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    private UICanvas _UICanvas;
    private Animator _animator;

    [Header("Main Quests")]
    [SerializeField]
    private GameObject _questStartedBox;
    [SerializeField]
    private TextMeshProUGUI _questStartedText;
    [SerializeField]
    private GameObject _questFinishedBox;
    [SerializeField]
    private TextMeshProUGUI _questFinishedText;
    [SerializeField]
    private GameObject _questIdleBox;
    [SerializeField]
    private TextMeshProUGUI _questIdleText;

    [Header("Sidequests")]
    [SerializeField]
    private GameObject _sideQuestStartedBox;
    [SerializeField]
    private TextMeshProUGUI _sideQuestStartedText;
    [SerializeField]
    private GameObject _sideQuestFinishedBox;
    [SerializeField]
    private TextMeshProUGUI _sideQuestFinishedText;

    [HideInInspector]
    public string displayNameUI;
    [HideInInspector]
    public string displaySidequestNameUI;

    PlayerController _playerController;

    private float counter = 0.0f;
    private float maxWaitTime = 5.0f;
    private bool counterPassed = false;

    public GameObject currentQuest = null;

    private void Awake()
    {
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>();
        _animator = GetComponent<Animator>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Start()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(false);
        _sideQuestStartedBox.SetActive(false);
        _sideQuestFinishedBox.SetActive(false);
        _questIdleBox.SetActive(false);
    }

    private void Update()
    {
        //icheck if the player is idle and not in dialogue
        if (_playerController.PlayerStateMachine.CurrentState == _playerController.PlayerStateMachine.idleState && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (counter < maxWaitTime)
            {
                counter += Time.deltaTime;
            }
        }
        else
        //reset the timer if idle is interrupted or player enters dialogue
        {
            counter = 0.0f;
            counterPassed = false;
            ShowQuestUIIdle(false);
        }
        //check if the counter has passd the time and active the idle UI
        if (counter >  maxWaitTime && !counterPassed)
        {
            counterPassed = true;
            ShowQuestUIIdle(true);
            
        }
    }

    public void ShowQuestUIIdle(bool appear)
    {
        if (_questIdleText.text == "")
        {
            return;
        }
        //Display the idle popup
        if (appear)
        {
            _questIdleBox.SetActive(true);
            
            _animator.SetBool("IsIdle", true);
            _animator.Play("QuestUIShowIdlePopup");
        }
        else
        {
            _animator.SetBool("IsIdle", false);
        }
    }

    public void StartQuestAfterDialogue(QuestInfoSO questInfo)
    {
        if (!questInfo.isSideQuest)
        {
            StartQuestUI();
        }
        else
        {
            StartSidequestUI();
        }
    }

    public void ChangeQuestDisplayName(QuestInfoSO questInfo)
    {
        if (!questInfo.isSideQuest)
        {
            displayNameUI = questInfo.displayName;
        }
        else
        {
            displaySidequestNameUI = questInfo.displayName;
        }
    }
  
    public void ShowQuestUI(bool startedQuest, QuestPoint questPoint, NPCInteract npcInteract)
    {
        if (startedQuest)
        {
            // Check if the npc interact script is not null
            if (npcInteract != null)
            {   // Check if the dialogue has started
                if (npcInteract.DialogueHasInteracted)
                {   // Set that this dialogue has started
                    questPoint.startedQuestDialogue = true;
                }
                else if (npcInteract.DialogueHasInteracted && !startedQuest)
                {
                    questPoint.finishedQuestDialgue = true;
                }
            }
            else
            {
                if (!questPoint.questInfoForPoint.isSideQuest)
                {
                    StartQuestUI();
                }
                else
                {
                    StartSidequestUI();
                }
            }
        }
        else
        {
            // Check if the npc interact script is not null
            if (npcInteract != null)
            {   // Check if the dialogue has started
                if (npcInteract.DialogueHasInteracted)
                {
                    questPoint.finishedQuestDialgue = true;
                }
            }
            if (!questPoint.questInfoForPoint.isSideQuest)
            {
                FinishQuestUI();
            }
            else
            {
                FinishSidequestUI();
            }
        }
    }

    private void StartQuestUI()
    {
        _questStartedBox.SetActive(true);
        _questFinishedBox.SetActive(false);
        _questStartedText.text = "Started Quest: " + displayNameUI;
        _animator.Play("QuestUIStartedPopup");

        _questIdleText.text = "Currently Active: " + displayNameUI;
    }

    private void FinishQuestUI()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(true);
        _questFinishedText.text = "Finished Quest: " + displayNameUI;
        _animator.Play("QuestUIFinishedPopup");

        _questIdleText.text = "";
    }

    private void StartSidequestUI()
    {
        _sideQuestStartedBox.SetActive(true);
        _sideQuestFinishedBox.SetActive(false);
        _sideQuestStartedText.text = "Started Sidequest: " + displaySidequestNameUI;
        _animator.Play("SidequestUIStartedPopup");
    }

    private void FinishSidequestUI()
    {
        _sideQuestStartedBox.SetActive(false);
        _sideQuestFinishedBox.SetActive(true);
        _sideQuestFinishedText.text = "Finished Sidequest: " + displaySidequestNameUI;
        _animator.Play("SidequestUIFinishedPopup");
    }
}
