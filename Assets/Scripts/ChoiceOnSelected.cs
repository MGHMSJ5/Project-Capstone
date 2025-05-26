using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceOnSelected : MonoBehaviour
{
    [SerializeField] public GameObject choiceIdicator;
    [SerializeField] public Button targetButton;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == targetButton.gameObject)
        {
            choiceIdicator.SetActive(true);
        }
        else
        {
            choiceIdicator.SetActive(false);
        }   
    }
}
