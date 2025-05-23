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

    [SerializeField]
    private GameObject _questStartedBox;
    [SerializeField]
    private TextMeshProUGUI _questStartedText;
    [SerializeField]
    private GameObject _questFinishedBox;
    [SerializeField]
    private TextMeshProUGUI _questFinishedText;

    [HideInInspector]
    public string displayNameUI;

    private void Awake()
    {
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(false);
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
}
