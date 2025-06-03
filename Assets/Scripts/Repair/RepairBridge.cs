using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBridge : MonoBehaviour
{
    private BaseInteract baseInteract;
    private CanvasSceneTransition _canvasSceneTransition;
    private Animator _animator;

    private void Awake()
    {
        baseInteract = GetComponent<BaseInteract>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBridge;
    }

    private void OnDisable()
    {
        baseInteract.onSubmitPressed -= FixBridge;
    }

    //Add that the queststep is finished when interacting with the bridge
    private void FixBridge()
    {
        _canvasSceneTransition.FadeAction += BridgeChange;
        _canvasSceneTransition.FadeAction += BridgeChange;
        _canvasSceneTransition.CanvasFadeInAndOut(2f);

        //FinishQuestStep();
        baseInteract.onSubmitPressed -= FixBridge;

        baseInteract.InvokeSubmitPressed();
    }

    private void BridgeChange()
    {
        // Move Chobo to landing area
        ScriptedEvents.Instance.TeleportChoboToLandingArea();
        _animator.Play("CloseBridge");
    }
}
