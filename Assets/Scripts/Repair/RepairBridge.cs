using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBridge : MonoBehaviour
{
    private BaseInteract baseInteract;
    private CanvasSceneTransition _canvasSceneTransition;

    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private Quaternion finalRotation;

    private void Awake()
    {
        baseInteract = GameObject.Find("LandingAreaBridge").GetComponent<BaseInteract>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
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

        GameObject.Find("LandingAreaBridge").transform.position = finalPosition;
        GameObject.Find("LandingAreaBridge").transform.rotation = finalRotation;
    }
}
