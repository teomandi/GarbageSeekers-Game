using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] int freezedDuration;
    [SerializeField] Material icedMaterial;
    [SerializeField] Material myMaterial;

    public void Freeze()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if( renderer != null)
        {
            renderer.material = icedMaterial;
            Invoke("UnFreeze", freezedDuration);
        }
    }

    public void UnFreeze()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = myMaterial;
        }
    }
}
