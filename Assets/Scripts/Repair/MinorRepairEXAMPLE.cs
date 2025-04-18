using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// The MinorRepair script is required
[RequireComponent(typeof(MinorRepair))]

// This script is an example for a repair item. This script causes the 'result' of what happens when repairing
public class MinorRepairEXAMPLE : MonoBehaviour
{   // Always get a reference to the MinorRepair script
    private MinorRepair _minorRepair;

    [SerializeField]
    private GameObject _objectChangeColor;

    private void Awake()
    {
        _minorRepair = GetComponent<MinorRepair>();
    }

    private void ChangeColor()
    {   // Change the color of the object to green
        _objectChangeColor.GetComponent<Renderer>().material.color = Color.green;
    }

    //subscribe to event so that the subscribed function will be invoked when repairing
    private void OnEnable()
    {
        _minorRepair.RepairAction += ChangeColor;
    }
    // Ubsubscribe to the event if this object gets destroyed to prevent errors
    private void OnDestroy()
    {
        _minorRepair.RepairAction -= ChangeColor;
    }
}
