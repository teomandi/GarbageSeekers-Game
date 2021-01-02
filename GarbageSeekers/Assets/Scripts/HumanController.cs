using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class HumanController : MonoBehaviour
{
    public float lookRadius = 10f;

    Animator animator;
    NavMeshAgent agent;
    Transform currentTarget;

    bool isAttacking;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        isAttacking = false;

        //idle
        animator.SetInteger("legs", 5);
        animator.SetInteger("arms", 5);
    }

    void Update()
    {
/*        if (isFreezed)
            return;*/
        Transform target = GetClosestPlayer();
        currentTarget = target;
        if (target == null)
            return;
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            animator.SetInteger("legs", 1);
            animator.SetInteger("arms", 1);

            if (distance <= agent.stoppingDistance + 1)
            {
                Debug.Log("In stopping distance");
                //attack the target
                if (!isAttacking)
                {
                    isAttacking = true;
                    InvokeRepeating("Attack", .5f, 1f);
                }
                //face the target
                FaceTarget(target);
            }
            else
            {
                isAttacking = false;
                CancelInvoke();
            }
        }
        else
        {
            animator.SetInteger("legs", 5);
            animator.SetInteger("arms", 5);
        }
    }


    Transform GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player"); //maybe in start (?) maybe each player registers himself (?) maybe use a list (?)
        if (players.Length == 0)
            return null;
        GameObject closestPlayer = players[0];
        float minDistance = float.MaxValue, distance;
        foreach (GameObject player in players)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < minDistance)
            {
                closestPlayer = player;
                minDistance = distance;
            }
        }
        return closestPlayer.transform;
    }


    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 90, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }


    private void Attack(/*Transform target*/)
    {
        isAttacking = true;
        animator.SetInteger("arms", 14);
        if(currentTarget!=null)
            currentTarget.GetComponent<PlayerController>().TakeDamage(10);
        Debug.Log("Attack!!!");
    }

    public void applyStop(bool _isStopped)
    {
        agent.isStopped = _isStopped;
        if (!_isStopped)
            animator.StopPlayback();
        else
            animator.StartPlayback();
    }
}
