using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] string puzzleName, quoate;
    [SerializeField] GameObject puzzleObject;
    [SerializeField] GameObject puzzleCamera;


    bool isActive = false, messageOn = false; 
    public bool isComplete = false;

    PlayerController player;
    GameObject playerCamera;


    private void OnTriggerEnter(Collider other)
    {
        if (!messageOn)
        {
            if (other.gameObject.tag == "player")
            {
                player = other.gameObject.GetComponent<PlayerController>();
                if (player.PV.IsMine) //<--------------on multiplayer need fix (should be true)
                {
                    playerCamera = other.transform.Find("CameraHolder").Find("Camera").gameObject;
                    player.SetMessage("Pree E to " + quoate, Color.white);
                    messageOn = true;

                    Debug.Log("mes: " + messageOn);

                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player" && messageOn)
        {
            player.SetMessage("", Color.white);
            messageOn = false;
            //player = null;
        }
    }

    public void Update()
    {
        if(messageOn && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("EEEE WAS CLICKED");
            OpenPuzzle();
            
        }
        if(isActive && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle(false);
        }
    }

    void OpenPuzzle()
    {
        isActive = true;

        player.setPuzzleMode(true);
        playerCamera.SetActive(false);
        puzzleCamera.SetActive(true);

        puzzleObject.SetActive(true);
        Debug.Log("puzzle opened");
    }

    public void ClosePuzzle(bool complete)
    {
        isActive = false;

        player.setPuzzleMode(false);
        playerCamera.SetActive(true);
        puzzleCamera.SetActive(false);

        puzzleObject.SetActive(false);
        Debug.Log("puzzle closed!");

        isComplete = complete;


    }

}
