using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarbageCounter : MonoBehaviour
{
    public int totalGarbage = 200;
    public static int currentGarbage = 0;
    [SerializeField] TMP_Text garbageText;


/*    private void Start()
    {
        SetText();
    }

    public void IncreaseCounter(int value)
    {
        currentGarbage += value;
        SetText();
    }

    private void SetText()
    {
        garbageText.text = "Garbage: " + currentGarbage + "/" + totalGarbage;
    }*/

    void Update()
    {
        garbageText.text = "Garbage: " + currentGarbage + "/" + totalGarbage;
    }
}
