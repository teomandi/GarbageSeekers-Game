using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMine : MonoBehaviour
{
    //HP of the resource
    public int resourceHP=10;
    //value of garbages
    public int value=5;
    //delay of the mini items
    public bool MineObjDelay = true;
    //miny objects produced during mining
    public Transform MineObj;


    void OnParticleCollision(GameObject other)
    {
        resourceHP -= 1;
        if (MineObjDelay)
        {
            GameObject miniMe = Instantiate(MineObj.gameObject, transform.position, MineObj.rotation) as GameObject;
            miniMe.transform.localScale = miniMe.transform.localScale / 2;
            MineObjDelay = false;
            StartCoroutine(resetDelay());
        }
    }

    IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(.05f);
        MineObjDelay = true;
    }


    void Update()
    {
        if (resourceHP < 1)
        {
            Destroy(gameObject);
            GarbageManager garbageManager = GameObject.FindGameObjectWithTag("garbage manager").GetComponent<GarbageManager>();
            if(garbageManager !=null)
                garbageManager.IncreaseGarbage(value); 
        }
    }
}
