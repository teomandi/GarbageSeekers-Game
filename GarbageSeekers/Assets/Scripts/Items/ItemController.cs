using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] ParticleSystem visualEffect;
    public virtual void StartInteraction()
    {
        if (visualEffect != null)
            visualEffect.Play();
    }
    public virtual void StopInteraction()
    {
        if (visualEffect != null)
            visualEffect.Stop();
    }
}
