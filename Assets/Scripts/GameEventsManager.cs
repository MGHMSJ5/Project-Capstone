using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEventsManager : MonoBehaviour
{
   public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public BaseInteract baseInteract;
    //public RewardEvents rewardEvents;

   private void Awake()
   {
        if(instance != null)
        {
            Debug.LogError("More than one Game Events Manager in the scene.");
        }
        instance = this;

        //initialise all events
        questEvents = new QuestEvents();
        baseInteract = new BaseInteract();
        //rewardEvents = new RewardEvents();
   }
}
