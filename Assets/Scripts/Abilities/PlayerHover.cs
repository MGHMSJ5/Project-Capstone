using UnityEngine;

public class PlayerHover : MonoBehaviour
{
    [Header("Hovering")]
    [SerializeField] private bool _hoverAbilityGranted = true;
    [SerializeField] private float _hoverForce = 2f;
    [SerializeField] private float _maxHoverTime = 3f;
    private float _currentHoverTime = 0f;
    private bool _isHovering = false;

    [Header("Movement")]
    private PlayerController _playerController;
    private GravityBody _gravityBody;

    [Header("Ground Check")]
    private bool _grounded;

    private Vector3 GravityDirection => _gravityBody != null ? _gravityBody.GravityDirection : Vector3.down;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _gravityBody = GetComponent<GravityBody>(); // This could be null, and that's okay now!
    }

    void Update()
    {
        if (!_hoverAbilityGranted) return;

        _grounded = Physics.Raycast(
            transform.position,
            GravityDirection,
            _playerController.PlayerHeight * 0.5f + 0.2f,
            _playerController.GroundMask
        );

        if (!_grounded && Input.GetButton("Jump") && _currentHoverTime < _maxHoverTime)
        {
            Hover();
        }
        else
        {
            StopHover();
        }

        if (_grounded && !_isHovering)
        {
            _currentHoverTime = 0f;
        }
    }

    private void Hover()
    {
        _isHovering = true;
        _currentHoverTime += Time.deltaTime;

        Rigidbody rb = _playerController.RB;
        Vector3 localUp = -GravityDirection;

        if (Vector3.Dot(rb.velocity, -localUp) > 0f)
        {
            rb.AddForce(localUp * _hoverForce, ForceMode.Acceleration);
        }

        Vector3 moveInput = _playerController.Orientation.forward * _playerController.VerticalInput +
                            _playerController.Orientation.right * _playerController.HorizontalInput;
        Vector3 localMove = Vector3.ProjectOnPlane(moveInput, localUp).normalized;

        rb.velocity = new Vector3(localMove.x * _playerController.Speed, rb.velocity.y, localMove.z * _playerController.Speed);
    }

    private void StopHover()
    {
        _isHovering = false;

        if (_currentHoverTime >= _maxHoverTime)
        {
            _currentHoverTime = _maxHoverTime;
        }
    }
}
