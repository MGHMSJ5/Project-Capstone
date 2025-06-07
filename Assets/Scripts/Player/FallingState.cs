using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : IState
{
    private PlayerController player;
    private Animator animator;

    public FallingState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>(); 
    }

    public void Enter()
    {
        //Debug.Log("Falling");
        animator.SetTrigger("FallingTrigger");
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
                    animator.Play("PATCH|LandingAnim");
                }
                else
                {   // Transition to the walk state if the player is not sprinting
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
                    animator.Play("PATCH|LandingAnim");
                }
            }
            // Transition to the idle state if the player is not moving
            else
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
                animator.Play("PATCH|LandingAnim");
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
        animator.ResetTrigger("FallingTrigger");
    }
}
