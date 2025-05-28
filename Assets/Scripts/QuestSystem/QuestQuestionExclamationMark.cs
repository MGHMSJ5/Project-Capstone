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

    private Renderer _exclamationMarkRenderer;

    private Color _baseColor;

    public QuestState _state;

    void Awake()
    {
        questPoints = GetComponents<QuestPoint>();
        _exclamationMarkRenderer = _exclamationMark.transform.GetChild(0).GetComponent<Renderer>();
        _baseColor = _exclamationMarkRenderer.material.color;
    }

    private void OnEnable()
    {
        for (int i = 0; i < questPoints.Length; i++)
        {
            questPoints[i].CanStartEvent.AddListener(ShowQuestionMark);
            questPoints[i].CanFinishEvent.AddListener(ShowExclamationMark);
            questPoints[i].StartQuestAfterDialogueEvent.AddListener(HideQuestionMark);
            questPoints[i].FinishQuestAfterDialogueEvent.AddListener(HideExclamationMark);

            questPoints[i].QuestInProgressionAction = ShowTransparentExclamationMark;
        }
    }

    public void ShowQuestionMark()
    {
        _questionMark.SetActive(true);
    }

    public void ShowExclamationMark()
    {
        _exclamationMark.SetActive(true);
        _exclamationMarkRenderer.material.color = _baseColor;
    }

    public void HideQuestionMark()
    {
        _questionMark.SetActive(false);
    }

    public void HideExclamationMark()
    {
        _exclamationMark.SetActive(false);
    }

    public void ShowTransparentExclamationMark()
    {
        _exclamationMark.SetActive(true);
        _exclamationMarkRenderer.material.color = Color.white;
    }
}




