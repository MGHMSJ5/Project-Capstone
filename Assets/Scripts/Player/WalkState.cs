using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    private PlayerController player;
    private Animator animator;

    public WalkState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        Debug.Log("Walk");
        animator.SetTrigger("WalkingTrigger");
    }

    public void Execute()
    {
        // If player is no longer grounded, transition
        if (!player.Grounded)
        {
            // Transition to the jump state if the player has jumped
            if (!player.LetJumpGo)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }
            else //, the player is falling
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
            }
        }

        // If the player is not moving, then transition to the idle state
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
        animator.ResetTrigger("WalkingTrigger");
    }
}
