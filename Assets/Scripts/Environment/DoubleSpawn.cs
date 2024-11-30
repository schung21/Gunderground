using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSpawn : MonoBehaviour
{
    public static DoubleSpawn instance;
    public GameObject[] effects;
    private bool canFire;

    private void Awake()
    {
        instance = this;

        //PlayerController.instance.effects = GameObject.FindGameObjectsWithTag("Player Gun Effect");

        Invoke("CanShoot", 0.1f);

    }


    public void CanShoot()
    {
        PlayerController.instance.canSpawnEffect = true;
    }




}
