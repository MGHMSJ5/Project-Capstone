using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }

    // Reference to the state objects
    public IdleState idleState;
    public WalkState walkState;
    public SprintState sprintState;
    public JumpState jumpState;
    public FallingState fallingState;
    public HoverState hoverState;
    public PulseState pulseState;
    public LandState landState;

    // Event to notify other objects of the state change
    public event Action<IState> stateChanged;

    // Pass in the necessary parameters into constructor
    public PlayerStateMachine(PlayerController player)
    {
        this.idleState = new IdleState(player);
        this.walkState = new WalkState(player);
        this.sprintState = new SprintState(player);
        this.jumpState = new JumpState(player);
        this.fallingState = new FallingState(player);
        this.hoverState = new HoverState(player);
        this.pulseState = new PulseState(player);
        this.landState = new LandState(player);
    }

    // Set the starting state
    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

        // Notify the other objects that the state has changed
        stateChanged?.Invoke(state);
    }

    // Exit this state and enter another
    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

        // Notify other objects that the state has changed
        stateChanged?.Invoke(nextState);
    }

    // Allow the StateMachine to update this state
    public void Execute()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}
