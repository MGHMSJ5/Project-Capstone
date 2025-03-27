using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation; 
    [SerializeField] private float _speed;

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
        PlayerInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculate movement direction
        _direction = _orientation.forward * verticalInput + _orientation.right * horizontalInput;
        _rb.MovePosition(_rb.position + _direction.normalized * _speed * Time.fixedDeltaTime);
        //_rb.velocity = _direction.normalized * _speed;
    }
}
