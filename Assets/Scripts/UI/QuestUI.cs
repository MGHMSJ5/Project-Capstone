using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    private UICanvas _UICanvas;
    private QuestManager _questManager;

    [SerializeField]
    private GameObject _questStartedBox;
    [SerializeField]
    private GameObject _questFinishedBox;

    private void Awake()
    {
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>();
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
    }

    public void FinishQuestUI()
    {
        _questStartedBox.SetActive(false);
        _questFinishedBox.SetActive(true);
    }
}
