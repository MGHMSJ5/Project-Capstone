using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;
    private Animator animator;

    public JumpState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        //Debug.Log("Jump");
        animator.SetTrigger("JumpTrigger");
    }

    public void Execute()
    {
        // else if (hovering stuff)
        if (player.PlayerHover.IsHovering)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.hoverState);
        }

        // if grounded land
        if(player.Grounded)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.landState);
        }
        else
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("JumpTrigger");
        animator.ResetTrigger("LandTrigger");
    }
}
