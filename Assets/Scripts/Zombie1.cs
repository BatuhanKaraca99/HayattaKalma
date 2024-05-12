using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public CapsuleCollider capsulecol;

    [Header("Zombie Guarding Variables")]
    public GameObject[] walkPoints;
    int currentZombiePosition = 0;
    public float zombieSpeed = 4f;
    float walkingPointRadius = 2f;

    [Header("Zombie Attacking Variables")]
    public float timeBetweenAttack = 1.5f;
    bool previouslyAttack = false;

    [Header("Zombie Animation")]
    public Animator anim;

    [Header("Zombie Mood/States")]
    public float visionRadius = 15f;
    public float attackingRadius = 0.7f;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;

    private void Awake()
    {
        presentHealth = zombieHealth;
        healthBar.GiveFullHealth(presentHealth);
        zombieAgent = GetComponent<NavMeshAgent>();
        capsulecol = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if(!playerInvisionRadius && !playerInattackingRadius) Guard();
        else if(playerInvisionRadius && !playerInattackingRadius) PursuePlayer();
        else if (playerInvisionRadius && playerInattackingRadius) AttackPlayer();
    }

    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentZombiePosition].transform.position,transform.position) < walkingPointRadius) 
            //is zombie closer to walk point
        {
            currentZombiePosition = Random.Range(0,walkPoints.Length); //find another walk point
            if(currentZombiePosition >= walkingPointRadius)
            {
                currentZombiePosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        //change zombie facing
        transform.LookAt(walkPoints[currentZombiePosition].transform.position);
    }

    private void PursuePlayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            zombieAnimationControl(false, true, false, false);
        }
        else
        {
            zombieAnimationControl(false, false, false, true);
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position); //stop zombie
        transform.LookAt(LookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward,out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }

                zombieAnimationControl(false, false, true, false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }

    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            zombieAnimationControl(false,false,false,true);
            zombieDie();
        }
    }

    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        capsulecol.radius = 0f;
        capsulecol.height = 0f;
        Object.Destroy(gameObject,5.0f);
        Object.Destroy(healthBar, 0.01f);
    }

    private void zombieAnimationControl(bool walking,bool running,bool attacking,bool died)
    {
        //animations
        anim.SetBool("Walking", walking);
        anim.SetBool("Running", running);
        anim.SetBool("Attacking", attacking);
        anim.SetBool("Died", died);
    }
}
