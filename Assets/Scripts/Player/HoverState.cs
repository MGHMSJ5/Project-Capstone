using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverState : IState
{
    private PlayerController player;

    public HoverState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Hover");
        player.particleSystem.gameObject.SetActive(true);
        player.particleSystem.Play();
    }

    public void Execute()
    {
        // If the player is not hovering ↓
        if (!player.PlayerHover.IsHovering)
        {
            // If the player is not on the ground, then transition to the jump state
            if (!player.Grounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }
            else
            {   // If the player is on the ground, then transition to the walk state
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }
        }
    }

    public void Exit()
    {
        player.particleSystem.Stop();
        player.particleSystem.gameObject.SetActive(false);
    }
}
