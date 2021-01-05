using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class HumanController : MonoBehaviour
{
    [SerializeField] float lookRadius = 10f, walkingSpeed=3.5f, runningSpeed = 5f;
    [SerializeField] string currentState;
    [SerializeField] int attackRange;

    Animator animator;
    NavMeshAgent agent;
    Transform currentTarget;
    bool isAttacking, isApplyingHobby;

    // Hobby
    [SerializeField] bool isWalking, isSiting, isRunning, isChilling;
    [SerializeField] Transform movePointA, movePointB, relaxPoint;
    Vector3 targetPoint;


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
        ValidateHobby();
    }

    void Update()
    {
        currentTarget = GetClosestPlayer();
        if (currentTarget == null)
            return;
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (distance <= lookRadius)
        {
            isApplyingHobby = false;
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
            isAttacking = false;
            if (!isApplyingHobby)
                ApplyHobby();
            else if(isRunning || isWalking)
            {
                float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPoint.x, 0, targetPoint.z));
                if(dist <= agent.stoppingDistance)
                {
                    ChangeTargertPoint();
                }
            }
            else if (isSiting || isChilling)
            {
                float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(relaxPoint.position.x, 0, relaxPoint.position.z));
                if (dist <= 2)
                {
                    ApplyResting();
                }
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
        //agent.SetDestination(transform.position);
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
                agent.speed = walkingSpeed;
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
                agent.speed = runningSpeed;
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

    void ValidateHobby() //error checking for hobby
    {
        if (isWalking || isRunning)
        {
            if (movePointA == null || movePointB == null)
            {
                Debug.LogError("Agent is not set correctly! Missing movePoing");
                isChilling = true;
                relaxPoint = transform;
            }
        }
        else if (isChilling || isSiting)
        {
            if (relaxPoint == null)
            {
                Debug.LogError("Agent is not set correctly! Missing relaxPoint");
                isChilling = true;
                relaxPoint = transform;
            }
        }
    }

    void ApplyHobby()
    {
        isApplyingHobby = true;
        if (isWalking || isRunning)
        {
            ApplyMoving();
        }
        else if (isChilling || isSiting)
        {
            GoToSpot();
        }
    }
    void ApplyMoving()
    {
        float distanceA = Vector3.Distance(transform.position, movePointA.position);
        float distanceB = Vector3.Distance(transform.position, movePointB.position);
        targetPoint = distanceA < distanceB ? movePointA.position : movePointB.position;
        agent.SetDestination(targetPoint);
        if (isWalking)
            SetState("walk");
        else if (isRunning)
            SetState("run");
    }

    void ChangeTargertPoint()
    {
        targetPoint = targetPoint == movePointA.position ? movePointB.position : movePointA.position;
        agent.SetDestination(targetPoint);
    }

    void GoToSpot()
    {
        agent.SetDestination(relaxPoint.position);
        SetState("walk");
    }

    void ApplyResting()
    {
        if (isSiting)
            SetState("sit");
        else
            SetState("idle");
    }
}
