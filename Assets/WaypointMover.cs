using System.Runtime.CompilerServices;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{

    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float moveSpeed = 5f;

    [Range(0f, 15f)] //How fast the agent will rotate once it reaches its waypoint
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private float distanceThreshold = 0.1f;

    [SerializeField] private float curveStrength = 2f;

    private Transform previousWaypoint;

    private float t;

    private Vector3 startPoint;
    private Vector3 endPoint;

    private Transform currentWaypoint;

    //The rotation target for the current frame
    private Quaternion rotationGoal;

    //The direction to the next waypoint that the agent needs to rotate towards
    private Vector3 directionToWaypoint;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            //transform.LookAt(currentWaypoint);
        }
        RotateTowardsWaypoint();
    }

    //Will slowly rotate the agent towards the current waypoint it is moving towards
    private void RotateTowardsWaypoint()
    {         
        directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationGoal = Quaternion.LookRotation(directionToWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, rotateSpeed * Time.deltaTime);
    }

    private Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
}
