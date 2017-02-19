using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Actor {

    public float MoveSpeed = 128f;
    public Path path;
    public float nextWaypointDistance = 3;
    private Seeker seeker;
    private int currentWaypoint;
    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
    }

    void FixedUpdate()
    {
        if (path == null) {
            Move(Vector2.zero);
            return;
        }
        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count) {
            Debug.Log("End Of Path Reached");
            currentWaypoint++;
            Move(Vector2.zero);
            return;
        }
        Vector3 dir = (path.vectorPath[currentWaypoint]
                       - transform.position).normalized;
        dir *= MoveSpeed;
        Move((Vector2)dir);
        if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude
             < nextWaypointDistance*nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
    public void PathTo(GameObject target)
    {
        Debug.Log("Pathing");
        seeker.StartPath(transform.position, target.transform.position 
                                             + new Vector3(16,16, 0),
                         OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Path found");
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }
}
