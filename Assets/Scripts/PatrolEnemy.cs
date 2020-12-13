using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    Vector3 patrolPoint;
    Vector3 targetPoint;

    public float radius = 3f;

    public float patrolSeed = 1;
    public float chaseSpeed = 2f;

    public float distance = 2f;
    public float distanceFrom = 4f;
    public float cooldown = 5;
    public GameObject player;

    public float visionAngle = 45 * Mathf.Deg2Rad;
    //public Quaternion visionAngle;
    public GameObject coneRoot;

    enum State
    {
        Idle,
        Patrol,
        Chasing,
    }

    State state = State.Idle;

    private void FixedUpdate()
    {
        if(state == State.Patrol)
        {
            Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position, distance);

            foreach (Collider2D c in coliders)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    state = State.Chasing;
                }
            }  
        } else if(state == State.Chasing)
        {
            if(DistanceToPlayer() > distanceFrom)
            {
                state = State.Idle;
            }
        }
    }
    void VisionCone()
    {
       // visionAngle = visionAngle * Mathf.Deg2Rad;

        float x = coneRoot.transform.position.x + (radius * Mathf.Cos(visionAngle)); 
        float y = coneRoot.transform.position.y + (radius * Mathf.Sin(visionAngle));

        float x2 = coneRoot.transform.position.x + (radius * Mathf.Cos(0)); 
        float y2 = coneRoot.transform.position.y + (radius * Mathf.Sin(0));


        Debug.DrawLine(transform.position, new Vector3(x,y,0), Color.blue);
        Debug.DrawLine(transform.position, new Vector3(x2, y2, 0), Color.green);
       // Debug.DrawLine(transform.position, new Vector3(x, y, 0), Color.yellow, 1.0f);
    }
    void GetTarget()
    {
        float randomAngle = Random.value * 360;

        float x = patrolPoint.x + (radius * Mathf.Cos(randomAngle));
        float y = patrolPoint.y + (radius * Mathf.Sin(randomAngle));

        targetPoint = new Vector3(x, y, 0);
    }
    private float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }
    void Update()
    {
        VisionCone();
        switch (state)
        {
            case State.Patrol:
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSeed * Time.deltaTime);
                if(transform.position == targetPoint)
                {
                    cooldown -= Time.deltaTime;
                    Debug.DrawLine(patrolPoint, targetPoint, Color.red, 40.0f);
                    if (cooldown < 0)
                    {
                        cooldown = 5;
                    }
                    GetTarget();
                }
               
                break;
            case State.Idle:
                    patrolPoint = transform.position;
                    GetTarget();
                    state = State.Patrol;
                break;
            case State.Chasing:
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position , chaseSpeed * Time.deltaTime);
                break;
        }
    }
}
