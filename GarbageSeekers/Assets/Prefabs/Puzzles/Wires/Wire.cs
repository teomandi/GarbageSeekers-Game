using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 startPosition;
    SpriteRenderer wireEnd;
    GameObject lightOn;
    float distance=25f;
    bool lights = false;


    public GameObject respawns;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.parent.position;
        startPosition = transform.position;

        wireEnd =  gameObject.transform.Find("Wire_end").GetComponent<SpriteRenderer>();
        lightOn = gameObject.transform.parent.Find("Light").gameObject;    
    }


    // Update is called once per frame
    private void  OnMouseDrag()
    {
        //mouse position to world point
        Vector3 mousePos = Input.mousePosition;
        float distance2goal = Vector2.Distance(respawns.transform.position, transform.position);
        
        Debug.Log(distance2goal);
        if (distance2goal < distance)
        {
            lights = true;
            UpdateWire(respawns.transform.position);
            WireManager.Instance.SwitchChange(1);
            respawns.GetComponent<Wire>().Done();
            Done();
            return;
        }
        

        UpdateWire(mousePos);
    }

    public void Done()
    {
        //Debug.Log("MPHKA");
        lightOn.SetActive(true);
        Destroy(this);

    }

    private void OnMouseUp()
    {
        //reset wire position
        UpdateWire(startPosition);
        
    }

    void UpdateWire(Vector3 newPosition)
    {
        //update wire
        //update position
        transform.position = newPosition;

        //update direction
        Vector3 direction = newPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;

        float dist = Vector2.Distance(startPoint, newPosition);
        wireEnd.size = new Vector2(dist / 100, wireEnd.size.y);
    }
}
