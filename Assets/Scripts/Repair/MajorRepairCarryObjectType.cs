using UnityEngine;
// Put in here all the object types that can be carried
// They will be used for the Major repairs
public enum CarryTypes
{
    Plank,
    BigBolt
}

public class MajorRepairCarryObjectType : MonoBehaviour
{
    // Set whic type this carriable repair resource is
    public CarryTypes carryType;
}
