using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The MinorRepair script is required
[RequireComponent(typeof(MinorRepair))]

// This script is an example for a repair item. This script causes the 'result' of what happens when repairing
public class MinorRepairWaterPump : MonoBehaviour
{   // Always get a reference to the MinorRepair script
    private MinorRepair _minorRepair;

    [SerializeField]
    private Animator _animator;

    private void Awake()
    {
        _minorRepair = GetComponent<MinorRepair>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void PlayAnimation()
    {
        _animator.SetBool("HasRepaired", true);
    }

    //subscribe to event so that the subscribed function will be invoked when repairing
    private void OnEnable()
    {
        _minorRepair.RepairAction += PlayAnimation;
    }
    // Ubsubscribe to the event if this object gets destroyed to prevent errors
    private void OnDestroy()
    {
        _minorRepair.RepairAction -= PlayAnimation;
    }
}
