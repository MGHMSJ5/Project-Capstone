using System;
using UnityEngine;

public class Kettler22TPhaseHandler
{
    [SerializeField]
    private WorldPhase cold;
    [SerializeField]
    private WorldPhase warm;

    public static event Action<WorldPhase> ChangePhase;

    public static WorldPhase CurrentPhase {  get; private set; }

    public void ColdPhase()
    {
        SetPhase(cold);
    }


    public void WarmPhase()
    {
        SetPhase(warm);
    }
    public static void SetPhase(WorldPhase phase)
    {
        Debug.Log("Phase changed to " + phase);
        CurrentPhase = phase;
        ChangePhase?.Invoke(phase);
    }
}
