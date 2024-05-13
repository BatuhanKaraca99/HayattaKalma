using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
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

    [Header("Zombie Standing Variables")]
    public float zombieSpeed = 4f;

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
        healthBar.GiveFullHealth(zombieHealth);
        zombieAgent = GetComponent<NavMeshAgent>();
        capsulecol = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInattackingRadius) Idle();
        else if (playerInvisionRadius && !playerInattackingRadius) PursuePlayer();
        else if (playerInvisionRadius && playerInattackingRadius) AttackPlayer();
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position); //stop zombie
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }

    private void PursuePlayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            //animations
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position); //stop zombie
        transform.LookAt(LookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }

                anim.SetBool("Attacking", true);
                anim.SetBool("Running", false);
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

        if (presentHealth <= 0)
        {
            anim.SetBool("Died", true);
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
        anim.SetBool("Attacking", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Running", false);
        anim.SetBool("Running", false);
        anim.SetBool("Died", true);
        Object.Destroy(gameObject, 5.0f);
        Object.Destroy(healthBar, 0.01f);
    }

}
