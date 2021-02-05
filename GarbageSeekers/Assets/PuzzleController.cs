using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] string name, quoate;
    [SerializeField] GameObject puzzleObject;
    [SerializeField] Camera puzzleCamera;
    bool isActive = false, messageOn = false;
    private PlayerController player;
    private Camera playerCamera;


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
                    playerCamera = other.transform.Find("CameraHolder").Find("Camera").GetComponent<Camera>();
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
        if(isActive && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    void OpenPuzzle()
    {
        isActive = true;

        player.playingPuzzle = true;
        playerCamera.enabled = false;
        puzzleCamera.enabled = true;

        puzzleObject.SetActive(true);
        Debug.Log("puzzle opened");
    }

    void ClosePuzzle()
    {
        isActive = false;

        player.playingPuzzle = false;
        playerCamera.enabled = true;
        puzzleCamera.enabled = false;

        puzzleObject.SetActive(false);
        Debug.Log("puzzle closed!");
    }




}
