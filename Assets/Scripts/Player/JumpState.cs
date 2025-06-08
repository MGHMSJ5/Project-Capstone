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
        if (player._isCarrying)
        {
            animator.SetLayerWeight(1, 1f);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
        }
        //Debug.Log("Jump");
        animator.SetTrigger("JumpTrigger");
    }

    public void Execute()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
    }

    public void Exit()
    {
        animator.ResetTrigger("JumpTrigger");
    }
}
