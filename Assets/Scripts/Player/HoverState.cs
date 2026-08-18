using UnityEngine;

public class HoverState : IState
{
    private PlayerController player;
    private Animator animator;

    public HoverState(PlayerController player)
    {
        this.player = player;

        animator =
            player.GetComponentInChildren<Animator>();
    }

    public void Enter()
    {
        animator.SetTrigger(
            "HoverTrigger"
        );

        // This is the SAME particle system
        // that the original version used.
        //
        // Assign it in PlayerController's
        // "Particle System" Inspector field.
        if (player.ParticleSystem != null)
        {
            player.ParticleSystem.gameObject.SetActive(
                true
            );

            player.ParticleSystem.Play();
        }

        SoundManager.PlaySound(
            SoundType.HOVER,
            0.7f
        );
    }

    public void Execute()
    {
        // Hover has ended.
        if (!player.PlayerHover.IsHovering)
        {
            // Still airborne.
            if (!player.Grounded)
            {
                player.PlayerStateMachine.TransitionTo(
                    player.PlayerStateMachine.fallingState
                );

                return;
            }

            // Grounded and moving.
            if (player.Direction.magnitude >
                0.1f)
            {
                if (player.IsSprinting)
                {
                    player.PlayerStateMachine.TransitionTo(
                        player.PlayerStateMachine.sprintState
                    );
                }
                else
                {
                    player.PlayerStateMachine.TransitionTo(
                        player.PlayerStateMachine.walkState
                    );
                }
            }
            else
            {
                player.PlayerStateMachine.TransitionTo(
                    player.PlayerStateMachine.landState
                );
            }
        }
    }

    public void Exit()
    {
        SoundManager.StopSound();

        animator.ResetTrigger(
            "HoverTrigger"
        );

        // Turn off hover VFX.
        if (player.ParticleSystem != null)
        {
            player.ParticleSystem.Stop();

            player.ParticleSystem.gameObject.SetActive(
                false
            );
        }
    }
}