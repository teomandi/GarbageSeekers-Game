using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static int currentHobbyId = 0;

    public static int GetHobbyID()
    {
        currentHobbyId++;
        return currentHobbyId;
    }
}
