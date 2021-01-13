using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;


public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Item[] items;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView PV;

    HealthBar healthBar;
    GarbageCounter garbageCounter;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] GameObject crossHairPrefab;
    [SerializeField] GameObject garbageCounterPrefab;


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
            Cursor.visible = false;
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
        Look();
        Move();
        Jump();
        Fire();
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            TakeDamage(-10);
        }

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
                hash.Add("freezing_beam", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            itemController.StopInteraction();
            if (PV.IsMine)
            {
                HashTable hash = new HashTable();
                hash.Add("freezing_beam", false);
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
        currentHealth -= _damage;
        if(healthBar != null) //when hiting other players
            healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            Die();
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

            if (changedProps.ContainsKey("freezing_beam"))
            {

                ItemController itemController = items[itemIndex].GetComponent<Item>().itemGameObject.GetComponent<ItemController>();
                if (itemController != null)
                    if ((bool)changedProps["freezing_beam"])
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
/*        Debug.Log("You are dead!!!!");*/
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


        //show garbage counter
        Vector3 garbageCounterOffset = new Vector3(0, 0, 0);
        GameObject garbageCounterObject = Instantiate(garbageCounterPrefab, garbageCounterOffset, Quaternion.identity) as GameObject;
        garbageCounterObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        garbageCounter = garbageCounterObject.GetComponent<GarbageCounter>();
    }


    void InitHealthBar(HealthBar _healthbar)
    {
        healthBar = _healthbar;
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
}
