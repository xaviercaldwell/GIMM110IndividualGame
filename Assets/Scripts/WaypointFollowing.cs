using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WaypointFollowing : MonoBehaviour
{
    private int currentWaypointIndex = 0;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 1.0f;

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].position, transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
    }

}
