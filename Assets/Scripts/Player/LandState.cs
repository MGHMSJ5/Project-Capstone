using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : IState
{
    private PlayerController player;
    private Animator animator;

    public LandState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        if (player._isCarrying)
        {
            animator.SetLayerWeight(1, 1f);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
        }
        //Debug.Log("Falling");
        animator.SetTrigger("LandTrigger");
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
        else
        {
            // Transition to the jump state if the player ha jumped
            if (!player.ReadyToJump)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("LandTrigger");
    }
}
