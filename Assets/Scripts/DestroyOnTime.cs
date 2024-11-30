using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float countDown, breakRange, breakRange2;
    [HideInInspector]
    public float Timer;
    public GameObject Effect, breakEffect;
    [HideInInspector]
    public GameObject[] Enemy, bossParts;

    public bool Deactivate, breakTouch, randomSpawn, matchScale, delWithPlayer;

    [Header("Boss Spawn")]
    public bool isSummon;

    // Start is called before the first frame update

 
    void Start()
    {
        Timer = countDown;

  
        if (breakTouch)
        {

            //Boss = GameObject.FindGameObjectsWithTag("Boss");
            bossParts = GameObject.FindGameObjectsWithTag("Boss Part");

          
           Physics2D.IgnoreCollision(BossController.instance.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());

            foreach (var a in bossParts)
            {
                Physics2D.IgnoreCollision(a.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        Timer -= Time.deltaTime;

        if(Timer <= 0)
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
                    if (matchScale)
                    {
                        var effect = Instantiate(Effect, transform.position, transform.rotation);
                        effect.transform.localScale = transform.localScale;
                    }
                    else
                    {
                        Instantiate(Effect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }

                if (Deactivate)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            else if (Deactivate)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (breakTouch)
        {

            if (Vector3.Distance(BossController.instance.transform.position, transform.position) < breakRange)
            {
                Destroy(gameObject);
                Instantiate(breakEffect, transform.position, transform.rotation);
            }

            foreach (var a in bossParts)
            {
                if (Vector3.Distance(a.transform.position, transform.position) < breakRange2)
                {
                    Destroy(gameObject);
                    Instantiate(breakEffect, transform.position, transform.rotation);
                }
            }

        }

        if (delWithPlayer)
        {
            if(PlayerHealth.instance.currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (isSummon)
        {
            if(BossController.instance.currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CreateEffect()
    {
        Instantiate(Effect, transform.position, transform.rotation);
    }


}
