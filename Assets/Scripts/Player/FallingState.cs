using UnityEngine;

public class FallingState : IState
{
    private PlayerController player;
    private Animator animator;

    public FallingState(PlayerController player)
    {
        this.player = player;

        animator =
            player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        animator.SetTrigger(
            "FallingTrigger"
        );
    }

    public void Execute()
    {
        // Hover takes priority while airborne.
        if (player.PlayerHover.IsHovering)
        {
            player.PlayerStateMachine.TransitionTo(
                player.PlayerStateMachine.hoverState
            );

            return;
        }

        // Player landed.
        if (player.Grounded)
        {
            SoundManager.PlaySound(
                SoundType.FALL,
                0.5f
            );

            player.PlayerStateMachine.TransitionTo(
                player.PlayerStateMachine.landState
            );
        }
    }

    public void Exit()
    {
        animator.ResetTrigger(
            "FallingTrigger"
        );
    }
}