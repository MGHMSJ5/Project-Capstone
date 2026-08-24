using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPhases : MonoBehaviour
{
    private void OnEnable()
    {
        //Kettler22TPhaseHandler.ChangePhase += PhaseChanged;
    }

    private void OnDisable()
    {
        //Kettler22TPhaseHandler.ChangePhase -= PhaseChanged;
    }

    //void PhaseChanged(Kettler22TPhase phase)
    //{
    //    switch (phase)
    //    {
    //        case Kettler22TPhase.Cold:
    //            {
    //                Cold();
    //                break;
    //            }
    //        case Kettler22TPhase.Warm:
    //            {
    //                Warm();
    //                break;
    //            }
    //    }
    //}

    void Cold()
    {
        print("TESTING COLD");
    }

    void Warm()
    {
        print("TESTING WARM");
    }
}
