using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryState : IState
{
    private PlayerController player;

    public CarryState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Carry");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
