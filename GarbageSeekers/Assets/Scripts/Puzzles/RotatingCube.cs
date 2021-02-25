using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    Vector3 rotationOffset;

    private void Awake()
    {
        rotationOffset = new Vector3(Random.Range(10, 90), Random.Range(10, 90), Random.Range(10, 90));
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationOffset * Time.deltaTime);
    }
}
