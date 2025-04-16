using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// The MinorRepair script is required
[RequireComponent(typeof(MinorRepair))]
public class MinorRepairEXAMPLE : MonoBehaviour
{   // This is an example of a repair item. This script causes the 'result' of what happens when repairing
    private MinorRepair _minorRepair;

    [SerializeField]
    private GameObject _objectChangeColor;

    private void Awake()
    {
        _minorRepair = GetComponent<MinorRepair>();
    }

    private void ChangeColor()
    {
        _objectChangeColor.GetComponent<Renderer>().material.color = Color.green;
    }

    //subscribe to event so that the subscribes function will be invoked when repairing
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
