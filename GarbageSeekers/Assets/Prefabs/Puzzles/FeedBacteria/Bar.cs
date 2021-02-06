using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Bar : MonoBehaviour
{
    public GameObject bar;
    public GameObject image;
    public int time;
    [SerializeField] PuzzleController controller;

    public void AnimateBar()
    {
        LeanTween.moveX(image, 1000, time);
        LeanTween.scaleX(bar, 1, time).setOnComplete(Message);
    }

    public void Message()
    {
        Debug.Log("END");
        controller.ClosePuzzle(true);
    }
}
