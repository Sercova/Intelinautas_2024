using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CreatureAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform creatureSprite;
    public float jumpMagnitude;
    public float momentumFactor;
    public bool stuck;
    public Rigidbody2D rb;
    public int CreatureID;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        stuck = false;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (Mathf.Abs(rb.velocity.x) < 0.1f && rb.velocity.y < 0.05f && rb.velocity.y >= 0.0f)
        {
            stuck = true;
            Jump(momentumFactor);
        } else
            stuck = false;



        if (rb.velocity.x > 0.01f)
            creatureSprite.localScale = new Vector3(1f, 1f, 1f);
        else if (rb.velocity.x < -0.01f)
            creatureSprite.localScale = new Vector3(-1f, 1f, 1f);

    }

    public void Jump(float arg_magnitud)
    {
        Vector2 force = Vector2.up * arg_magnitud;
        rb.AddForce(force);
        //Debug.Log("force.magnitude: " + force.magnitude);
        //gameObject.GetComponentInParent<CreatureAI>().speed = initialSpeed;
    }

}
