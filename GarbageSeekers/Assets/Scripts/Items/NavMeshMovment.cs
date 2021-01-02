using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovment : MonoBehaviour
{
    Camera cam;
    public LayerMask movmentMask;
    public Animator ani;

    NavMeshAgent agent;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.Log("CAM IS NULL");
        agent = GetComponent<NavMeshAgent>();
        ani.SetInteger("legs", 5);
        ani.SetInteger("arms", 5);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movmentMask))
            {
                //move the agent
                agent.SetDestination(hit.point);
                ani.SetInteger("legs", 1);
                ani.SetInteger("arms", 1);
            }
        }
        float distance = Vector3.Distance(agent.destination, transform.position);
        if(distance < agent.stoppingDistance + 1)
        {
            ani.SetInteger("legs", 5);
            ani.SetInteger("arms", 5);
        }
    }
}
