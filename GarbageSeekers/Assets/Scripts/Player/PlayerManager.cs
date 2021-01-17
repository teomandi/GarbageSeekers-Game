using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    int index;
    GameObject[] playerSpawners;
    GameObject[] players;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        index = 0;
        playerSpawners = GameObject.FindGameObjectsWithTag("spawner");


    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
            Debug.Log("Player " + PhotonNetwork.NickName + " was setup");
        }
        index++;
        if (index <= playerSpawners.Length)
            index = 0;  // do the cycle
    }
    void CreateController() //create a player
    {
        float x = playerSpawners[index].transform.position.x;
        float y = playerSpawners[index].transform.position.y + 3;
        float z = playerSpawners[index].transform.position.z;
        Debug.Log("Instantiated PlayerController");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), new Vector3(x, y, z), Quaternion.identity);
        
    }
}
