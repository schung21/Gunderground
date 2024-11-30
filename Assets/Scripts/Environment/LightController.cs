using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public static LightController instance;

    public float availableDistance;
    private float Distance;
    private Light Lightcomponent;
    private GameObject Player;
 


    void Start()
    {
        instance = this;
        Lightcomponent = gameObject.GetComponent<Light>();
       
    }

    // Update is called once per frame
    void Update()
    {
      
        if (PlayerController.instance != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");

            if (Player != null)
            {
                Distance = Vector3.Distance(Player.transform.position, transform.position);
            }
        }


        if (Distance < availableDistance)
        {
            Lightcomponent.enabled = true;
        }
        if (Distance > availableDistance)
        {
            Lightcomponent.enabled = false;
        }
    }

}

