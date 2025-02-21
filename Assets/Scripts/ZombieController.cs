using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    
    private Animator animator;
    private int health = 100;

    private Transform player;
    private NavMeshAgent zombie;

    public LayerMask whatIsGround;
    
    // Patrolling
    private Vector3 walkPoint;
    private bool walkPointSet;
    
    // Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float walkPointRange;

    private bool playerInSightRange, playerInAttackRange;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        zombie = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        Vector3 zombieCurrentPosition = transform.position;
        
        // Check if player is in sight or attack range
        playerInSightRange = Physics.CheckSphere(zombieCurrentPosition, sightRange, LayerMask.GetMask("player"));
        playerInAttackRange = Physics.CheckSphere(zombieCurrentPosition, attackRange, LayerMask.GetMask("player"));

        if (health <= 0) return;
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            zombie.speed = 0.5f;
            zombie.SetDestination(walkPoint);
            animator.Play("walk");
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 zombieCurrentPosition = transform.position;

        walkPoint = new Vector3(zombieCurrentPosition.x + randomX, zombieCurrentPosition.y, zombieCurrentPosition.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }
    
    private void AttackPlayer()
    {
        zombie.SetDestination(transform.position);
        
        transform.LookAt(player);
        
        animator.Play("atack1");

    }
    
    private void ChasePlayer()
    {
        Vector3 distanceToPlayer = transform.position - player.position;

        zombie.speed = 2f;
        
        if (distanceToPlayer.magnitude < 1.5f)
        {   
            zombie.SetDestination(transform.position);
        }
        else
        {
            animator.Play("run");
            zombie.SetDestination(player.position);  
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        health -= 30;

        animator.Play(health <= 0 ? "death2" : "gethit");
    }
}