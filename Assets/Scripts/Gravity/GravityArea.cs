using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class GravityArea : MonoBehaviour
{
    //Determines the priority of the gravity area
    [SerializeField] private int _priority;
    public int Priority => _priority; //Public getter for the priority value
    
    void Start()
    {
        //Makes sure that the collider is a trigger
        transform.GetComponent<Collider>().isTrigger = true;
    }
    
    //This function must be used in the child classes
    //It decides the gravity direction for a specific GravityBody inside this area
    public abstract Vector3 GetGravityDirection(GravityBody _gravityBody);
    
    //Called when another objects enters the gravity area
    private void OnTriggerEnter(Collider other)
    {   //Checks if the object has a GravityBody component
        if (other.TryGetComponent(out GravityBody gravityBody))
        {   //Add this gravity area to the object's list of active gravity sources
            gravityBody.AddGravityArea(this);
        }
    }
    
    //Called when an object leaves the gravity area
    private void OnTriggerExit(Collider other)
    {   //Checks if the object has a GravityBody component
        if (other.TryGetComponent(out GravityBody gravityBody))
        {   //Removes this gravity area from the object's list
            gravityBody.RemoveGravityArea(this);
        }
    }
}