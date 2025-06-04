using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBridge : MonoBehaviour
{
    private MinorRepair minorRepair;
    private CanvasSceneTransition _canvasSceneTransition;
    private Animator _animator;

    private void Awake()
    {
        minorRepair = GetComponent<MinorRepair>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        minorRepair.RepairAction += FixBridge;
    }

    private void OnDisable()
    {
        minorRepair.RepairAction -= FixBridge;
    }

    //Add that the queststep is finished when interacting with the bridge
    private void FixBridge()
    {
        _canvasSceneTransition.FadeAction += BridgeChange;
        _canvasSceneTransition.FadeAction += BridgeChange;
        _canvasSceneTransition.CanvasFadeInAndOut(2f);

        minorRepair.RepairAction -= FixBridge;
    }

    private void BridgeChange()
    {
        // Move Chobo to landing area
        ScriptedEvents.Instance.TeleportChoboToLandingArea();
        _animator.Play("CloseBridge");
    }
}
