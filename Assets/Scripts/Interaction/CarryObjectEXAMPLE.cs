
using UnityEngine;

public class CarryObjectEXAMPLE : BaseInteract
{
    [SerializeField]
    private Collider _objectCollider; // Set in the prefab
    private Transform _playerCarryPoint;
    private bool _isCarrying = false;

    protected override void Start()
    {
        base.Start();
        _playerCarryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
    }

    protected override void InteractFunction()
    {
        base.InteractFunction();
        if (!_isCarrying)
        {
            print("pick up");
            // Set parent to the carry point and reset the position so that the object will always be on the same position as the carry point
            transform.SetParent(_playerCarryPoint);
            transform.localPosition = Vector3.zero;
            // Set collider to trigger so that it won't collider with anything
            _objectCollider.isTrigger = true;
        }
        else
        {
            print("let go");
            // Reset the parent
            transform.parent = null;
            // Disable trigger function so that it can collide again
            _objectCollider.isTrigger = false;
        }
        
        // To interact again while still in the collider:
        SetInteract(true);
        _isCarrying = !_isCarrying;
    }
}
