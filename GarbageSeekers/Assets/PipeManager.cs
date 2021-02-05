using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public GameObject PipesHolder;
    public GameObject[] Pipes;

    [SerializeField]  int totalPipes = 0;
    [SerializeField]  int correctPipes = 0;

    void Start()
    {
        totalPipes = PipesHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];

        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipesHolder.transform.GetChild(i).gameObject;
        }
    }

    public void correctMove()
    {
        correctPipes += 1;
        if (correctPipes == totalPipes)
        {
            Debug.Log("You win");
        }
    }

    public void wrongMove()
    {
        correctPipes -= 1;
    }
}
