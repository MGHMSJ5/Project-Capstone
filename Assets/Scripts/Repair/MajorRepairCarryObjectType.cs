using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CarryTypes
{
    Plank,
    BigBolt
}

public class MajorRepairCarryObjectType : MonoBehaviour
{
    // Put in here all the object types that can be carried
    // They will be used for the Major repairs

    public CarryTypes carryType;
}
