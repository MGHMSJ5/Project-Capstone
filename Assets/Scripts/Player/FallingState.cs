using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : IState
{
    private PlayerController player;

    public FallingState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Falling");
    }

    public void Execute()
    {
        // If the player is on the ground, then ...
        if (player.Grounded)
        {
            //Activate fall sound if falling is activated
            SoundManager.PlaySound(SoundType.FALL, 0.5f);

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
        if (player.PlayerHover.IsHovering)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.hoverState);
        }
    }

    public void Exit()
    {

    }
}
