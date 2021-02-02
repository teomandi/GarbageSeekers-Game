using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarbageCounter : MonoBehaviour
{
    public int totalGarbage = 200;
    public static int currentGarbage = 0;
    [SerializeField] TMP_Text garbageText = null;
    bool complete = false;


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

/*    public static void SetGarbageUI(TMP_Text _garbageText)
    {
        garbageText = _garbageText;
    }*/

    void Update()
    {
        if(garbageText != null)
            garbageText.text = "Garbage: " + currentGarbage + "/" + totalGarbage;
        if (currentGarbage < 0)
            currentGarbage = 0;
        if (Input.GetKeyDown(KeyCode.P))
            currentGarbage += 10;
        if(currentGarbage > totalGarbage && !complete)
        {
            complete = true;
            GameManager.LevelComplete();
        }
    }
}
