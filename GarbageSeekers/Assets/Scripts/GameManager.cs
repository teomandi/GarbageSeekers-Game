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

    public static void RestorePlayer(GameObject _player)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("spawner");
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] == _player)
                players[i].transform.position = spawners[i].transform.position;
        }
    }

    public static void LevelComplete()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");

        for (int i = 0; i < players.Length; i++)
        {
            if(players != null)
                players[i].GetComponent<PlayerController>().SetMessage("Level Completed!!!", Color.green, 55);
        }
        Time.timeScale = 0f;

        // load new level
    }



}
