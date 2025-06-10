using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ScriptedEvents : Singleton<ScriptedEvents>
{
    [Header("Tutorial variables")]
    private WaypointMovement _choboMovement;
    private BaseInteract _baseInteract;
    private Collider _collider;
    private NavMeshAgent _agent;

    [Header("Waterpump event")]
    [SerializeField]
    private Transform _kettleTeleportTransform;
    private GameObject _chobo;

    [Header("RepairBridge event")]
    [SerializeField]
    private Transform _landingAreaTeleportTransform;

    [Header("Plug in the plug event")]
    [SerializeField]
    private GameObject _cablePluggedIn;
    [SerializeField] 
    private GameObject _cableUnplugged;
    private CanvasSceneTransition _canvasSceneTransition;


    void Start()
    {
        _chobo = GameObject.Find("Chobo");
        _choboMovement = _chobo.GetComponent<WaypointMovement>();
        _baseInteract = _chobo.GetComponent<BaseInteract>();
        _collider = _chobo.GetComponent<Collider>();
        _agent = _chobo.GetComponent<NavMeshAgent>();
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _cablePluggedIn.SetActive(false);
        _cableUnplugged.SetActive(true);
    }
    // Tutorial functions
    public void MoveChobo()
    {
        _choboMovement.stopWalking = false;
        _baseInteract.enabled = false;
        _collider.enabled = false;
    }

    public void EnableScriptChobo()
    {
        _baseInteract.enabled = true;
        _collider.enabled = true;
    }

    public void StopChobo()
    {
        _choboMovement.stopWalking = true;
        _agent.enabled = false;
        _choboMovement.enabled = false;
    }
    // Waterpump functions
    public void TeleportChoboToKettle()
    {
        _chobo.transform.position = _kettleTeleportTransform.transform.position;
        _chobo.transform.rotation = _kettleTeleportTransform.rotation;
    }

    // Fix the bridge function
    public void TeleportChoboToLandingArea()
    {
        _chobo.transform.position = _landingAreaTeleportTransform.transform.position;
        _chobo.transform.rotation = _landingAreaTeleportTransform.rotation;
    }

    // Plug in the plug function
    public void PlugInThePlug()
    {
        //play scripted event - in scene fade in and out
        _canvasSceneTransition.FadeAction += PlugChangePosition;
        Debug.Log("Plug Changed!");
        _canvasSceneTransition.CanvasFadeInAndOut(2f);
    }

    private void PlugChangePosition()
    {
        _cableUnplugged.SetActive(false);
        _cablePluggedIn.SetActive(true);
    }
    // Workshop event
    public void GetHover()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHover>()._hoverAbilityGranted = true;
    }
}
