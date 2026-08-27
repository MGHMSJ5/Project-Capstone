using System;
using UnityEngine;
// Main script that directly changes the currentPhase values of SO and also invoked action
public class WorldPhaseHandler : MonoBehaviour
{
    [SerializeField]
    private WorldPhase worldPhases;

    public Action<string> ChangeWorldPhase;

    private void OnEnable()
    {
        ChangeWorldPhase?.Invoke(worldPhases.currentWorldPhase);
    }

    public void SetPhase(string phase)
    {
        if (!worldPhases.allWorldPhases.Contains(phase))
        {
            Debug.LogWarning($"Phase ' {phase}' does not exist.");
            return;
        }

        if (phase == worldPhases.currentWorldPhase)
        {
            Debug.LogWarning($"World is already set to '{phase}' phase.");
            return;
        }

        worldPhases.currentWorldPhase = phase;
        ChangeWorldPhase?.Invoke(worldPhases.currentWorldPhase);
    }
}
