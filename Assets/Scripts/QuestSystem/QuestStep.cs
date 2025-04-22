using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is supposed to be inherited by another script and not used alone, therefore it is an abstract class.
public abstract class QuestStep : MonoBehaviour
{
    [SerializeField] public string description;

    private bool isFinished = false;

    protected void FinsihQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            //TODO - Advance to next quest step

            Destroy(this.gameObject);
        }
    }
}
