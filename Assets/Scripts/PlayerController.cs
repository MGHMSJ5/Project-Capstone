using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation; 
    [SerializeField] private float _speed;
    [SerializeField] private float _groundDrag;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    private bool _readyToJump = true;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;
    private bool _grounded;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _direction;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundMask);

        PlayerInput();
        SpeedControl();

        //handle drag
        if (_grounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && _readyToJump && _grounded)
        {
            _readyToJump = false;
            Jump();
            Invoke("ResetJump", _jumpCooldown);
        }

        if (Input.GetButton("Sprint"))
        {
            print("weee");
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        _direction = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        if (_grounded)
        {
            _rb.MovePosition(_rb.position + _direction.normalized * _speed * Time.fixedDeltaTime);
            //_rb.velocity = _direction.normalized * _speed;
        }
        else if (!_grounded) //in the air
        {
            _rb.MovePosition(_rb.position + _direction.normalized * _speed * _airMultiplier * Time.fixedDeltaTime);
        }

    }

    private void Jump()
    {
        //reset y velocity (to make sure that player always jumps the same height
        if (_rb.velocity.y < 0f)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        }

        //use impulse, because the force is only applied once
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        //limit velocuty if needed
        if (flatVel.magnitude > _speed)
        {
            Vector3 limitedVel = flatVel.normalized * _speed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
}
