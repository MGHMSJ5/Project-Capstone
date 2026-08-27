using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettler22TImplementPhases : MonoBehaviour
{
    private WorldPhaseHandler worldPhaseHandler;

    private void Awake()
    {
        worldPhaseHandler = GetComponent<WorldPhaseHandler>();
        worldPhaseHandler.ChangeWorldPhase = PhaseChanged;
    }

    void PhaseChanged(string phase)
    {
        switch (phase)
        {
            case "Cold":
                print("it is cold");
                break;

            case "Warm":
                print("it is warm");
                break;
        }
    }
}
