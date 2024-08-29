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
    public float attackRange = 3f;
    public float searchDuration = 5f;
    private float searchTimer;

    public float idleTime = 2f;
    private float idleTimer;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    private Transform player;
    private bool isPlayerInSight = false;

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

        agent.speed = 0.5f;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = EnemyState.Idle;
        }
    }

    void ChasePlayer()
    {
        // Asegurarse de interrumpir todas las demás animaciones al detectar al jugador
        SetAnimatorStates(idle: false, patrol: false, chase: true, search: false);

        agent.speed = 1f;
        agent.destination = player.position;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
            agent.isStopped = true;
        }
        else if (distanceToPlayer > detectionRadius)
        {
            currentState = EnemyState.Searching;
            searchTimer = 0f;
        }
    }

    void AttackPlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: false, search: false);

        animator.SetTrigger("Attack");

        // Aquí agregarías el código para el Game Over o la lógica correspondiente

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

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1f, directionToPlayer, out hit, detectionRadius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        isPlayerInSight = true;
                        currentState = EnemyState.Chasing;
                        agent.isStopped = false;

                        // Asegurarse de interrumpir cualquier otra animación
                        SetAnimatorStates(idle: false, patrol: false, chase: true, search: false);
                    }
                    else
                    {
                        isPlayerInSight = false;
                    }
                }
            }
        }
        else if (isPlayerInSight)
        {
            currentState = EnemyState.Searching;
            agent.isStopped = true;
            isPlayerInSight = false;
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