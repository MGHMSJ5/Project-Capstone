using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{   //Strength of the gravity force
    private static float GRAVITY_FORCE = 800;
    
    //Calculates the current gravity direction
    public Vector3 GravityDirection
    {
        get
        {   //= No gravity areas
            if (_gravityAreas.Count == 0) return Vector3.zero; 
            //Sort gravity areas by priority (lowest to highest)
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            //Use the highest priority gravity area and get its gravity direction
            return _gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody _rigidbody;
    private List<GravityArea> _gravityAreas;

    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _gravityAreas = new List<GravityArea>();
    }
    
    void FixedUpdate()
    {
        //Apply gravity force int eh current gravity direction
        _rigidbody.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode.Acceleration);

        //Rotate object to align with the gravity direction
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
        Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * 3f);;
        _rigidbody.MoveRotation(newRotation);
    }
    //Called when the object enters a gravity area
    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Add(gravityArea);
    }
    //Called when the object leaves a gravity area
    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Remove(gravityArea);
    }
}