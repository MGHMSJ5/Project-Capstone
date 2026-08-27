using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettler22TImplementPhases : MonoBehaviour
{
    private WorldPhaseHandler worldPhaseHandler;

    [Header("Warm Phase")]
    [SerializeField]
    private List<ParticleSystem> particlesToturnOn = new List<ParticleSystem>();

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
                ChangeToCold();
                break;

            case "Warm":
                ChangeToWarm();
                break;
        }
    }

    void ChangeToCold()
    {
        print("it is cold");
    }

    void ChangeToWarm()
    {
        print("it is warm");
        StartCoroutine(WaitBetweenActivations());
    }
    IEnumerator WaitBetweenActivations()
    {
        StartKettleWater();

        yield return new WaitForSeconds(2f);

        WarmParticlesOn();
    }

    public void StartKettleWater()
    {
        print("water starts flowing");
    }

    public void WarmParticlesOn()
    {
        foreach (ParticleSystem particle in particlesToturnOn)
        {
            particle.Play();
        }
    }
}
