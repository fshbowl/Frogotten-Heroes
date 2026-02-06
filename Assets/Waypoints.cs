using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float waypointSize = 1f;

    [Header("Path Settings")]
    //Sets the path to be looped so agent will go from last waypoint to first waypoint or vice versa
    [SerializeField] private bool canLoop = true;

    //Sets the agent to move forward or backwards
    [SerializeField] private bool isMovingForward = true;

    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        //If the path is set to loop then draw the line between the last and first waypoint
        if (canLoop)
        {
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }

    }

    //Will get the correct next waypoint based on the direction currently travelling
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }
        
        //Stores the index of the current waypoint
        int currentIndex = currentWaypoint.GetSiblingIndex();

        //Stores the index of the next waypoint to travel towards
        int nextIndex = currentIndex;


        //Agent is moving forward on the path
        if (isMovingForward)
        {
            nextIndex += 1;

            //If the next waypoint index is equal to the count of the children/waypoints then it is already at the last waypoint
            //Check if the path is set to loop and return the first waypoint as the current waypoint, otherwise we subtract 1 from the nextIndex which will
            //return the same waypoint that the agent is currently at, which will cause it to stop moving since it is already there.

            if (nextIndex >= transform.childCount)
            {
                if (canLoop)
                {
                    nextIndex = 0;
                }
                else
                {
                    nextIndex -= 1;
                }
            }
        }
        //Agent is moving backwards on the path
        else
        {
            nextIndex -= 1;

            //If the nextIndex is below 0 then it means that you are already at the first waypoint, check if the path is set to loop and if so return the last waypoint
            //otherwise add 1 to the nextIndex which will return the current waypoint that you are already at which will casue the agent to stop since it is already there.

            if (nextIndex < 0)
            {
                if (canLoop)
                {
                    nextIndex = transform.childCount - 1;
                }
                else
                {
                    nextIndex += 1;
                }
            }
        }

        //Return the waypoint that has the index of nextIndex.
        return transform.GetChild(nextIndex);

    }
}
