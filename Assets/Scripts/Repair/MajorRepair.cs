using UnityEngine;
using UnityEngine.UI;

public class MajorRepair : MinorRepair
{
    [SerializeField]
    private CarryTypes _acceptedObject;
    [SerializeField]
    private Sprite _acceptedObjectImage;

    private MajorRepairCarryObjectType _majorRepairCarryObjectType;

    private bool _majorResourceIn = false;

    protected override void Start()
    {
        base.Start();

        SetMajorResourceUI();
    }

    protected override void Repair()
    {
        base.Repair();
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
