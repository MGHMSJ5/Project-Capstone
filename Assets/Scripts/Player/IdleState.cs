using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerController player;
    private Animator animator;

    public IdleState(PlayerController player)
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
        Debug.Log("Idle");
        animator.SetTrigger("IdleTrigger");
    }

    public void Execute()
    {
        // If player is no longer grounded, transition
        if (!player.Grounded)
        {
            // Transition to the jump state if the player has jumped
            if (!player.ReadyToJump)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }
            
        }

        // If the player is moving, then transition to the walking state
        if (player.Direction.magnitude > 0.1f)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
        }

        // If the player activates the pulse, then transition to the pulse state
        if (player.PlayerPulse.IsPulseActive)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.pulseState);
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("IdleTrigger");
    }
}
