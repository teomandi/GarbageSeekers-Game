using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerController : ItemController
{
    public float range;

    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.green);
    }
    public override void StartInteraction()
    {
        base.StartInteraction();

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, transform.forward, out _hit, range))
        {
            Debug.Log("You hit " + _hit.transform.name);
            // we hit somehting
            if(_hit.transform.tag == "freezable")
            {
                _hit.transform.GetComponent<ItemFreeze>().Freeze();
            }
            else if (_hit.transform.tag == "human")
            {
                _hit.transform.GetComponent<HumanFreeze>().Freeze();
            }
        }
    }
}
