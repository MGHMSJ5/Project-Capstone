using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [Header("Gravity")]

    [Tooltip("Strength of gravity acceleration.")]
    [SerializeField] private float _gravityForce = 30f;

    public Vector3 GravityDirection
    {
        get
        {
            if (_gravityAreas.Count == 0)
            {
                return Vector3.zero;
            }

            _gravityAreas.Sort(
                (area1, area2) =>
                    area1.Priority.CompareTo(
                        area2.Priority
                    )
            );

            return _gravityAreas.Last()
                .GetGravityDirection(this)
                .normalized;
        }
    }

    private Rigidbody _rigidbody;

    private List<GravityArea> _gravityAreas;

    private void Start()
    {
        _rigidbody =
            GetComponent<Rigidbody>();

        _gravityAreas =
            new List<GravityArea>();
    }

    private void FixedUpdate()
    {
        if (GravityDirection == Vector3.zero)
        {
            return;
        }

        // ForceMode.Acceleration already handles
        // the timestep.
        _rigidbody.AddForce(
            GravityDirection *
            _gravityForce,
            ForceMode.Acceleration
        );

        // Align player with gravity.
        Quaternion upRotation =
            Quaternion.FromToRotation(
                transform.up,
                -GravityDirection
            );

        Quaternion newRotation =
            Quaternion.Slerp(
                _rigidbody.rotation,
                upRotation *
                _rigidbody.rotation,
                Time.fixedDeltaTime * 3f
            );

        _rigidbody.MoveRotation(
            newRotation
        );
    }

    public void AddGravityArea(
        GravityArea gravityArea
    )
    {
        if (!_gravityAreas.Contains(
            gravityArea))
        {
            _gravityAreas.Add(
                gravityArea
            );
        }
    }

    public void RemoveGravityArea(
        GravityArea gravityArea
    )
    {
        _gravityAreas.Remove(
            gravityArea
        );
    }
}