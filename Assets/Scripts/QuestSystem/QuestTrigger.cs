using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public Quest quest;
    public string stepID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && quest.IsStepActive(stepID))
        {
            quest.CompleteStep(stepID);
            gameObject.SetActive(false); // Optional: disable trigger after use
        }
    }
}
