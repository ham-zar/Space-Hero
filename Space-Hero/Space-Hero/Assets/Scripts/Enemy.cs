using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    NavMeshPath Path;
    Vector3 target;

    bool VaildPath;
    bool playerInSightRange, playerInAttackRange;
    bool alreadyAttacked;
    bool Incountine;

    public float timerForNewPath;
    public float xPosition, negativeXPosition;
    public float zPosition, negativeZPosition;
    public float sightRange, attackRange;
    public float timeBetweenAttacks;
    public float force;

    public GameObject projectile;
    public LayerMask whatIsPlayer, whatIsGround;
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Path = new NavMeshPath();
        GetNewPath();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !Incountine) StartCoroutine(DoSomething());
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(negativeZPosition, zPosition);
        float randomX = Random.Range(negativeXPosition, xPosition);

        target = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    IEnumerator DoSomething()
    {
        Incountine = true;
        yield return new WaitForSeconds(timerForNewPath);
        GetNewPath();

        VaildPath = navMeshAgent.CalculatePath(target, Path);

        while (!VaildPath)
        {
            yield return new WaitForSeconds(0.05f);
            GetNewPath();
            VaildPath = navMeshAgent.CalculatePath(target, Path);
        }
        Incountine = false;
    }

    void GetNewPath()
    {
        SearchWalkPoint();
        navMeshAgent.SetDestination(target);
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
    void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked)
        {

            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
