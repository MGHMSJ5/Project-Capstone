using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseState : IState
{
    private PlayerController player;

    public PulseState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Pulse");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
