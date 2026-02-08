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
    private Transform nextWaypoint;
    private Transform prevWaypoint;

    //The rotation target for the current frame
    private Quaternion rotationGoal;

    //The direction to the next waypoint that the agent needs to rotate towards
    private Vector3 directionToWaypoint;
    private Vector3 controlPoint;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousWaypoint = waypoints.GetNextWaypoint(null);
        currentWaypoint = waypoints.GetNextWaypoint(previousWaypoint);
        nextWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        transform.position = previousWaypoint.position;
        SetupCurve();
    }

    // Update is called once per frame
    void Update()
    {
        // Move along the curve
        t += Time.deltaTime * moveSpeed;
        transform.position = Bezier(startPoint, controlPoint, currentWaypoint.position, t);

        RotateTowardsWaypoint();

        // When curve is finished, advance waypoints
        if (t >= 1f)
        {
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
            nextWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

            SetupCurve();
        }
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

    // Prepare a new curve segment
    private void SetupCurve()
    {
        startPoint = transform.position;
        t = 0f;

        Vector3 dirIn = (currentWaypoint.position - previousWaypoint.position).normalized;
        Vector3 dirOut = (nextWaypoint.position - currentWaypoint.position).normalized;

        Vector3 curveOffset = Vector3.Cross(dirIn, dirOut).normalized * curveStrength;

        // Prevent weird flips on straight lines
        if (Vector3.Dot(dirIn, dirOut) > 0.95f)
            curveOffset = Vector3.zero;

        controlPoint = currentWaypoint.position + curveOffset;
    }
}
