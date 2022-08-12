using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // Set the size of the wyapoints for visualization
    [Range(0f, 3f)]
    [SerializeField] private float waypointSize = 2f;
    

    // Allows things to be drawn on the scene. These don't show in the game view
    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for(int i=0;i<transform.childCount - 1;i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position,transform.GetChild(i+1).position);    
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position,transform.GetChild(0).position);
    }

    // Will be used in the movement script
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0); // If the object doesn't start at a waypoint, it is directed towards the first one
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1); // If the object is already at a waypoint, it is directed towards the next one
        }
        else
        {
            return transform.GetChild(0); // If the object is at the final waypoint, it is directed towards the first one, closing the loop
        }
    }
}
