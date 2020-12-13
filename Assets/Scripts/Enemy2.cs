using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public GameObject[] patrolPoints;
    private Vector3[] patrolPath;
    int currentPoint = 0;
    Vector3 debug_previous;
   
    Vector3 targetPoint;

    public float patrolSeed = 1;
    public float chaseSpeed = 2f;

    public float distance = 2f;
    public float distanceFrom = 4f;
    public float cooldown = 5;
    public GameObject player;

    enum State
    {
        Idle,
        Patrol,
        Chasing,
    }

    State state = State.Idle;
    private void Start()
    {
        patrolPath = new Vector3[patrolPoints.Length];
        
        int i = 0;
        foreach(GameObject go in patrolPoints)
        {
            patrolPath[i] = go.transform.position;
            i++;
        }
    }
    private void FixedUpdate()
    {
        if (state == State.Patrol)
        {
            Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position, distance);

            foreach (Collider2D c in coliders)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    state = State.Chasing;
                }
            }
        }
        else if (state == State.Chasing)
        {
            if (DistanceToPlayer() > distanceFrom)
            {
                state = State.Patrol;
            }
        }
    }
    void GetTarget()
    {
        debug_previous = targetPoint;
        targetPoint = patrolPath[currentPoint];
       
        if(currentPoint == patrolPath.Length-1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }
    }
    private float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }
    void Update()
    {
        switch (state)
        {
            case State.Patrol:
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSeed * Time.deltaTime);
                if (transform.position == targetPoint)
                {
                    cooldown -= Time.deltaTime;
                    Debug.DrawLine(debug_previous, targetPoint, Color.red, 40.0f);
                    if (cooldown < 0)
                    {
                        cooldown = 5;
                    }
                    GetTarget();
                }

                break;
            case State.Idle:
                    GetTarget();
                    state = State.Patrol;
                break;
            case State.Chasing:
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
                break;
        }
    }
}