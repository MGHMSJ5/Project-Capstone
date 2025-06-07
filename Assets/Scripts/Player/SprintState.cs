using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : IState
{
    private PlayerController player;
    private Animator animator;

    public SprintState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>(); 
    }

    public void Enter()
    {
        Debug.Log("Sprint");
        animator.SetTrigger("SprintingTrigger");
    }

    public void Execute()
    {
        // If player is no longer grounded, transition
        if (!player.Grounded)
        {
            // Transition to the jump state if the player ha jumped
            if (!player.ReadyToJump)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }
            else //, the player is falling
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
            }
        }

        // If the player is moving, then transition to the idle state
        if (player.Direction.magnitude < 0.1f)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
        }

        // If the player is not sprinting, then transition to the walk state
        if (!player.IsSprinting)
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
        animator.ResetTrigger("SprintingTrigger");
        animator.ResetTrigger("LandTrigger");
    }
}
