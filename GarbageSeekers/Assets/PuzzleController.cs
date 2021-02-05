using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] string name, quoate;
    [SerializeField] GameObject puzzleObject;
    
    bool isActive = false, messageOn = false;
    PlayerController player;
    GameObject playerCamera;
    [SerializeField] GameObject puzzleCamera;

    private void Awake()
    {
        /*puzzleCamera = GameObject.Find("PuzzleCamera");*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!messageOn)
        {
            if (other.gameObject.tag == "player")
            {
                Debug.Log("Platyer Entered the trigger!!!");
                player = other.gameObject.GetComponent<PlayerController>();

                if (!player.PV.IsMine) //<--------------on multiplayer need fix
                {
                    playerCamera = other.transform.Find("CameraHolder").Find("Camera").gameObject;
                    player.SetMessage("Pree E to " + quoate, Color.white);
                    messageOn = true;
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
            player = null;
        }
    }

    public void Update()
    {
        if(messageOn && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
            
        }
        if(isActive && Input.GetKeyDown(KeyCode.Escape) && false)
        {
            ClosePuzzle();
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

    void ClosePuzzle()
    {
        isActive = false;

        player.setPuzzleMode(false);
        playerCamera.SetActive(true);
        puzzleCamera.SetActive(false);

        puzzleObject.SetActive(false);
        Debug.Log("puzzle closed!");
    }




}
