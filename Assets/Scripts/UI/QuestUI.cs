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

    private void Awake()
    {
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(false);
        _sideQuestStartedBox.SetActive(false);
        _sideQuestFinishedBox.SetActive(false);
    }

    private void Update()
    {

    }

    public void StartQuestAfterDialogue(QuestInfoSO questInfo)
    {
        if (questInfo.isSideQuest)
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
                if (npcInteract.DialogueHasInteracted && startedQuest)
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
    }

    private void FinishQuestUI()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(true);
        _questFinishedText.text = "Finished Quest: " + displayNameUI;
        _animator.Play("QuestUIFinishedPopup");
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
