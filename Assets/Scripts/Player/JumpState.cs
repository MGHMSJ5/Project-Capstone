using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;
    private Animator animator;

    public JumpState(PlayerController player)
    {
        this.player = player;

        animator =
            player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        animator.SetTrigger(
            "JumpTrigger"
        );
    }

    public void Execute()
    {
        // Once the player leaves the ground,
        // enter the falling/airborne state.
        if (!player.Grounded)
        {
            player.PlayerStateMachine.TransitionTo(
                player.PlayerStateMachine.fallingState
            );
        }
    }

    public void Exit()
    {
        animator.ResetTrigger(
            "JumpTrigger"
        );
    }
}