using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerController : ItemController
{
    public float range;

    [SerializeField] LayerMask mask;
    [SerializeField] Material icedMaterial;


    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
    }
    public override void StartInteraction()
    {
        base.StartInteraction();

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, transform.forward, out _hit, range, mask))
        {
            Debug.Log("You hit " + _hit.transform.name);
            // we hit somehting
            _hit.transform.gameObject.GetComponent<Renderer>().material = icedMaterial;
        }
    }
}
