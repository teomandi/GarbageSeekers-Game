using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    [SerializeField] int freezedDuration;
    [SerializeField] Material icedMaterial;
    [SerializeField] Material myMaterial;
    [SerializeField] AnimationClip attakAnimation;

    Transform target;
    NavMeshAgent agent;
    bool isFreezed, isAttacking;

    void Start()
    {
        target = PlayerTracker.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        isFreezed = false;
        isAttacking = false;
    }

    void Update()
    {
        if (isFreezed)
            return;
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance + 1)
            {
                Debug.Log("In stopping distance");
                //attack the target
                if (!isAttacking)
                {
                    InvokeRepeating("Attack", .5f, 1f);
                    isAttacking = true;
                }
                //face the target
                FaceTarget();
            }
            else
            {
                isAttacking = false;
                if(!isFreezed)
                    CancelInvoke(); //buggy!!
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void Attack()
    {
        Debug.Log("Attack!!!");
        isAttacking = true;
        target.GetComponent<PlayerController>().TakeDamage(10);
    }
    public void Freeze()
    {
        agent.isStopped = true;
        isFreezed = true;
        GetComponentInChildren<Renderer>().material = icedMaterial;
        Invoke("UnFreeze", freezedDuration);
    }

    private void UnFreeze()
    {
        agent.isStopped = false;
        isFreezed = false;
        GetComponentInChildren<Renderer>().material = myMaterial;
    }
}
