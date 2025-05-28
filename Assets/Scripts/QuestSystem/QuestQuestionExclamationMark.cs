using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestQuestionExclamationMark : MonoBehaviour
{
    [SerializeField]
    private GameObject _questionMark;
    [SerializeField]
    private GameObject _exclamationMark;
    [SerializeField]
    private QuestPoint[] questPoints;

    public QuestState _state;

    void Awake()
    {
        questPoints = GetComponents<QuestPoint>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < questPoints.Length; i++)
        {
            questPoints[i].CanStartEvent.AddListener(ShowQuestionMark);
            questPoints[i].CanFinishEvent.AddListener(ShowExclamationMark);
            questPoints[i].StartQuestAfterDialogueEvent.AddListener(HideQuestionMark);
            questPoints[i].FinishQuestAfterDialogueEvent.AddListener(HideExclamationMark);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < questPoints.Length; i++)
        {
            questPoints[i].CanStartEvent.RemoveListener(ShowQuestionMark);
            questPoints[i].CanFinishEvent.RemoveListener(ShowExclamationMark);
            questPoints[i].StartQuestAfterDialogueEvent.RemoveListener(HideQuestionMark);
            questPoints[i].FinishQuestAfterDialogueEvent.RemoveListener(HideExclamationMark);
        }
    }

    void Update()
    {

    }

    public void ShowQuestionMark()
    {
        _questionMark.SetActive(true);
    }

    public void ShowExclamationMark()
    {
        _exclamationMark.SetActive(true);
    }

    public void HideQuestionMark()
    {
        _questionMark.SetActive(false);
    }

    public void HideExclamationMark()
    {
        _exclamationMark.SetActive(false);
    }
}




