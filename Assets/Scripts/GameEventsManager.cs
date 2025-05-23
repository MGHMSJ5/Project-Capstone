using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameEventsManager : MonoBehaviour
{
   public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public RewardEvents rewardEvents;
    public event Action toolboxOpened;

   private void Awake()
   {
        if(instance != null)
        {
            Debug.LogError("More than one Game Events Manager in the scene.");
        }
        instance = this;

        //initialise all events
        questEvents = new QuestEvents();
        rewardEvents = new RewardEvents();
   }

    public void InvokeToolboxEvent()
    {
        toolboxOpened?.Invoke();
    }
}
