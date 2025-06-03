
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class CarryObjectEXAMPLE : BaseInteract
{
    [Header("Own variables")]
    private Transform _parent;
    private Transform _playerCarryPoint;
    [SerializeField]
    private bool _isCarrying = false;
    // Instance is used to keep track of which item is being carried
    private static CarryObjectEXAMPLE _currentlyCarrying;

    private Collider _parentCollider;
    private Rigidbody _parentRb;
    private GravityBody _gravityBody;

    private PlayerController _playerController;

    [SerializeField]
    private float _objectHeight;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private bool _grounded;

    protected override void Start()
    {
        base.Start();
        _parent = transform.parent;
        _playerCarryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _parentCollider = _parent.GetComponent<Collider>();
        _parentRb = _parent.GetComponent<Rigidbody>();
        _gravityBody = _parent.GetComponent<GravityBody>();
        _objectHeight = _parent.transform.localScale.y;
    }

    protected override void Update()
    {
        base.Update();
        // Ground Check
        _grounded = Physics.CheckBox(transform.position + Vector3.down * _objectHeight * 0.5f, new Vector3(0.5f, 0.05f, 0.5f), Quaternion.identity, _groundMask);
        //_grounded = Physics.Raycast(transform.position, -transform.up, _objectHeight * 0.5f + 0.1f, _groundMask);
        if (_grounded)
        {
            _parentRb.isKinematic = true;
        }
        else
        {
            _parentRb.isKinematic = false;
        }

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
        void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * _objectHeight * 0.5f, new Vector3(1f, 0.05f, 1f));
    }
    // Interacting will happen when the player is either carrying the object or not //Thomas "carring" to "carrying" 
    protected override void InteractFunction()
    {
        base.InteractFunction();
        if (!_isCarrying)
        {
            // Only carry object if nothing else is being carried
            if (_currentlyCarrying == null)
            {
                Carry();
            }
        }
        else
        {
            Drop();
        }
        // To interact again while still in the collider:
        SetInteract(true);        
    }
    // Function that will be called from objects that have the interrupt carrying set to True
    public void Interrupt()
    {
        InteractFunction();
    }

    // Function created to clear the current carried object with a delay to fix issue of carrying
    public void ClearCurrentCarry()
    {
        _currentlyCarrying = null;
    }
    private void Carry()
    {
        // Reset the parent
        _parent.parent = null;
        // Thomas add a little comment on what is happening below (I know what is happening but just for consistency)
        // Set this instance as the item that is being carried
        _currentlyCarrying = this;
        // Set parent to the carry point so that the parent will follow the player's carry point
        _parent.SetParent(_playerCarryPoint);
        // Set collider's Trigger on so that it won't collide with anything // Thomas 2nd "collider" to "collide"
        _parentCollider.isTrigger = true;
        // Also disable the gravity script so that it wont fall through the floor // Thomas "floow" to "floor"
        // Check if the parent does have a GravityBody script. Otherwise disable the gravity usage from the RigidBody of the parent
        if (_gravityBody != null)
        {
            _gravityBody.enabled = false;
        }
        else
        {
            _parentRb.useGravity = false;
        }

        // Slow down movement of the player if the object has a "HeavyObject" tag
        if (_parent.tag == "HeavyObject")
        {
            _playerController.CarryObject(true, true);
        }
        else
        {
            _playerController.CarryObject(true, false);
        }
        _isCarrying = true;
        
    }

    private void Drop() // Thomas "LetGo" to "Drop" for additional clarity
    {
        _currentlyCarrying.Invoke("ClearCurrentCarry", 0.2f);
        // Reset the parent's parent
        _parent.parent = null;
        // Disable trigger function so that it can collide again
        _parentCollider.isTrigger = false;
        // Enable the gravity so that it falls to the planet
        if (_gravityBody != null)
        {
            _gravityBody.enabled = true;
        }
        else
        {
            _parentRb.useGravity = true;
        }

        // Reset the player movement if the object has a "HeavyObject" tag
        _playerController.CarryObject(false, false);
        _isCarrying = false;
    }
}
