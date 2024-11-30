using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    public List<GameObject> gunPickups;
    public static PickupManager instance;
    public bool pickItemUp, inGunZone, inShopZone;
    public Transform pickupSpawn1;
    public GameObject spawnEffect, convertMsg;
   

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
