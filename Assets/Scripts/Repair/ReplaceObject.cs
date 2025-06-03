using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinorRepair))]
public class ReplaceObject : MonoBehaviour
{
    private MinorRepair _minorRepair;

    [SerializeField]
    private GameObject _brokenObject;
    [SerializeField]
    private GameObject _fixedObject;

    private void Awake()
    {
        _minorRepair = GetComponent<MinorRepair>();
    }

    private void OnEnable()
    {
        _minorRepair.RepairAction += ReplaceBrokenObject;
    }
    private void OnDisable()
    {
        _minorRepair.RepairAction -= ReplaceBrokenObject;
    }

    private void ReplaceBrokenObject()
    {
        _brokenObject.SetActive(false);
        _fixedObject.SetActive(true);
    }
}
