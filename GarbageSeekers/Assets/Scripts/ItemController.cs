using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] ParticleSystem visualEffect;
    public void StartInteraction()
    {
        if (visualEffect != null)
            visualEffect.Play();
    }
    public void StopInteraction()
    {
        if (visualEffect != null)
            visualEffect.Stop();
    }
}
