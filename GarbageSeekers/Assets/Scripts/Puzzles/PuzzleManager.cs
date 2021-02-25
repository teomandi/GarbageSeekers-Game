using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using UnityEngine.SceneManagement;


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
        if (completePuzzle.All(x => x) || Input.GetKeyDown(KeyCode.H))
        {
            winMesg.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
                Invoke("LoadNextLevel", 3f);
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

    private void LoadNextLevel()
    {
        PhotonNetwork.LoadLevel(3);
    }

}
