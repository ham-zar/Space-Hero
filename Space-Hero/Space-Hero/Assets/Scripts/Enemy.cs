using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBetweenAttack;
    public float attackRange;
    public float attackForce;

    private bool isPlayerInRange;
    private bool alreadyAttack;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        isPlayerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        AttackPlayer();
    }
    
    void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttack && isPlayerInRange)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);

            alreadyAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }
    void ResetAttack()
    {
        alreadyAttack = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
