using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMineObj : MonoBehaviour
{
    private Vector3 wayPointPos;
    private GameObject wayPoint;

    //This will be the mine item speed. Adjust as necessary.
    [SerializeField]private float speed = 10.0f;

    GameObject GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player"); //maybe in start (?) maybe each player registers himself (?) maybe use a list (?)
        if (players.Length == 0)
            return null;
        GameObject closestPlayer = players[0];
        float minDistance = float.MaxValue, distance;
        foreach (GameObject player in players)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                closestPlayer = player;
                minDistance = distance;
            }
        }
        return closestPlayer;
    }

    void Start()
    {
        wayPoint = GetClosestPlayer();
    }


    void Update()
    {
        wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
        
        // the item will follow the waypoint.
        transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, wayPointPos) < 2)
        {
            Destroy(gameObject);
        }
    }
}
