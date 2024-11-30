using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyController : MonoBehaviour
{
    public static DecoyController instance;
    public bool visible;
    public int code;

    [Header("Raccoon Decoy")]
    public bool isRaccoon;
    public bool explosion;
    public GameObject RaccoonSkin0, RacconSkin1;
    public GameObject teleport0, teleport1;


    void Start()
    {
        instance = this;

        if (isRaccoon)
        {
            if (TraitManager.instance.Raccoon[1] == 1)
            {
                if (!explosion)
                {
                    GetComponent<DestroyOnTime>().countDown += 1;
                }
            }

            if (SkinManager.instance.currentSkinCode != 0)
            {
                if(SkinManager.instance.currentSkinCode == 1)
                {
                    RaccoonSkin0.SetActive(false);
                    RacconSkin1.SetActive(true);

                    if (!explosion)
                    {
                        GetComponent<DestroyOnTime>().Effect = teleport1;
                    }
                }
            }
            /*else
            {
                RaccoonSkin0.SetActive(true);
                RacconSkin1.SetActive(false);

                if (!explosion)
                {
                    GetComponent<DestroyOnTime>().Effect = teleport0;
                }
            }*/
        }
        
    }

    // Update is called once per frame
    public void Destroy()
    {
        //GetComponent<DestroyOnTime>().Timer = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyController>() != null)
            {
                other.gameObject.GetComponent<EnemyController>().decoys = gameObject;
            }

        }
    }
    private void OnBecameVisible()
    {
        visible = true;
    }
    private void OnBecameInvisible()
    {
        visible = false;
    }
}
