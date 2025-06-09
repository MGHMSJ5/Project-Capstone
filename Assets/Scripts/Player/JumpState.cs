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
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);

        if (player.Grounded)
        {
            if (player.Direction.magnitude > 0.1f)
            {
                if (player.IsSprinting)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.sprintState);
                }
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("JumpTrigger");
    }
}
