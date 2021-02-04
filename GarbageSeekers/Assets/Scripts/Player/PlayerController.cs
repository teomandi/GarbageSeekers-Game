using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using System.IO;


public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Item[] items;
    [SerializeField] int maxHealth, deathPenalty;
    [SerializeField] int currentHealth, minimumY;
    public bool playingPuzzle = false;
    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    public PhotonView PV;

    HealthBar healthBar;
    GarbageManager garbageManager;
    TMP_Text messageUI;

    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] GameObject crossHairPrefab;
    [SerializeField] GameObject garbageManagerPrefab;
    [SerializeField] GameObject playerMessagePrefab;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        SetupPlayerUI(); //<-------------------------only for test
        EquipItem(0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PV.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            /*Cursor.visible = false;*/
            SetupPlayerUI();
            EquipItem(0);
        }
/*        else //<-------------------------
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }*/
    }

    private void Update()
    {
        /*        if (!PV.IsMine) //<-------------------------
                    return;*/
        if (playingPuzzle)
            return;
        Look();
        Move();
        Jump();
        Fire();
        if(transform.position.y < minimumY)
            GameManager.RestorePlayer(gameObject);
        if (Input.GetKeyUp(KeyCode.LeftShift))
            TakeDamage(-5); //this is gain ;p

        //items handle from numbers
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
        //items handle from scrollwheel
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
                EquipItem(items.Length - 1);
            else
                EquipItem(itemIndex - 1);
        }
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    private void Fire()
    {
        ItemController itemController = items[itemIndex].GetComponent<Item>().itemGameObject.GetComponent<ItemController>();
        if (Input.GetButtonDown("Fire1"))
        {
            itemController.StartInteraction();
            if (PV.IsMine)
            {
                HashTable hash = new HashTable();
                hash.Add("firing", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            itemController.StopInteraction();
            if (PV.IsMine)
            {
                HashTable hash = new HashTable();
                hash.Add("firing", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
/*        if (!PV.IsMine)  //<-------------------------
            return;*/
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            HashTable hash = new HashTable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void TakeDamage(int _damage)
    {
        if (PV.IsMine)
        {
            currentHealth -= _damage;
            if (healthBar != null) //when hiting other players
                healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    //to share the weapon change among the other players
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            if(changedProps.ContainsKey("itemIndex"))
                EquipItem((int)changedProps["itemIndex"]);

            if (changedProps.ContainsKey("firing"))
            {

                ItemController itemController = items[itemIndex].GetComponent<Item>().itemGameObject.GetComponent<ItemController>();
                if (itemController != null)
                    if ((bool)changedProps["firing"])
                    {
                        itemController.StartInteraction();
                    }
                    else
                    {
                        itemController.StopInteraction();
                    }
            }
        }
    }

    public void Die()
    {
        Debug.Log("You are dead!!!!");
        SetMessage("YOU DIED;", Color.red, 50);
        transform.position = new Vector3(transform.position.x, transform.position.y + 200, transform.position.z);
        Invoke("ApplyDeath", 2f);
    }

    public void SetMessage(string _text, Color _color, int _size=40)
    {
        if (messageUI != null)
        {
            messageUI.text = _text;
            messageUI.fontSize = _size;
            messageUI.color = _color;
        }
    }
    void ApplyDeath()
    {
        // lose trash
        garbageManager.IncreaseGarbage(deathPenalty);
        // restore position
        GameManager.RestorePlayer(gameObject);
        // restore health
        currentHealth = maxHealth;
        if (healthBar != null) //when hiting other players
            healthBar.SetHealth(maxHealth);
        SetMessage("", Color.white);
    }

    //sets up the players UI
    void SetupPlayerUI()
    {
        //show health-bar
        Vector3 healtBarOffset = new Vector3(-55, -30, 0);
        GameObject healthBarObject = Instantiate(healthBarPrefab, healtBarOffset, Quaternion.identity) as GameObject;
        healthBarObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        InitHealthBar(healthBarObject.GetComponent<HealthBar>());

        //show crosshair
        GameObject crossHaiObject = Instantiate(crossHairPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        crossHaiObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        if(SceneManagerHelper.ActiveSceneBuildIndex == 1)
            garbageManager = GameObject.FindGameObjectWithTag("garbage manager").GetComponent<GarbageManager>();

        //show player message
        GameObject playerMessager = Instantiate(playerMessagePrefab, new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
        playerMessager.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        messageUI = playerMessager.GetComponent<TMP_Text>();
        Debug.Log(messageUI.text);
    }

    void InitHealthBar(HealthBar _healthbar)
    {
        healthBar = _healthbar;
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
}
