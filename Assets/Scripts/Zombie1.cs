using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    public float giveDamage = 5f;


    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Guarding Variables")]
    public GameObject[] walkPoints;
    int currentZombiePosition = 0;
    public float zombieSpeed = 4f;
    float walkingPointRadius = 2f;

    [Header("Zombie Attacking Variables")]
    public float timeBetweenAttack = 1f;
    bool previouslyAttack = false;

    [Header("Zombie Mood/States")]
    public float visionRadius = 15f;
    public float attackingRadius = 0.7f;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;

    private void Awake()
    {
        zombieAgent = GetComponent<NavMeshAgent>();
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
        zombieAgent.SetDestination(playerBody.position);
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
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }

    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }
}
