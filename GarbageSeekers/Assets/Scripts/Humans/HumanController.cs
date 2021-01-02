using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class HumanController : MonoBehaviour
{
    [SerializeField] float lookRadius = 10f;
    [SerializeField] string currentState;
    [SerializeField] int attackRange;

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

        SetState(currentState);

    }

    void Update()
    {
        currentTarget = GetClosestPlayer();
        if (currentTarget == null)
            return;

        
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(currentTarget.position);
            if (!isAttacking)
            {
                SetState("run");
            }

            if (distance <= agent.stoppingDistance + attackRange)
            {
                //face the target
                FaceTarget(currentTarget);

                //attack the target
                if (!isAttacking)
                {
                    isAttacking = true; //there
                    InvokeRepeating("Attack", .5f, 1f);
                }
            }
            else
            {
                isAttacking = false;
                CancelInvoke();
            }
        }
        else
        {
            if (currentState != "idle")
            {
                SetState("idle");
                isAttacking = false;
            }
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }


    private void Attack(/*Transform target*/)
    {
        SetState("attack");
        if (currentTarget!=null)
            currentTarget.GetComponent<PlayerController>().TakeDamage(10);
        Debug.Log("Attack!!!");
    }

    public void applyStop(bool _isStopped)
    {
        agent.isStopped = _isStopped;
        isAttacking = false;
        CancelInvoke();
        agent.SetDestination(transform.position);

        if (!_isStopped)
            animator.StopPlayback();
        else
            animator.StartPlayback();
    }

    public void SetState(string _state)
    {
        switch (_state)
        {
            case "idle":
                currentState = "idle";
                animator.SetInteger("legs", 5);
                animator.SetInteger("arms", 5);
                break;
            case "walk":
                currentState = "idle"; //walking is also idle (like smoking or sitting)
                animator.SetInteger("legs", 1);
                animator.SetInteger("arms", 1);
                break;
            case "attack":
                currentState = "attack";
                animator.SetInteger("legs", 5);
                animator.SetInteger("arms", 14);
                break;
            case "run":
                currentState = "run";
                animator.SetInteger("legs", 2);
                animator.SetInteger("arms", 2);
                break;
            case "sit":
                currentState = "idle"; //siting is also idle (like smoking or sometimes walking)
                animator.SetInteger("legs", 3);
                animator.SetInteger("arms", 3);
                break;

            default:
                break;
        }
    }

    public void RandomWalk()
    {
        //set random points to walk to
    }
}
