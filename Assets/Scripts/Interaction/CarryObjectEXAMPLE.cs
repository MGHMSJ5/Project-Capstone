
using UnityEngine;

public class CarryObjectEXAMPLE : BaseInteract
{
    [Header("Own variables")]
    private Transform _parent;
    private Transform _playerCarryPoint;
    private bool _isCarrying = false;

    private Collider _parentCollider;
    private GravityBody _gravityBody;

    private PlayerController _playerController;

    protected override void Start()
    {
        base.Start();
        _parent = transform.parent;
        _playerCarryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _parentCollider = _parent.GetComponent<Collider>();
        _gravityBody = _parent.GetComponent<GravityBody>();
    }

    protected override void Update()
    {
        base.Update();
        if (_isCarrying)
        {
            // Reset the position of the parent when it is not (0,0,0)
            // Because it first checks if it's not Vector3.zero, it reduces the unnecessary amount of times it reset the position
            if (_parent.localPosition != Vector3.zero)
            {
                _parent.localPosition = Vector3.zero;
            }
        }
    }
    // Interacting will happen when the player is either carring the object or not //Thomas "carring" to "carrying"
    protected override void InteractFunction()
    {
        base.InteractFunction();
        if (!_isCarrying)
        {
            Carry();
        }
        else
        {
            LetGo();
        }
        
        // To interact again while still in the collider:
        SetInteract(true);
        // Change bool 
        _isCarrying = !_isCarrying;
    }
    // Function that will be called from objects that have the interrupt carrying set to True
    public void Interrupt()
    {
        InteractFunction();
    }

    private void Carry()
    {
        // Set parent to the carry point so that the parent will follow the player's carry point
        _parent.SetParent(_playerCarryPoint);

        // Set collider's Trigger on so that it won't collider with anything // Thomas 2nd "collider" to "collide"
        _parentCollider.isTrigger = true;
        // Also disable the gravity script so that it wont fall to the floow // Thomas "floow" to "floor"
        _gravityBody.enabled = false;

        // Slow down movement of the player if the object has a "HeavyObject" tag
        if (_parent.tag == "HeavyObject")
        {
            _playerController.CarryHeavyObject(true);
        }
    }

    private void LetGo() // Thomas "LetGo" to "Drop" for additional clarity
    {
        // Reset the parent's parent
        _parent.parent = null;
        // Disable trigger function so that it can collide again
        _parentCollider.isTrigger = false;
        // Enable the gravity so that it falls to the planet
        _gravityBody.enabled = true;

        // Reset the player movement if the object has a "HeavyObject" tag
        if (_parent.tag == "HeavyObject")
        {
            _playerController.CarryHeavyObject(false);
        }
    }
}
