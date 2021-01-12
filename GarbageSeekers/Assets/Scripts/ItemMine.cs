using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMine : MonoBehaviour
{
    //HP of the resource
    public int resourceHP=10;
    //value of garbages
    public int value = 5;

    
    //delay of the mini items
    public string MineObjDelay = "n";
    //miny objects produced during mining
    public Transform MineObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("mining");
        resourceHP -= 1;
        if (MineObjDelay == "n")
        {
            Instantiate(MineObj, transform.position, MineObj.rotation);
            MineObjDelay = "y";
            StartCoroutine(resetDelay());
        }
        
    }

    IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(.35f);
        MineObjDelay = "n";
    }

    void Update()
    {
        if (resourceHP < 1)
        {
            Destroy(gameObject);
            GarbageScore.theScore += value; 
        }
    }
}
