using UnityEngine;
using UnityEngine.AI;

public class WaypointMovement : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints; //array list of waypoints
    int waypointindex; //int that will be used to select different waypoints
    Vector3 target; //will be used to get the location of the waypoint(s)

    public bool stopWalking;
    [Tooltip("Set to true if the agent needs to stop at the last waypoint")]
    public bool doNotLoopWaypoints = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //get the NavMeshAgent from this object
        UpdateDestination(); //start the first destination: the first waypoint
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 1 && !stopWalking)//if the distance between the Object, and the waypoint target is less than 1 ↓
        {
            NextWaypoint(); //change the int, to get the next waypoint
            UpdateDestination();//go to the next waypoint
        }
        if (stopWalking)
        {
            agent.SetDestination(transform.position); //stop the npc
        }
        if (!stopWalking && agent.SetDestination(transform.position))
        {
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        if (!stopWalking)
        {
            target = waypoints[waypointindex].position; //get the position of the waypoint
            agent.SetDestination(target); //set destination of the target to the waypoint
        }
    }

    void NextWaypoint()
    {
        waypointindex++; //+1, will be used to get to the next waypoint
        if (waypointindex == waypoints.Length) //if this int gets bigger that the number of waypoints
        {
            if (doNotLoopWaypoints)
            {
                stopWalking = true;
            }
            else
            {
                waypointindex = 0; //set back to start, the first waypoint
            }
        }
    }
}
