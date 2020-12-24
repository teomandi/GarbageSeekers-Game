using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    #region
    public static PlayerTracker instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
}
