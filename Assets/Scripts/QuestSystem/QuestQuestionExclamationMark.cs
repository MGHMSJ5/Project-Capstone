using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestQuestionExclamationMark : MonoBehaviour
{
    [SerializeField]
    private GameObject _questionMark;
    [SerializeField]
    private GameObject _exclamationMark;

    public QuestState _state;

    void Start()
    {

    }

    void Update()
    {
        if (_state == QuestState.CAN_START)
        {
            ShowQuestionMark();
        }

        if (_state == QuestState.CAN_FINISH)
        {
            ShowExclamationMark();
        }
    }

    private void ShowQuestionMark()
    {
            print(_state);
            _questionMark.SetActive(true);
    }

    private void ShowExclamationMark()
    {
            print(_state);
            _exclamationMark.SetActive(true);
    }
}




