using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : IState
{
    private PlayerController player;

    public SprintState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Sprint");
    }

    public void Execute()
    {
        // If the player is not sprinting, then transition to the walk state
        if (!player.IsSprinting)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
        }

        // If player is no longer grounded, transition to the jump state
        if (!player.Grounded)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
        }

        // If the player is hovering, then go to the hover state
    }

    public void Exit()
    {

    }
}
