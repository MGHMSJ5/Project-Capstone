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

            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.landState);
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
