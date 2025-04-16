using UnityEngine;
using UnityEngine.UI;

public class MajorRepair : MinorRepair
{
    [SerializeField]
    private CarryTypes _acceptedObject;
    [SerializeField]
    private Sprite _carryObjectImage;

    protected override void Start()
    {
        base.Start();

        Transform resourceAmountObject = _canvas.transform.GetChild(1);
        Image carryObjectImage = resourceAmountObject.GetChild(1).GetComponent<Image>();
        carryObjectImage.sprite = _carryObjectImage;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        MajorRepairCarryObjectType majorRepairCarryObjectType = other.GetComponent<MajorRepairCarryObjectType>();

        if (majorRepairCarryObjectType != null)
        {
            if (majorRepairCarryObjectType.carryType == _acceptedObject)
            {
                print("object accepted!");
            }
        }
    }
}
