using UnityEngine;
using UnityEngine.UI;
// Script inherits from MinorRepair, since the repair is similar, but builds a bit more on it
public class MajorRepair : MinorRepair
{
    // CarryTypes is created in MajorRepairCarryObjectType
    [Tooltip("Set this to the type of object that is needed to repair this object.")]
    [SerializeField]
    private CarryTypes _acceptedObject;
    [Tooltip("Put the sprite that needs to be displayed on the UI to represent the carriable object needed here.")]
    [SerializeField]
    private Sprite _acceptedObjectImage;
    
    // Is used to reference the repair object that is in the trigger, and then used to delete said object when repairing
    private MajorRepairCarryObjectType _majorRepairCarryObjectType;
    // Is used to keep track if the right repair source is in the trigger
    private bool _majorResourceIn = false;

    protected override void Start()
    {
        base.Start();
        // Also set the UI of the other repair resource (on top of the collectible resources)
        SetMajorResourceUI();
    }

    protected override void Repair()
    {
        base.Repair();
        // + destroy the carriable resource
        Destroy(_majorRepairCarryObjectType.gameObject);
    }

    protected override bool CanRepair()
    {
        // Check for if the player has enough repair resources compared to what is needed
        for (int i = 0; i < _repairTypeAmount.Count; i++)
        {
            if (_repairTypeAmount[i].currentAmount >= _repairTypeAmount[i].amount)
            {
                continue;
            }
            else
            {
                // Return if the player doesnt have enough resources
                return false;
            }
        }
        // Return true or false depending on if the major repair source is also in the collider
        return _majorResourceIn;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // + check for if an object that is a repairsource has entered, and check if the type matches with the needed type to repair
        if ( _majorRepairCarryObjectType == null)
        {
            _majorRepairCarryObjectType = other.GetComponent<MajorRepairCarryObjectType>();
            if (_majorRepairCarryObjectType != null)
            {
                if (_majorRepairCarryObjectType.carryType == _acceptedObject)
                {
                    _majorResourceIn = true;
                }
            }
        }        
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        // If the needed repair source has left the trigger, then reset the related variables
        // NOTE: if multiple carriable objects are needed, or of multiple of the same carriable type exists, then the way this works needs to be changed!
        MajorRepairCarryObjectType exitedObject = other.GetComponent<MajorRepairCarryObjectType>();
        if (exitedObject != null && exitedObject == _majorRepairCarryObjectType)
        {
            if (_majorRepairCarryObjectType.carryType == _acceptedObject)
            {
                _majorResourceIn = false;
                _majorRepairCarryObjectType = null;
            }
        }

        
    }

    private void SetMajorResourceUI()
    {
        Transform resourceAmountObject = _canvas.transform.GetChild(1);
        Image carryObjectImage = resourceAmountObject.GetChild(1).GetComponent<Image>();
        carryObjectImage.sprite = _acceptedObjectImage;
    }
}
