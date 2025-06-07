using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// The MinorRepair script is required
[RequireComponent(typeof(MinorRepair))]

// This script is an example for a repair item. This script causes the 'result' of what happens when repairing
public class RepairGate : MonoBehaviour
{   // Always get a reference to the MinorRepair script
    private MinorRepair _minorRepair;
    private Collider _collider;

    [SerializeField]
    private GameObject _brokenGate;
    [SerializeField]
    private GameObject _fixedGate;
    [SerializeField]
    private Animator _animator;

    private void Awake()
    {
        _minorRepair = GetComponent<MinorRepair>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _collider.enabled = false;
        _fixedGate.SetActive(false);
    }

    private void OpenGate()
    {   // Destroy the broken gate and set the fixed gate to true and then play its animation
        Destroy(_brokenGate);
        _fixedGate.SetActive(true);
        _animator.Play("FenceGateDoorOpening");
    }

    //subscribe to event so that the subscribed function will be invoked when repairing
    private void OnEnable()
    {
        _minorRepair.RepairAction += OpenGate;
    }
    // Ubsubscribe to the event if this object gets destroyed to prevent errors
    private void OnDestroy()
    {
        _minorRepair.RepairAction -= OpenGate;
    }
}
