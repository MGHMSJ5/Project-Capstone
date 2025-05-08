using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;

    public JumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Jump");
    }

    public void Execute()
    {
        // If the player is on the ground, then ...
        if (player.Grounded)
        {
            // Check if the player is moving
            if (player.Direction.magnitude > 0.1f)
            {   // Transition to the sprint state when sprinting
                if (player.IsSprinting)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.sprintState);
                }
                else
                {   // Transition to the walk state if the player is not sprinting
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
                }
            }
            // Transition to the idle state if the player is not moving
            else
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
            }
        }
        // else if (hovering stuff)
    }

    public void Exit()
    {

    }
}
