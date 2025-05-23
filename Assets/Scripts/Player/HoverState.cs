﻿using System.Collections;
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
        Debug.Log("Hover");

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
            {   // If the player is on the ground, then transition to the walk state
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
            }
        }
    }

    public void Exit()
    {

    }
}
