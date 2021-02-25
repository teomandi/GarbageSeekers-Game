using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animation = GetComponent<Animator>();

        animation.StopPlayback();
        animation.Play("AnimationName");

        
    }


}
