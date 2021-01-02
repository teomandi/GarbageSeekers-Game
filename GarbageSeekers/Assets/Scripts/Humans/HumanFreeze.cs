using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class HumanFreeze : MonoBehaviour
{
    [SerializeField] bool isFreezable;
    [SerializeField] int freezedDuration;
    [SerializeField] Material skinMaterial;
    [SerializeField] Material icedMaterial;
    [SerializeField] SkinnedMeshRenderer bodySkinRenderer, headSkinRenderer;

    HumanController controller;

    private void Start()
    {
        /*controller = GetComponent<EnemyController>();*/
        isFreezable = true;
        controller = GetComponent<HumanController>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Freeze();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnFreeze();
        }
    }


    public void Freeze()
    {
        if (!isFreezable)
            return;
        Debug.Log("I am freezing!!");

        headSkinRenderer.material = icedMaterial;
        bodySkinRenderer.material = icedMaterial;

        controller.applyStop(true);
        //stop the agent
        Invoke("UnFreeze", freezedDuration);
    }

    private void UnFreeze()
    {
        controller.applyStop(false);
        bodySkinRenderer.material = skinMaterial;
        headSkinRenderer.material = skinMaterial;
    }
}
