using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawnManager : MonoBehaviour
{

    public GameObject normalOre, normalOre2;
    public GameObject superOre;
    public GameObject shopOre;
    public float superSpawnPercent;
    public bool isShopOre;

    // Start is called before the first frame update
    void Start()
    {
      
       SpawnOre();
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnOre()
    {
        float superChance = Random.Range(0, 100f);

        if (shopOre)
        {
            Instantiate(shopOre, transform.position, transform.rotation);
        }
        else
        {
            if (superChance <= superSpawnPercent)
            {
                Instantiate(superOre, transform.position, transform.rotation);
            }
            else
            {
                if (normalOre2 != null)
                {
                    int random = Random.Range(0, 11);

                    if (random <= 5)
                    {
                        Instantiate(normalOre, transform.position, transform.rotation);
                    }
                    else if (random >= 6)
                    {
                        Instantiate(normalOre2, transform.position, transform.rotation);
                    }
                }
                else
                {
                    Instantiate(normalOre, transform.position, transform.rotation);
                }

            }
        }

    }
}
