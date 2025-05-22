using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TalkToFactoryNPCStep : QuestStep
{
    private BaseInteract baseInteract;
    private CanvasSceneTransition _canvasSceneTransition;
    
    [SerializeField] private Vector3 pluggedPosition;

    private void Awake()
    {
        baseInteract = GameObject.Find("Oolo").GetComponent<BaseInteract>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        Debug.Log(_canvasSceneTransition);
    }

    private void OnEnable()
    {
        baseInteract.onSubmitPressed += PlayScriptedEvent;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= PlayScriptedEvent;
    }

    //Add that the queststep is finished when talking to NPC
    private void PlayScriptedEvent()
    {
        //play scripted event - in scene fade in and out
        _canvasSceneTransition.FadeAction += PlugChangePosition;
        _canvasSceneTransition.CanvasFadeInAndOut(2f);

        FinishQuestStep();
        baseInteract.onSubmitPressed -= PlayScriptedEvent;

        baseInteract.InvokeSubmitPressed();
    }

    private void PlugChangePosition()
    {
        GameObject.Find("PlugParent").transform.position = pluggedPosition;
    }
}
