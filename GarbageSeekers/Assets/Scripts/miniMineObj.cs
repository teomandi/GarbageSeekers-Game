using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMineObj : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject wayPoint;
    private Vector3 wayPointPos;
    //This will be the mine item speed. Adjust as necessary.
    private float speed = 10.0f;
    void Start()
    {
        wayPoint = GameObject.Find("ItemHolder");///exei 8ema auto
        //GetComponent<Rigidbody>().velocity = new Vector3(gun.transform.position.x, gun.transform.position.y, gun.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);
        //Here, the item will follow the waypoint.
        transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, wayPointPos) < 2)
        {
            Destroy(gameObject);
        }
    }
}
