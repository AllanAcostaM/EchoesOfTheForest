using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chasing,
        Searching,
        Attacking
    }

    public EnemyState currentState;

    private Animator animator;
    private NavMeshAgent agent;

    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    public float detectionRadius = 10f;
    public float fieldOfViewAngle = 120f;
    public float attackRange = 1.5f;
    public float searchDuration = 5f;
    private float searchTimer;

    public float idleTime = 2f;
    private float idleTimer;

    private Transform player;
    private bool chasingPlayer = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        GoToCurrentPatrolPoint();
    }

    void Update()
    {
        DetectPlayer();

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Searching:
                SearchForPlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    void Idle()
    {
        SetAnimatorStates(idle: true, patrol: false, chase: false, search: false);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
            currentState = EnemyState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    void Patrol()
    {
        SetAnimatorStates(idle: false, patrol: true, chase: false, search: false);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = EnemyState.Idle;
        }
    }

    void ChasePlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: true, search: false);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
            agent.isStopped = true;
        }
        else
        {
            agent.destination = player.position;

            if (distanceToPlayer > detectionRadius)
            {
                currentState = EnemyState.Searching;
                agent.isStopped = true;
                searchTimer = 0f;
                chasingPlayer = false;
            }
        }
    }

    void AttackPlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: false, search: false);

        animator.SetTrigger("Attack");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chasing;
            agent.isStopped = false;
        }
    }

    void SearchForPlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: false, search: true);

        agent.isStopped = true;
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            GoToClosestPatrolPoint();
            currentState = EnemyState.Patrol;
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Verificar si el jugador está dentro del ángulo de visión
            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                RaycastHit hit;

                // Emitir un raycast desde la posición del enemigo hacia el jugador
                if (Physics.Raycast(transform.position + Vector3.up * 1f, directionToPlayer, out hit, detectionRadius))
                {
                    // Verificar si el raycast golpea al jugador directamente
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (currentState != EnemyState.Chasing)
                        {
                            currentState = EnemyState.Chasing;
                            agent.isStopped = false;
                            chasingPlayer = true;
                            SetAnimatorStates(idle: false, patrol: false, chase: true, search: false);
                        }
                    }
                    else
                    {
                        // Si el raycast golpea algo que no sea el jugador, perder la visión
                        if (currentState == EnemyState.Chasing)
                        {
                            currentState = EnemyState.Searching;
                            agent.isStopped = true;
                            chasingPlayer = false;
                            SetAnimatorStates(idle: false, patrol: false, chase: false, search: true);
                        }
                    }
                }
            }
        }
        else if (currentState == EnemyState.Chasing)
        {
            // Cambiar a estado de búsqueda si el jugador se pierde
            currentState = EnemyState.Searching;
            agent.isStopped = true;
            chasingPlayer = false;
            SetAnimatorStates(idle: false, patrol: false, chase: false, search: true);
        }
    }

    void SetAnimatorStates(bool idle, bool patrol, bool chase, bool search)
    {
        animator.SetBool("isIdle", idle);
        animator.SetBool("isPatrolling", patrol);
        animator.SetBool("isChasing", chase);
        animator.SetBool("isSearching", search);
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        GoToCurrentPatrolPoint();
    }

    void GoToCurrentPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        agent.isStopped = false;
    }

    void GoToClosestPatrolPoint()
    {
        float shortestDistance = Mathf.Infinity;
        int closestPointIndex = currentPatrolIndex;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, patrolPoints[i].position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPointIndex = i;
            }
        }

        currentPatrolIndex = closestPointIndex;
        GoToCurrentPatrolPoint();
    }
}