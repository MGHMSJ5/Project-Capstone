using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScriptedEvents : Singleton<ScriptedEvents>
{
    [Header("Tutorial variables")]
    private WaypointMovement _choboMovement;
    private BaseInteract _baseInteract;
    private Collider _collider;
    private NavMeshAgent _agent;
    private Animator _choboAnimator;

    [Header("Waterpump event")]
    private Transform _kettleTeleportTransform;
    private GameObject _chobo;

    [Header("RepairBridge event")]
    private Transform _landingAreaTeleportTransform;

    [Header("Plug in the plug event")]
    private GameObject _cablePluggedIn;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset all variables, and make GameObjects that will be used to check if the needed object is in the scene
        _choboAnimator = null;
        _kettleTeleportTransform = null;
        _landingAreaTeleportTransform = null;
        _cablePluggedIn = null;
        _cableUnplugged = null;
        GameObject choboModel = null;
        GameObject kettleTeleport = null;
        GameObject landingAreaTeleport = null;
        GameObject cablePluggedIn = null;
        GameObject cableUnplugged = null;
        // Check if the name of the scene is not LoadingScene and try to find Chobo's Model
        if (scene.name != "LoadubgScebe")
        {
            choboModel = GameObject.Find("Chobo Model");
            kettleTeleport = GameObject.Find("Chobo_Kettle_Teleport");
            landingAreaTeleport = GameObject.Find("Chobo_LandingArea_Teleport");
            cablePluggedIn = GameObject.Find("Plug & Cable Plugged In");
            cableUnplugged = GameObject.Find("Plug & Cable Unplugged");
        }

        if (choboModel != null)
        {
            _choboAnimator = choboModel.GetComponent<Animator>();
        }
        if (kettleTeleport != null)
        {
            _kettleTeleportTransform = kettleTeleport.GetComponent<Transform>();
        }
        if (landingAreaTeleport != null)
        {
            _landingAreaTeleportTransform = landingAreaTeleport.GetComponent<Transform>();
        }
        if (cablePluggedIn != null)
        {
            _cablePluggedIn = cablePluggedIn;
        }
        if (cableUnplugged != null)
        {
            _cableUnplugged = cableUnplugged;
        }
    }

    private void Update()
    {
        if (_choboAnimator != null && _choboAnimator.GetCurrentAnimatorStateInfo(0).IsName("NPC_Walking") && _choboMovement.stopWalking)
        {
            StopChoboAnimation();
        }
    }
    // Tutorial functions
    public void MoveChobo()
    {
        _choboAnimator.SetBool("IsWalking", true);
        _choboMovement.stopWalking = false;
        _baseInteract.enabled = false;
        _collider.enabled = false;
    }

    private void StopChoboAnimation()
    {
        _choboAnimator.SetBool("IsWalking", false);
    }

    public void EnableScriptChobo()
    {
        _baseInteract.enabled = true;
        _baseInteract.onSubmitPressed += StopChoboAnimation;
        _collider.enabled = true;
    }

    public void StopChobo()
    {   
        _choboMovement.stopWalking = true;
        _agent.enabled = false;
        _choboMovement.enabled = false;
        _baseInteract.onSubmitPressed -= StopChoboAnimation;
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
