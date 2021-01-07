using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    [SerializeField] GameObject garbagePrefab;
    [SerializeField] float period=0.05f, possibility=1;
    [SerializeField] int minThrowForce=400, maxThrowForce=800;


    private void Start()
    {
        InvokeRepeating("ThrowGarbage", period, period);
    }

    void ThrowGarbage()
    {
        GameObject garbage = Instantiate(garbagePrefab) as GameObject;
        garbage.transform.position = transform.position;
        int throwForce = Random.Range(minThrowForce, maxThrowForce);
        Rigidbody rb = garbage.GetComponent<Rigidbody>();

        garbage.transform.rotation = Random.rotation;

        Debug.Log(throwForce);

        rb.AddForce(throwForce * new Vector3(1, 1, Random.Range(-1f, 1f)));
    }


}
