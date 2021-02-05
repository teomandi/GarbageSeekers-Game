using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    [SerializeField] bool isPlaced = false;
    [SerializeField] PipeManager pipeManager;

    int PossibleRots = 1;
    float[] rotations = { 0, 90, 180, 270 };
    public float[] correctRotaton;

    void Start()
    {
        PossibleRots = correctRotaton.Length;
        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        if (PossibleRots > 1)
        {
            if (transform.eulerAngles.z == correctRotaton[0] || transform.eulerAngles.z == correctRotaton[1])
            {
                isPlaced = true;
                pipeManager.correctMove();
            }
        }
        else
        {
            if (transform.eulerAngles.z == correctRotaton[0])
            {
                isPlaced = true;
                pipeManager.correctMove();
            }
        }

    }


    private void OnMouseDown()
    {
        Debug.Log("Pipe got mMouseDown");
        transform.Rotate(new Vector3(0, 0, 90));
        if (PossibleRots > 1)
        {
            if (transform.eulerAngles.z == correctRotaton[0] || transform.eulerAngles.z == correctRotaton[1] && isPlaced == false)
            {
                isPlaced = true;
                pipeManager.correctMove();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                pipeManager.wrongMove();
            }
        }
        else
        {
            if (transform.eulerAngles.z == correctRotaton[0] && isPlaced == false)
            {
                isPlaced = true;
                pipeManager.correctMove();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                pipeManager.wrongMove();
            }
        }
    }
}
