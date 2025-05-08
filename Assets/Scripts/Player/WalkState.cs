using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    private PlayerController player;

    public WalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Walk");
    }

    public void Execute()
    {
        // If player is no longer grounded, transition to the jump state
        if (!player.Grounded)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
        }

        // If the player is moving, then transition to the idle state
        if (player.Direction.magnitude < 0.1f)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
        }
        
        // If the player is sprinting, then transition to the sprint state
        if (player.IsSprinting)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.sprintState);
        }

        // If the player activates the pulse, then transition to the pulse state
        if (player.PlayerPulse.IsPulseActive)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.pulseState);
        }
    }

    public void Exit()
    {

    }
}
