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
        Debug.Log("Hover");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
