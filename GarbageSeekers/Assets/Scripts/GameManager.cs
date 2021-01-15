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
    private static GameObject[] players;
    private static GameObject[] spawners;

/*    public static void RegisterPlayer(GameObject _player, Vector3 _spawner)
    {
        players[index] = _player;
        spawners[index] = _spawner;
        index++;
    }*/

    public void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("player");
        spawners = GameObject.FindGameObjectsWithTag("spawner");
    }

    public static void RestorePlayer(GameObject _player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] == _player)
                players[i].transform.position = spawners[i].transform.position;
        }
    }

    public static void LevelComplete()
    {
        Debug.Log("~~~~~~>" + players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            if(players != null)
                players[i].GetComponent<PlayerController>().SetMessage("Level Complete!!!", Color.green, 55);
        }

        // load new level
    }



}
