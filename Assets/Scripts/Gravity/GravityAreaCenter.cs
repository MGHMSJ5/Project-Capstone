using System.Collections.Generic;
using UnityEngine;

public class GravityAreaCenter : GravityArea
{
    //Function calculates the direction of the gravity for an object
    public override Vector3 GetGravityDirection(GravityBody _gravityBody)
    {   //Find the direction from the object to the center of this gravity area
        return (transform.position - _gravityBody.transform.position).normalized;
    }
}
