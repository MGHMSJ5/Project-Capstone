using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBridgeStep : QuestStep
{
    private BaseInteract baseInteract;
    private MinorRepair minorRepair;
    private CanvasSceneTransition _canvasSceneTransition;

    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private Quaternion finalRotation;

    private void Awake()
    {
        baseInteract = GameObject.Find("LandingAreaBridge").GetComponent<BaseInteract>();
        minorRepair = GameObject.Find("LandingAreaBridge").GetComponent<MinorRepair>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
    }
    private void OnEnable()
    {
        baseInteract.onSubmitPressed += FixBridge;
        Invoke("CheckIfDone", 1f);
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

        FinishQuestStep();
        baseInteract.onSubmitPressed -= FixBridge;

        baseInteract.InvokeSubmitPressed();
    }

    private void BridgeChange()
    {
        GameObject.Find("LandingAreaBridge").transform.position = finalPosition;
        GameObject.Find("LandingAreaBridge").transform.rotation = finalRotation;
    }

    private void CheckIfDone()
    {
        print("Function was called. Did it work?");
        if (minorRepair.HasBeenRepaired)
        {
            FinishQuestStep();
        }
        print("Hell yeah, it worked!");
    }
}
