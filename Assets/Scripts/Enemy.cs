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
    public EnemyState State;
    public float CollisionStallTime = 1f;
    public Sprite DeadSprite;
    private Seeker seeker;
    private int currentWaypoint;

    private List<GameObject> collisionsToResolve = new List<GameObject>();
    private float collisionStallTimer = 0f;
    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
    }

    void FixedUpdate()
    {
        if (path == null) {
            Move(Vector2.zero);
            if (State == EnemyState.IDLE)
            {
                SetIdleTarget();
            }
            return;
        }
        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count) {
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
        seeker.StartPath(transform.position, target 
                                             + new Vector3(16,16, 0),
                         OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
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
        State = EnemyState.IDLE;
    }

    public void InvalidatePath()
    {
        path = null;
        collisionStallTimer = 0f;
    }

    public void Die()
    {
        // disable collider, and change sprite.
        GetComponent<Collider2D>().enabled = false;
        sprite_renderer.sprite = DeadSprite;
        State = EnemyState.DEAD;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            Enemy other = coll.gameObject.GetComponent<Enemy>();
            if (other.State == EnemyState.IDLE & State == EnemyState.IDLE)
            {
                // both idle - just force repathing
                InvalidatePath();
                other.InvalidatePath();
            }
            collisionsToResolve.Add(coll.gameObject);
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        collisionStallTimer += Time.fixedDeltaTime;
        if (collisionStallTimer >= CollisionStallTime)
        {
            InvalidatePath();
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (collisionsToResolve.Contains(coll.gameObject))
        {
            collisionsToResolve.Remove(coll.gameObject);
        }
        if (collisionsToResolve.Count == 0)
        {
            collisionStallTimer = 0f;
        }
    }
}
