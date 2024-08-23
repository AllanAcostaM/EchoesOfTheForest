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
        Attacking  // Nuevo estado de ataque
    }

    public EnemyState currentState;

    private Animator animator;        
    private NavMeshAgent agent;       

    public Transform[] patrolPoints;  
    private int currentPatrolIndex;   
    private Transform lastPatrolPoint;

    public float detectionRadius = 10f;
    public float fieldOfViewAngle = 120f;  
    public float attackRange = 1.5f; // Rango de ataque
    public float searchDuration = 5f;   
    private float searchTimer;          

    public float idleTime = 2f;        
    private float idleTimer;            

    private Transform player;           

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on " + gameObject.name);
            enabled = false;
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentState = EnemyState.Patrol;
        currentPatrolIndex = -1;
        GoToNextPatrolPoint();
    }

    void Update()
    {
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
            case EnemyState.Attacking:  // Añadir estado de ataque
                AttackPlayer();
                break;
        }

        DetectPlayer();
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

        // Si el jugador está en el rango de ataque, cambiar al estado Attacking
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
            agent.isStopped = true; // Detener el movimiento del enemigo
        }
        else
        {
            agent.destination = player.position;

            if (distanceToPlayer > detectionRadius)
            {
                currentState = EnemyState.Searching;
                lastPatrolPoint = patrolPoints[currentPatrolIndex];
            }
        }
    }

    void AttackPlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: false, search: false);

        // Reproducir la animación de ataque
        animator.SetTrigger("Attack");

        // Comprobar si el ataque conecta
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Simular Game Over si el ataque conecta
            Debug.Log("Game Over");
            Time.timeScale = 0;  // Pausar el juego
        }

        // Después del ataque, si el jugador escapa, vuelve a perseguir
        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chasing;
            agent.isStopped = false;  // Reactivar el movimiento
        }
    }

    void SearchForPlayer()
    {
        SetAnimatorStates(idle: false, patrol: false, chase: false, search: true);

        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            currentState = EnemyState.Patrol;
            GoToClosestPatrolPoint();
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

                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (currentState != EnemyState.Chasing && currentState != EnemyState.Attacking)
                        {
                            currentState = EnemyState.Chasing;
                        }
                    }
                }
            }
        }
        else if (currentState == EnemyState.Chasing)
        {
            currentState = EnemyState.Searching;
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

        do
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        } while (patrolPoints[currentPatrolIndex] == lastPatrolPoint);

        lastPatrolPoint = patrolPoints[currentPatrolIndex];
        agent.destination = patrolPoints[currentPatrolIndex].position;
    }

    void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        float minDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        agent.destination = patrolPoints[currentPatrolIndex].position;
    }
}