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

    private void Awake()
    {
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>();
    }
    private void Start()
    {
        _questStartedBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _questStartedBox.SetActive(true);
        }
    }
}
