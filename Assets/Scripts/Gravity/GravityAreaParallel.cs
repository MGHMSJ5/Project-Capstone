using UnityEngine;

public class GravityAreaParallel : GravityArea
{
    //Function calculates the direction of the gravity for an object
    public override Vector3 GetGravityDirection(GravityBody _gravityBody)
    {//Gravity always goes downwards
        return -transform.up;
    }
}
