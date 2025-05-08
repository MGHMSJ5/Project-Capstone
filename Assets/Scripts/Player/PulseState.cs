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
        // When the pulse is deactivated, go back to the idle state
        if (!player.PlayerPulse.IsPulseActive)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
        }
    }

    public void Exit()
    {

    }
}
