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
    private TextMeshProUGUI _questIdleTitle;
    [SerializeField]
    private TextMeshProUGUI _questIdleDescription;

    [Header("Sidequests")]
    [SerializeField]
    private GameObject _sideQuestStartedBox;
    [SerializeField]
    private TextMeshProUGUI _sideQuestStartedText;
    [SerializeField]
    private GameObject _sideQuestFinishedBox;
    [SerializeField]
    private TextMeshProUGUI _sideQuestFinishedText;

    [Header("Quest Steps")]
    [SerializeField]
    private GameObject _questStepBox;
    [SerializeField]
    private TextMeshProUGUI _questStepTitle;
    [SerializeField]
    private TextMeshProUGUI _questStepDescription;
    [SerializeField]
    private GameObject _sideQuestStepBox;
    [SerializeField]
    private TextMeshProUGUI _sideQuestStepTitle;
    [SerializeField]
    private TextMeshProUGUI _sideQuestStepDescription;

    [HideInInspector]
    public string displayNameUI;
    [HideInInspector]
    public string displaySidequestNameUI;

    PlayerController _playerController;

    private float counter = 0.0f;
    private float maxWaitTime = 3.0f;
    private bool counterPassed = false;

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
        _questStepBox.SetActive(false);
        _questIdleTitle.text = "";
    }

    private void Update()
    {
        //icheck if the player is idle and not in dialogue and the quest step Ui popup is not doing its animation
        if (_playerController.PlayerStateMachine.CurrentState == _playerController.PlayerStateMachine.idleState && !DialogueManager.GetInstance().dialogueIsPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("QuestStepUIPopup") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("SidequestStepUIPopup"))
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
        if (_questIdleTitle.text == "")
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

    public void StepQuestUI(string title, string description, bool isFirstStep)
    {
        _questStepBox.SetActive(true);
        _questStepTitle.text = title;
        _questStepDescription.text = description;

        _questIdleTitle.text = _questStepTitle.text;
        _questIdleDescription.text = _questStepDescription.text;

        if(!isFirstStep)
        {
            _animator.Play("QuestStepUIPopup");
        }
    }

    private void StartQuestUI()
    {
        _questStartedBox.SetActive(true);
        _questFinishedBox.SetActive(false);
        _questStartedText.text = "Started Quest: " + displayNameUI;
        _animator.Play("QuestUIStartedPopup");
    }

    private void FinishQuestUI()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(true);
        _questFinishedText.text = "Finished Quest: " + displayNameUI;
        _animator.Play("QuestUIFinishedPopup");

        _questIdleTitle.text = "";
    }

    public void StepSidequestUI(string title, string description, bool isFirstStep)
    {
        _sideQuestStepBox.SetActive(true);
        _sideQuestStepTitle.text = title;
        _sideQuestStepDescription.text = description;

        if (!isFirstStep)
        {
            _animator.Play("SidequestStepUIPopup");
        }
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
