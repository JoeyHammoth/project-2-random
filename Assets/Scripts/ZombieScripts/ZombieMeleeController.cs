using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieMeleeController : MonoBehaviour
{
    
    private Animator animator;

    private Transform player;
    private NavMeshAgent zombie;

    public LayerMask whatIsGround;
    
    // Patrolling
    private Vector3 walkPoint;
    private bool walkPointSet;
    
    // Attacking
    [SerializeField] private float timeBetweenAttacks;
    private float time;
    private bool alreadyAttacked;

    [SerializeField] private float maxHealth;
    [SerializeField] private float zombieSpeed;
    [SerializeField] private int damageToPlayer;
    
    private float health;
    private HealthBar healthBar;
    
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float walkPointRange;

    [SerializeField] private LootTable lootTable;

    // Attributes pertaining to zombie shaders
    [SerializeField] private Shader intialShader;
    [SerializeField] private Shader finalShader;

    // Initial Texture Setting
    [SerializeField] private Texture underlyingText;
    [SerializeField] private Texture bloodText;
    [SerializeField] private Texture emissiveText;
    [SerializeField] private Texture maskText;


    [SerializeField] private String zombieType;
    private GameObject root;
    private Renderer render;
    private bool isDead = false;
    private bool deathAnimRunning = false;

    [SerializeField] private float initialRevealValue = 2.0f;
    [SerializeField] private float initialFeather = 0.5f;
    [SerializeField] private float revealValueIncrement = 0.01f;

    // Attributes pertaining to randomizing parameters for initial zombie shader
    [SerializeField] private bool randomizeTexture = true;
    [SerializeField] private bool randomizeBlood = true;
    [SerializeField] private bool randomizeBloodColor = true;
    [SerializeField] private bool randomizeBloodAmount = true;


    private bool playerInSightRange, playerInAttackRange;
    

    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        this.health = maxHealth;
        this.healthBar = GetComponent<HealthBar>();
        // based on zombie type, use the specific name for the child.
        root = transform.Find(zombieType).gameObject;
        render = root.GetComponent<Renderer>(); // getting the child gameObject from the prefab
        render.material.shader = intialShader; // setting the shader to the initial shader for the child gameObject

        // Setting all the initial textures for initial shader
        render.material.SetTexture("_Texture", underlyingText);
        render.material.SetTexture("_Blood", bloodText);
        render.material.SetTexture("_Emissive", emissiveText);

        // Additional customization
        if (randomizeTexture) {
            render.material.SetTextureScale("_Texture", new Vector2(Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f)));
            render.material.SetTextureOffset("_Texture", new Vector2(Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f)));
        }
        if (randomizeBlood) {
            render.material.SetTextureScale("_Blood", new Vector2(Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f)));
            render.material.SetTextureOffset("_Blood", new Vector2(Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f)));
        }
        if (randomizeBloodColor) {
            render.material.SetColor("_BloodColor", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }
        if (randomizeBloodAmount) {
            render.material.SetFloat("_BloodAmount", Random.Range(0.0f, 1.0f));
        }
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

        if (health <= 0) {
            // When zombie has <=0 health, disable navmeshagent and collider, and set shader to final shader. 
            if (!isDead) {
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Collider>().enabled = false;
                render.material.shader = finalShader;

                // Set initial parameters for decay shader
                render.material.SetFloat("_RevealValue", initialRevealValue);
                render.material.SetFloat("_Feather", initialFeather);
                render.material.SetFloat("_DecayEffect", 0);
                
                // Set initial textures for decay shader
                render.material.SetTexture("_MainTex", underlyingText);
                render.material.SetTexture("_MaskTex", maskText);

                isDead = true;
                deathAnimRunning = true;
            } else {
                // Initiate death animation and decrement reveal value to start decay.
                if (deathAnimRunning) {
                    if (initialRevealValue > -0.5f) {
                        initialRevealValue -= revealValueIncrement;
                        render.material.SetFloat("_RevealValue", initialRevealValue);
                    } else {
                        deathAnimRunning = false;
                    }
                } else {
                    Destroy(gameObject, 4f);
                }
            }
            return;
        } 

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        
        if (time <= timeBetweenAttacks)
        {
            time += Time.deltaTime;
        }
        else
        {
            alreadyAttacked = false;
            time = 0f;
        }
        
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
            zombie.speed = zombieSpeed;
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
        
        if (alreadyAttacked) return;
        
        zombie.SetDestination(transform.position);
        
        transform.LookAt(player);
        
        animator.Play("attack");
        
        GameObject.FindWithTag("Player").GetComponent<PlayerHealthManager>().TakeDamage(damageToPlayer);
        
        alreadyAttacked = true;

    }
    
    private void ChasePlayer()
    {
        
        Vector3 distanceToPlayer = transform.position - player.position;

        zombie.speed = zombieSpeed;
        
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
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= other.gameObject.GetComponent<BulletController>().getDamage();
            healthBar.UpdateHealth(health, maxHealth);

            if (health <= 0)
            {
                DropLoot();
            }
            animator.Play(health <= 0 ? "death" : "gethit");
        }
    }

    public void takeGrenadeDamage(int damage)
    {
        Debug.Log("damage received");
        health -= damage;
        healthBar.UpdateHealth(health, maxHealth);
        if (health <= 0)
        {
            DropLoot();
        }
        animator.Play(health <= 0 ? "death" : "gethit");
        
    }
    
    
    // we first select random item from list, then check its drop rate
    private void DropLoot()
    {
        int randomIndex = Random.Range(0, lootTable.lootList.Count);
        Loot loot = lootTable.lootList[randomIndex];
        
        if (Random.Range(0, 100) <= loot.dropRate)
        {
            Instantiate(loot.lootPrefab, transform.position, Quaternion.identity);
        }
    }

}