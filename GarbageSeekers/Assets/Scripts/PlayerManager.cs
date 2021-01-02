using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    int spawnerIndex;
    GameObject[] playerSpawners;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        spawnerIndex = 0;
        playerSpawners = GameObject.FindGameObjectsWithTag("spawner");

    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
            Debug.Log("Player " + PhotonNetwork.NickName + " was setup");
        }
        spawnerIndex++;
        if (spawnerIndex <= playerSpawners.Length)
            spawnerIndex = 0;
    }
    void CreateController()
    {
        float x = playerSpawners[spawnerIndex].transform.position.x;
        float z = playerSpawners[spawnerIndex].transform.position.z;
        Debug.Log("Instantiated PlayerController");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), new Vector3(x, 0, z), Quaternion.identity);//create a player
    }
}
