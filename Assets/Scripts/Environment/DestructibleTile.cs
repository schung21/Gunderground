using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Animations;

public class DestructibleTile : MonoBehaviour
{
    public GameObject dirtCrumble;
    public GameObject dropObject;

    public Tilemap tilemap;

    public static DestructibleTile instance;

    public bool shouldDropItem, shouldDropGun, isShopItem, isBarrier, isRareOre;
    public GameObject[] items, rareItems;
    public GameObject[] commonDrops;
    public GameObject[] uncommonDrops;
    public List<GameObject> rareDrops;
    public List<GameObject> legendDrops;
    public float Range1, Range2, Range3;
    private float dropChance;
    private ParticleSystem sparkle;

    public Animator anim;

    public GameObject sound;

    [HideInInspector]
    public bool isDestroyed;

    [Header("Detonate Block")]
    public bool isDetonator;

    [Header("Supply")]
    public bool isSupply;
/*
    [Header("No Overlap")]
    public bool voidBlock;*/

    // public int health = 1;

    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        //tilemap = GetComponent<Tilemap>();

        if (GetComponent<ParticleSystem>() != null)
        {
            sparkle = GetComponent<ParticleSystem>();
            sparkle.Play();
        }

        DestroyShopItem();

        if (isSupply)
        {
            anim.SetTrigger("Break");
        }
    }


    // Update is called once per frame


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(other.gameObject.CompareTag("Bullets"))
        {
            Vector3 hitPos = Vector3.zero;
            foreach(ContactPoint2D hit in other.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPos), null);
              *//*  int rotation = Random.Range(0, 4);
                Instantiate(dirtCrumble, transform.position, Quaternion.Euler(0f, 0f, rotation * 90));*//*

            }
           
            
        }
    }*/

    /* public void DestroyTileAtPos(ContactPoint2D hit)
     {
         int rotation = Random.Range(0, 4);
         Vector3 hitPos = Vector3.zero;

         hitPos.x = hit.point.x - 0.01f * hit.normal.x;
         hitPos.y = hit.point.y - 0.01f * hit.normal.y;
         tilemap.SetTile(tilemap.WorldToCell(hitPos), null);

         Instantiate(dirtCrumble, hitPos, Quaternion.Euler(0f, 0f, rotation * 90));

     }*/

    public void BulletCollide()
    {
        if (gameObject.tag != "Wall" && gameObject.tag != "Shop Item")
        {
            if (!isDetonator)
            {
                isDestroyed = true;
            }

            if (sparkle != null)
            {
                sparkle.Stop();
            }

            if (shouldDropGun || shouldDropItem)
            {
                anim.SetTrigger("Break");
            }
            else
            {
                Destroy(gameObject);
            }

            int rotation = Random.Range(0, 4);
            Instantiate(dirtCrumble, transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
            //Instantiate(dirtCrumble, transform.position, Quaternion.Euler(0f, 0f, rotation * 90));


        }

    }


    public void DestroyShopItem()
    {
        if (isShopItem)
        {
            Destroy(gameObject);

 
            if (shouldDropGun)
            {
                dropChance = Random.Range(0, 100f);

                if (dropChance >= 0 && dropChance <= Range3)
                {
                    if (legendDrops.Count == 0)
                    {
                        int randomItem = Random.Range(0, uncommonDrops.Length);

                        Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
                    }
                    else
                    {
                        int randomItem = Random.Range(0, legendDrops.Count);

                        Instantiate(legendDrops[randomItem], transform.position, transform.rotation);
                    }
                }


                if (dropChance > Range3 && dropChance <= Range2)
                {
                    if (rareDrops.Count == 0)
                    {
                        int randomItem = Random.Range(0, uncommonDrops.Length);

                        Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
                    }
                    else
                    {
                        int randomItem = Random.Range(0, rareDrops.Count);

                        Instantiate(rareDrops[randomItem], transform.position, transform.rotation);
                    }
                }


                if (dropChance > Range2 && dropChance <= Range1)
                {
                    int randomItem = Random.Range(0, uncommonDrops.Length);

                    Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
                }

                if (dropChance > Range1 && dropChance <= 100)
                {
                    int randomItem = Random.Range(0, commonDrops.Length);

                    Instantiate(commonDrops[randomItem], transform.position, transform.rotation);
                }


            }


            else if (shouldDropItem)
            {
                if (rareItems.Length == 0)
                {
                    int randomItem = Random.Range(0, items.Length);

                    Instantiate(items[randomItem], transform.position, transform.rotation);
                }
                else
                {
                    int random = Random.Range(0, 10);

                    if(random > Range1)
                    {
                        int randomItem = Random.Range(0, rareItems.Length);

                        Instantiate(rareItems[randomItem], transform.position, transform.rotation);
                    }
                    else if(random <= Range1)
                    {
                        int randomItem = Random.Range(0, items.Length);

                        Instantiate(items[randomItem], transform.position, transform.rotation);
                    }
                }
            }
        }
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (tag == "Dirt")
        {
            if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
            {
                BulletCollide();
               
            }
            if (other.gameObject.CompareTag("Player") && PlayerController.instance.isKnockedBack == true)
            {
                BulletCollide();


            }
            if (other.gameObject.CompareTag("Overlap Box"))
            {
                Destroy(gameObject);
            }
         
        }
        if (tag == "Wall")
        {
            if (other.gameObject.tag == "Boss")
            {
                
                BulletCollide();
            }
            if (other.gameObject.CompareTag("Overlap Box"))
            {
                Destroy(gameObject);
            }
         
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.isMole)
            {
                Invoke("BulletCollide", 0.001f);
            }
            else if (ArtifactManager.instance.hasDrill)
            {
                Invoke("BulletCollide", 0.001f);
            }

        }
        if (other.gameObject.CompareTag("Companion"))
        {

            BulletCollide();

        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == "Dirt")
        {
            if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
            {
                BulletCollide();

            }

        }
        
    }

    public void DropItem()
    {
        if (shouldDropGun)
        {
            dropChance = Random.Range(0, 100f);

            if (isRareOre)
            {
                Range1 += RuneController.instance.uniqueDrop * 100;
                Range2 += RuneController.instance.legendDrop * 100;
            }
            else
            {
                Range2 += RuneController.instance.uniqueDrop * 100;
                Range3 += RuneController.instance.legendDrop * 100;
            }

            Debug.Log(Range1 + "-" + Range2 + "-" + Range3);

            if (dropChance >= 0 && dropChance <= Range3)
            {
               
                if (legendDrops.Count == 0)
                {
                    int randomItem = Random.Range(0, uncommonDrops.Length);

                    Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
                }
                else
                {
                    int randomItem = Random.Range(0, legendDrops.Count);

                    Instantiate(legendDrops[randomItem], transform.position, transform.rotation);
                }
            }


            if (dropChance > Range3 && dropChance <= Range2)
            {
                if (rareDrops.Count == 0)
                {
                    int randomItem = Random.Range(0, uncommonDrops.Length);

                    Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
                }
                else
                {
                    int randomItem = Random.Range(0, rareDrops.Count);

                    Instantiate(rareDrops[randomItem], transform.position, transform.rotation);
                }
            }


            if (dropChance > Range2 && dropChance <= Range1)
            {
                int randomItem = Random.Range(0, uncommonDrops.Length);

                Instantiate(uncommonDrops[randomItem], transform.position, transform.rotation);
            }

            if (dropChance > Range1 && dropChance <= 100)
            {
                int randomItem = Random.Range(0, commonDrops.Length);

                Instantiate(commonDrops[randomItem], transform.position, transform.rotation);
            }


        }

        else if (shouldDropItem)
        {

            if (rareItems.Length == 0)
            {
                int randomItem = Random.Range(0, items.Length);

                Instantiate(items[randomItem], transform.position, transform.rotation);
            }
            else
            {
                float random = Random.Range(0, 100f);

                if (random < Range1)
                {
                    int randomItem = Random.Range(0, rareItems.Length);

                    Instantiate(rareItems[randomItem], transform.position, transform.rotation);
                }
                else if (random >= Range1)
                {
                    int randomItem = Random.Range(0, items.Length);

                    Instantiate(items[randomItem], transform.position, transform.rotation);
                }
            }

            if (sound != null)
            {
                Instantiate(sound, transform.position, transform.rotation);
            }
        }
    }
        
}
