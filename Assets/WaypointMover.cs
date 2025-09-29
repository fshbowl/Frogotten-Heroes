using UnityEngine;

public class WaypointMover : MonoBehaviour
{

    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float moveSpeed = 5f;

    {SerializeField] private float distanceThreshold = 0.1f;

    private Transform currentWaypoint;



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
            transform.LookAt(currentWaypoint);
        }
    }
}
