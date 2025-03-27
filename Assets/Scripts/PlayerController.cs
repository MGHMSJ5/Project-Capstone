using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundMask;

    private float _groundCheckRadius = 0.52f;
    private float _speed = 8;
    private float _turnSpeed = 10f;
    private float _jumpForce = 500f;

    private Transform _groundCheck;
    private Rigidbody _rb;
    private Vector3 _direction;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        bool isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        bool isRunning = _direction.magnitude > 0.1f;

        if (isRunning)
        {
            Vector3 direction = transform.forward * _direction.z;
            _rb.MovePosition(_rb.position + direction * (_speed * Time.fixedDeltaTime));

            Quaternion rightDirection = Quaternion.Euler(0f, _direction.x * (_turnSpeed * Time.fixedDeltaTime), 0f);
            Quaternion newRotation = Quaternion.Slerp(_rb.rotation, _rb.rotation * rightDirection, Time.fixedDeltaTime * 3f);
            _rb.MoveRotation(newRotation);
        }
    }
}
