using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    private QuestInfoSO _questInfoSO;
    private UICanvas _UICanvas;
    private QuestManager _questManager;
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

    public void StartQuestUI()
    {
        _questStartedBox.SetActive(true);
        _questFinishedBox.SetActive(false);
        _questStartedText.text = "Started Quest: " + displayNameUI;
        _animator.Play("QuestUIStartedPopup");
    }

    public void FinishQuestUI()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(true);
        _questFinishedText.text = "Finished Quest: " + displayNameUI;
        _animator.Play("QuestUIFinishedPopup");
    }

    public void StartSidequestUI()
    {
        _sideQuestStartedBox.SetActive(true);
        _sideQuestFinishedBox.SetActive(false);
        _sideQuestStartedText.text = "Started Sidequest: " + displaySidequestNameUI;
        _animator.Play("SidequestUIStartedPopup");
    }

    public void FinishSidequestUI()
    {
        _sideQuestStartedBox.SetActive(false);
        _sideQuestFinishedBox.SetActive(true);
        _sideQuestStartedText.text = "Finished Sidequest: " + displaySidequestNameUI;
        _animator.Play("SidequestUIFinishedPopup");
    }
}
