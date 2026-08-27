using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kettler22TCamera : MonoBehaviour
{
    private EventsSceneManager eventsSceneManager;
    private Animator animator;

    private void Awake()
    {
        eventsSceneManager = GameObject.Find("EventsSceneManager").GetComponent<EventsSceneManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartAnimation("KettleActivate");
        }
    }

    public void StartAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public void ChangePhaseToWarm()
    {
        eventsSceneManager.WorldPhaseWarm();
    }

    public void DisableCamera()
    {
        gameObject.SetActive(false);
    }
}
