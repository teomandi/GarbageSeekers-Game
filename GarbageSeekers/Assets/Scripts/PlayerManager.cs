using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField] GameObject healthBarGO;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
            Debug.Log("Player " + PhotonNetwork.NickName + " was setup");
        }
    }
    void CreateController()
    {
        Debug.Log("Instantiated PlayerController");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);//create a player
    }
}
