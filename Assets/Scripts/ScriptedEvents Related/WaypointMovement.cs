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

    [Header("Rotate Correctly")]
    [SerializeField]
    private Transform model;
    [SerializeField]
    private Transform planet;
    private float rotationSpeed = 10f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //get the NavMeshAgent from this object
        UpdateDestination(); //start the first destination: the first waypoint
        UpdateRotation(model.forward, false);
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
            UpdateRotation(agent.velocity, true);
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

    void UpdateRotation(Vector3 direction, bool useSlerp)
    {
        if (planet == null || model == null)
        {
            return;
        }
        Vector3 up = (model.position - planet.position).normalized;

        if (up == Vector3.zero)
        {
            return;
        }

        Vector3 faceDir = direction;

        Vector3 forward = Vector3.ProjectOnPlane(faceDir, up).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(forward, up);

        model.rotation = useSlerp ? Quaternion.Slerp(model.rotation, targetRotation, rotationSpeed * Time.deltaTime) : targetRotation;
    }
}
