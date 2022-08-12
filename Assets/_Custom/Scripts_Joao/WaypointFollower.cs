//------------------------------------------------------------------------------------------------
// Edy's Vehicle Physics
// (c) Angel Garcia "Edy" - Oviedo, Spain
// http://www.edy.es
//------------------------------------------------------------------------------------------------

using UnityEngine;

namespace EVP
{

	public class WaypointFollower : MonoBehaviour
	{
		public float steerInterval = 10.0f;
		public float steerIntervalTolerance = 1.0f;
		public float steerChangeRate = 20.0f;

		[Space(5)]
		public float throttleInterval = 5.0f;
		public float throttleIntervalTolerance = 2.0f;
		public float throttleChangeRate = 3.0f;

		float m_targetSteer = 0.0f;
		float m_targetThrottle = 0.0f;
		float m_targetBrake = 0.0f;

		VehicleController m_vehicle;

		// When the object is with a certain distance from a given threshold, it will be assigned to the next one
		[SerializeField] private float distanceThreshold = 4f;

		// Stores a reference to the waypoint system this object will use
		[SerializeField] private Waypoints waypoints;

		// The current waypoint target that the object is moving towards
		Transform currentWaypoint;

		void OnEnable()
		{
			m_vehicle = GetComponent<VehicleController>();
		}
		void Start()
		{
			// Set the first waypoint target
			currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
		}

		float FindAngle()
        {
			var relativePos = currentWaypoint.position - transform.position;

			var forward = transform.forward;
			var angle = Vector3.Cross(forward, relativePos).y;

			return angle;
			
		}

		void Update()
		{

			// Setting a new waypoint as a target
			if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
			{
				currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
			}

			//Debug.Log(currentWaypoint.GetSiblingIndex());

			// Set a steer value based on the target waypoint

			m_targetSteer = FindAngle() / 30;
			//Debug.Log(m_targetSteer);
			//Debug.Log(FindAngle()); // Shows the angle between the truck and the waypoint 
			//Debug.Log(m_targetSteer); // Shows the desired steer of the truck in regard to the waypoint 

			float speed = m_vehicle.cachedRigidbody.velocity.magnitude;
			Debug.Log(speed);

			// Defining constant Throttle
			m_targetThrottle = 0.8f;
			m_targetBrake = 0.0f;

			// When the truck reaches a certain speed, breaks are activated to counteract the throttle, leading to constant speed
			if (speed > 9.0f)
            {
				m_targetBrake = 0.9f;
			}

            // Apply the input progressively

            m_vehicle.steerInput = Mathf.MoveTowards(m_vehicle.steerInput, m_targetSteer, steerChangeRate * Time.deltaTime);
			m_vehicle.throttleInput = Mathf.MoveTowards(m_vehicle.throttleInput, m_targetThrottle, throttleChangeRate * Time.deltaTime);
			m_vehicle.brakeInput = m_targetBrake;
			m_vehicle.handbrakeInput = 0.0f;
		}

        void FixedUpdate()
        {
            
        }

    }
}
