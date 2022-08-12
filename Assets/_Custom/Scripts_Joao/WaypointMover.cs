using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    // Stores a reference to the waypoint system this object will use
    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float moveSpeed = 5f;

    // When the object is with a certain distance from a given threshold, it will be assigned to the next one
    [SerializeField] private float distanceThreshold = 4f; 

    // The current waypoint target that the object is moving towards
    Transform currentWaypoint;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        // Ensures that front is turned towards waypoint (snapy movement)
        transform.LookAt(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        // The truck follows the XZ coordinates of the waypoint. Y is defined by the interaction between colliders and terrain
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z), moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            Debug.Log(currentWaypoint.GetSiblingIndex());
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            transform.LookAt(currentWaypoint);
        }
    }
}
