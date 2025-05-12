using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    // Runs when entering the state
    public void Enter()
    {
        
    }

    // "Pre-frame logic, inclue condition to transition to a new state"
    public void Execute()
    {

    }

    // Runs when exiting the state
    public void Exit()
    {

    }
}
