using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class GarbageManager : MonoBehaviour
{
    public int totalGarbage = 200;
    public int currentGarbage = 0;

    [SerializeField] TMP_Text garbageText = null;

    bool complete = false;
    private PhotonView PV; 

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }


    public void IncreaseGarbage(int _amount)
    {
        //currentGarbage += _amount;
        PV.RPC("RPCSetGarbage", RpcTarget.All, new object[] { _amount } );
    }

    [PunRPC]
    public void RPCSetGarbage(int _garbage)
    {
        Debug.Log("1--> Currnet Garbage: " + currentGarbage);
        Debug.Log("adding Garbage: " + _garbage);
        currentGarbage += _garbage;
        Debug.Log("2--> Currnet Garbage: " + currentGarbage);

    }

    void Update()
    {
        if(garbageText != null)
            garbageText.text = "Garbage: " + currentGarbage + "/" + totalGarbage;
        if (currentGarbage < 0)
            currentGarbage = 0;
        if (Input.GetKeyDown(KeyCode.P))
        {
            PV.RPC("RPCSetGarbage", RpcTarget.All, new object[] { 10 });
        }
        if (currentGarbage > totalGarbage && !complete)
        {
            complete = true;
            GameManager.LevelComplete();
        }
    }
}
