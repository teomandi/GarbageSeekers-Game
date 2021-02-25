using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    static public WireManager Instance;
    [SerializeField] PuzzleController controller;

    private int switchCount = 4;
    private int onCount = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void SwitchChange(int points)
    {
        Debug.Log(switchCount + " -- " + onCount);
        onCount = onCount + points;
        if (onCount == switchCount)
        {
            controller.ClosePuzzle(true);
        }
    }
}
