using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageScore : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;

    // Update is called once per frame
    void Update()
    {
        scoreText.GetComponent<Text>().text = "GARBAGES: " + theScore + "/50";
        
    }
}
