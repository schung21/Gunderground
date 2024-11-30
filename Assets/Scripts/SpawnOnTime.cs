using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTime : MonoBehaviour
{

    public float countDown;
    [HideInInspector]
    public float timer;

    public GameObject Effect, breakEffect;
  
    public bool randomSpawn, isBullet;
    // Start is called before the first frame update


    void Start()
    {
        timer = countDown;
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (Effect != null)
            {
                if (randomSpawn)
                {
                    float random = Random.Range(0, 180f);

                    Instantiate(Effect, transform.position, Quaternion.Euler(0f, 0f, random));
                }
                else
                {
                    if (isBullet)
                    {
                        GetComponent<PlayerBullet>().isExplodePlus = false;
                       
                    }
                    Instantiate(Effect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }

            }
         
        }

    }
}
