using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressController : MonoBehaviour
{
    void Start()
    {
        Transform bread = transform.GetChild(0).transform;
        bread.GetChild(Random.Range(0, bread.childCount - 1)).gameObject.SetActive(true);

        Transform cloth = transform.GetChild(1).transform;
        if (Random.Range(0f, 1f) < 0.5f) { 
            cloth.GetChild(Random.Range(0, 5)).gameObject.SetActive(true);
            cloth.GetChild(Random.Range(7, 19)).gameObject.SetActive(true);
        }
        else
        {
            cloth.GetChild(6).gameObject.SetActive(true);
            cloth.GetChild(6).transform.GetChild(Random.Range(0, 10)).gameObject.SetActive(true);
        }

        Transform hair = transform.GetChild(2).transform;
        hair.GetChild(Random.Range(0, hair.childCount - 1)).gameObject.SetActive(true);




    }
}
