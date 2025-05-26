using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedEvents : Singleton<ScriptedEvents>
{
    [Header("Tutorial variables")]
    private WaypointMovement _choboMovement;
    private BaseInteract _baseInteract;
    private Collider _collider;

    void Start()
    {
        GameObject chobo = GameObject.Find("Chobo");
        _choboMovement = chobo.GetComponent<WaypointMovement>();
        _baseInteract = chobo.GetComponent<BaseInteract>();
        _collider = chobo.GetComponent<Collider>();
    }


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
    }
}
