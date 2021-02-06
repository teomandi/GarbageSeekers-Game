using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;


[RequireComponent(typeof(PhotonView))]
public class PuzzleManager : MonoBehaviour
{

    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject[] puzzleObjects;
    [SerializeField] Material completeMaterial;
    [SerializeField] GameObject winMesg;

    PhotonView PV;
    bool[] completePuzzle;
    int completePuzzlesCounter = 0;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        SetScore();
        completePuzzle = new bool[puzzleObjects.Length];
        for (int i = 0; i < puzzleObjects.Length; i++)
        {
            completePuzzle[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < puzzleObjects.Length; i++)
        {
            if (!completePuzzle[i])
            {
                if (puzzleObjects[i].GetComponent<PuzzleController>().isComplete)
                {
                    completePuzzle[i] = true;
                    PV.RPC("RPCCompletePuzzle", RpcTarget.All, new object[] { i });
                    /*RPCCompletePuzzle(i);*/
                }

            }
        }
        if (completePuzzle.All(x => x))
        {
            winMesg.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
                Invoke("LoadMenu", 10f);
        }


    }

    [PunRPC]
    public void RPCCompletePuzzle(int i)
    {
        completePuzzle[i] = true;
        puzzleObjects[i].GetComponent<Renderer>().material = completeMaterial;
        completePuzzlesCounter += 1;
        SetScore();
    }

    void SetScore()
    {
        scoreText.text = completePuzzlesCounter + "/" + puzzleObjects.Length;
    }

    private void LoadMenu()
    {

        PhotonNetwork.LoadLevel(0);
    }
}
