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

    private static int index = 0;
    private static GameObject[] players = new GameObject[4];
    private static Vector3[] spawners = new Vector3[4];

    public static void RegisterPlayer(GameObject _player, Vector3 _spawner)
    {
        players[index] = _player;
        spawners[index] = _spawner;
        index++;
    }

    public static void RestorePlayer(GameObject _player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] == _player)
                players[i].transform.position = spawners[i];
        }
    }



}
