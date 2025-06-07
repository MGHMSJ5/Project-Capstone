using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverState : IState
{
    private PlayerController player;
    private Animator animator;

    public HoverState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        //Debug.Log("Hover");
        animator.SetTrigger("HoverTrigger");
        player.particleSystem.gameObject.SetActive(true);
        player.particleSystem.Play();

        //Activate hover sound if hover is activated
        SoundManager.PlaySound(SoundType.HOVER, 1.5f);
    }

    public void Execute()
    {
        // If the player is not hovering ↓
        if (!player.PlayerHover.IsHovering)
        {
            // If the player is not on the ground, then transition to the falling state
            if (!player.Grounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
            }
            else
            {   // If the player is on the ground, then transition to the land state
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.landState);
            }
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("HoverTrigger");
        player.particleSystem.Stop();
        player.particleSystem.gameObject.SetActive(false);
    }
}
