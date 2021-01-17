using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] garbagePrefabs;
    [SerializeField] float period, possibility;
    [SerializeField] int minThrowForce, maxThrowForce;


    private void Start()
    {
        InvokeRepeating("ThrowGarbage", period, period);
    }

    void ThrowGarbage()
    {
        float chanse = Random.Range(0f, 1f);
        if(chanse > possibility)
        {
            return;
        }

        // select object
        int idx = Random.Range(0, garbagePrefabs.Length - 1);
        GameObject garbage = Instantiate(garbagePrefabs[idx]) as GameObject;
        garbage.transform.position = transform.position;

        int throwForce = Random.Range(minThrowForce, maxThrowForce);
        Rigidbody rb = garbage.GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogWarning("No rigidbody on trash named::: " + garbage.name);
            return;
        }

        garbage.transform.rotation = Random.rotation;
/*        Debug.Log("Throwing garbage with force: " + throwForce);*/

        rb.AddForce(throwForce * new Vector3(1, 1, Random.Range(-1f, 1f)));
    }


}
