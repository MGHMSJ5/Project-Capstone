using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseState : IState
{
    private PlayerController player;
    private Animator animator;

    public PulseState(PlayerController player)
    {
        this.player = player;
        animator = player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        //Debug.Log("Pulse");
        animator.SetTrigger("PulseTrigger");

        //Activate pulse sound if pulse is activated
        SoundManager.PlaySound(SoundType.PULSE, 0.5f);
    }

    public void Execute()
    {
        // When the pulse is deactivated, go back to the idle state
        if (!player.PlayerPulse.IsPulseActive)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
        }
    }

    public void Exit()
    {
        animator.ResetTrigger("PulseTrigger");
    }
}
