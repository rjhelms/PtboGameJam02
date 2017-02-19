using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState {
    IDLE,
    CHASING,
    DEAD
}

public class Enemy : Actor {

    public float MoveSpeed = 128f;
    public Path path;
    public float nextWaypointDistance = 16f;
    public float IdlePathingTargetDistance = 320f;
    private Seeker seeker;
    private int currentWaypoint;
    [SerializeField]
    private EnemyState state;
    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
    }

    void FixedUpdate()
    {
        if (path == null) {
            Move(Vector2.zero);
            if (state == EnemyState.IDLE)
            {
                SetIdleTarget();
            }
            return;
        }
        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count) {
            Debug.Log("End Of Path Reached");
            path = null;
            currentWaypoint++;
            Move(Vector2.zero);
            return;
        }
        Vector3 dir = (path.vectorPath[currentWaypoint]
                       - transform.position).normalized;
        dir *= MoveSpeed;
        Move((Vector2)dir);
        if ((transform.position - path.vectorPath[currentWaypoint])
                 .sqrMagnitude
             < nextWaypointDistance*nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }

    private void SetIdleTarget()
    {
        // in idle state, don't clobber seeker svp
        if (seeker.IsDone())
        {
            Vector2 target_direction = Random.insideUnitCircle.normalized;
            target_direction *= IdlePathingTargetDistance;
            Vector3 target_point = transform.position + (Vector3)target_direction;
            PathTo(target_point);
        }
    }

    public void PathTo(GameObject target)
    {
        PathTo(target.transform.position);
    }

    public void PathTo(Vector3 target)
    {
        Debug.Log("Pathing");
        seeker.StartPath(transform.position, target 
                                             + new Vector3(16,16, 0),
                         OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Path found");
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards 
            // the first point in the path
            currentWaypoint = 0;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        state = EnemyState.IDLE;
    }
}
