using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEffect : MonoBehaviour
{
    public static ChargingEffect instance;
    public GameObject theBody;
    public GameObject blastEffect;
    public bool isTimed, hasDelay, destroy, isEnemy;
    public float chargeTime;
    private float chargeSeconds;

    void Start()
    {
        instance = this;
        chargeSeconds = chargeTime;
    }

    // Update is called once per frame
    void Update()
    {

        if(isTimed && theBody.activeInHierarchy)
        {
            chargeSeconds -= Time.deltaTime;

            if(chargeSeconds <= 0)
            {
                if (hasDelay)
                {
                    Invoke("Delay", 0.2f);
                    chargeSeconds = chargeTime;
                }
                else
                {
                    CreateBlast();
                    chargeSeconds = chargeTime;
                }
            }
        }

    }

    public void CreateBlast()
    {
        theBody.SetActive(false);
        Instantiate(blastEffect, transform.position, transform.rotation);
        if (destroy)
        {
            Destroy(gameObject);

        }
    }

    public void BodySetActive()
    {
        theBody.SetActive(true);
    }

    public void Delay()
    {
        theBody.SetActive(false);
        Instantiate(blastEffect, transform.position, transform.rotation);
      
    }
}
