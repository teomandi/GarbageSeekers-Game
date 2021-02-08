using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(PhotonView))]
public class Dialog : MonoBehaviour { 

    public TextMeshProUGUI textDisplay;
    [SerializeField] GameObject voteDialog;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    [SerializeField] TMP_Text voteMsg; 

    int vote=0, votedPlayers=0;
    PhotonView PV;

    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        PV = gameObject.GetComponent<PhotonView>();
        StartCoroutine(Type());

    }

    void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        if ( index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            voteDialog.SetActive(true);
        }
    }

    
    public void SetVote(bool playerVote)
    {
        int myVote = playerVote ? 1 : -1;
        PV.RPC("RPCApplyVote", RpcTarget.All, new object[] { myVote });

        voteDialog.SetActive(false);
    }

    [PunRPC]
    void RPCApplyVote(int laVote)
    {
        vote += laVote;
        votedPlayers += 1;

        if(PhotonNetwork.CountOfPlayers == votedPlayers)
        {
            if(vote >= 0)
            {
                //ok
                voteMsg.text = "Voting Completed! You agreed to help the humans.";
                voteMsg.color = Color.green;
            }
            else
            {
                //no
                voteMsg.text = "Voting Completed! You refused.";
                voteMsg.color = Color.red;
            }

            Invoke("LoadMenu", 5f);


        }


    }

    private void LoadMenu()
    {
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }




}
