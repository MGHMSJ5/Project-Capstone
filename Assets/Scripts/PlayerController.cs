using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;
    [SerializeField] private float _groundDrag;

    [Header("Sprinting")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _maxSprintAccelerationTime = 1f;
    [SerializeField] private float _currentSprintTime = 0.0f;

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

    // Exposing the following variables safely, for use in other scripts
    public Transform Orientation => _orientation;
    public float Speed => _speed;
    public float GroundDrag => _groundDrag;
    public float NormalSpeed => _normalSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float MaxSprintAccelerationTime => _maxSprintAccelerationTime;
    public float CurrentSprintTime => _currentSprintTime;
    public float JumpForce => _jumpForce;
    public float JumpCooldown => _jumpCooldown;
    public float AirMultiplier => _airMultiplier;
    public bool ReadyToJump => _readyToJump;
    public float PlayerHeight => _playerHeight;
    public LayerMask GroundMask => _groundMask;
    public bool Grounded => _grounded;
    public float HorizontalInput => horizontalInput;
    public float VerticalInput => verticalInput;
    public Rigidbody RB => _rb;
    public Vector3 Direction => _direction;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _speed = _normalSpeed;
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

        if (horizontalInput == 0 && verticalInput == 0)
        {
            _speed = _normalSpeed;
        }
        else
        {
            Sprint();
        }
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

    private void Sprint()
    {
        if (Input.GetAxis("Sprint") == 1)
        {   //increase the sprint time, but don't go over the max acceleration time
            _currentSprintTime = Mathf.Min(_currentSprintTime + Time.deltaTime, _maxSprintAccelerationTime);
        }
        else
        {   //decrease the sprint time, but don't go below 0
            _currentSprintTime = Mathf.Max(_currentSprintTime - Time.deltaTime, 0f);
        }
        //normalize the time value between 0 and 1
        float t = _currentSprintTime / _maxSprintAccelerationTime;
        //smoothly transition the speed based on sprinting time
        _speed = Mathf.Lerp(_normalSpeed, _sprintSpeed, t);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > _speed)
        {
            Vector3 limitedVel = flatVel.normalized * _speed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
}
