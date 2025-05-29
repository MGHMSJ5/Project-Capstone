using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;

    private float timer = 0f;
    private float waitTime = 0.5f;
    private bool hasTriggered = false;
    public JumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Jump");

        ResetTimer();
    }

    public void Execute()
    {
        // else if (hovering stuff)
        if (player.PlayerHover.IsHovering)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.hoverState);
        }

        if (hasTriggered) return;

        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.fallingState);
            hasTriggered = true; // Prevent it from running again
        }
    }

    public void Exit()
    {
             
    }

    public void ResetTimer()
    {
        timer = 0f;
        hasTriggered = false;
    }
}
