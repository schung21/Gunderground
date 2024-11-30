using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VendingMachine2 : MonoBehaviour
{
    public static VendingMachine2 instance;
    public List<GameObject> common, uncommon, unique, legend;
    public GameObject Message1, Message2;
    public bool canVend, cantVend, inVendZone;
    public string lastVendTime;
    public Text coolDown;
    public int cost, minutes, seconds, adCount;
    DateTime currentTime;
    DateTime lastTimeClicked;
    TimeSpan span;

/*    [Header("GemGift")]
    public GameObject dailyGem;
    public Transform gemPoint;*/

    private bool checkGunsList, gemAvailable;
    //reseting the health every time button is clicked and saving the time of it
    private void Start()
    {
      
        instance = this;
        Invoke("AddGunsList", 2f);
        checkGunsList = false;

        if (CharTracker.instance.vendingTime2 != "" || CharTracker.instance.vendingTime2 != null)
        {
            lastVendTime = CharTracker.instance.vendingTime2;
        }

        if (!PlayerPrefs.HasKey("adCountGun"))
        {
            PlayerPrefs.SetInt("adCountGun", 0);
            Load();
        }
        else
        {
            Load();
        }

    }


    public void AddGunsList()
    {
        for (int i = 0; i < ContentManager.instance.unlockedGuns.Count; i++)
        {
            if (i == ContentManager.instance.unlockedGuns.Count - 1)
            {
                foreach (GameObject a in PickupManager.instance.gunPickups)
                {
                    if (!a.GetComponent<GunPickup>().Uncommon && !a.GetComponent<GunPickup>().Rare && !a.GetComponent<GunPickup>().Legend)
                    {
                        common.Add(a);
                    }
                    else if (a.GetComponent<GunPickup>().Uncommon)
                    {
                        uncommon.Add(a);
                    }
                    else if (a.GetComponent<GunPickup>().Rare)
                    {
                        unique.Add(a);
                    }
                    else if (a.GetComponent<GunPickup>().Legend)
                    {
                        legend.Add(a);
                    }

                }
            }
        }

        checkGunsList = true;
    }
    public void Update()
    {
        if(checkGunsList && legend.Count == 0)
        {
            AddGunsList();
        }

        currentTime = DateTime.UtcNow;


        if (lastVendTime != "")
        {
            lastTimeClicked = DateTime.Parse(lastVendTime);
            span = (currentTime - lastTimeClicked);
            coolDown.text = string.Format("{0:00}:{1:00}", (4 - span.Minutes), (59 - span.Seconds));
            if (adCount == 1)
            {
                coolDown.gameObject.SetActive(true);
                canVend = false;
            }
        }

        if (span.TotalMinutes >= 5 && lastVendTime != "")
        {
            lastVendTime = "";
            adCount = 0;
            coolDown.gameObject.SetActive(false);
            canVend = true;

            Save();
            CharTracker.instance.SavePlayer();
        }


    }
    public void buttonClicked()
    {
        if (adCount < 1)
        {
            UIController.instance.confirmAdItem();
        /*    Message1.gameObject.SetActive(false);
            Message2.gameObject.SetActive(false);
            inVendZone = false;*/
        }
        else
        {
            Message2.gameObject.SetActive(false);
            Message1.gameObject.SetActive(false);
        }

        /* if (LevelManager.instance.currentGems >= cost)
         {
             //PlayAd()

             *//*  lastVendTime = DateTime.UtcNow.ToString();

               Message1.gameObject.SetActive(false);
               coolDown.gameObject.SetActive(true);

               canVend = false;

               CharTracker.instance.SavePlayer();*//*

             Message2.gameObject.SetActive(true);
             Message1.gameObject.SetActive(false);
             UIController.instance.interactButton.SetActive(false);
         }
         else
         {
             Message2.gameObject.SetActive(true);
         }*/

    }

    public void PlayAd()
    {
        int randomNum = Random.Range(0, 100);

        if (randomNum >= 60)
        {
            int randomGun = Random.Range(0, common.Count);

            Instantiate(common[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
            PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
            PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
        }
        if (59 >= randomNum && randomNum >= 30)
        {
            int randomGun = Random.Range(0, uncommon.Count);

            Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
            PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
            PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));

        }
        if (29 >= randomNum && randomNum >= 10)
        {
            if (unique.Count != 0)
            {
                int randomGun = Random.Range(0, unique.Count);

                Instantiate(unique[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                int randomGun = Random.Range(0, uncommon.Count);

                Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            }
        }
        if (9 >= randomNum && randomNum >= 0)
        {
            if (legend.Count != 0)
            {
                int randomGun = Random.Range(0, legend.Count);

                Instantiate(legend[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                int randomGun = Random.Range(0, uncommon.Count);

                Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 2f, PlayerController.instance.transform.position.y,
                PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            }
        }


        AddCount();

        if (lastVendTime == "" || lastVendTime == null)
        {
            lastVendTime = DateTime.UtcNow.ToString();
        }

        CharTracker.instance.SavePlayer();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canVend)
            {
                UIController.instance.interactButton.SetActive(true);
                Message1.gameObject.SetActive(true);
                inVendZone = true;

            }
       
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIController.instance.interactButton.SetActive(false);
            Message1.gameObject.SetActive(false);
            Message2.gameObject.SetActive(false);
            inVendZone = false;
        }
    }

    public void AddCount()
    {
        adCount++;
        Save();

        if(adCount == 1)
        {
            UIController.instance.interactButton.SetActive(false);
            Message1.gameObject.SetActive(false);
            Message2.gameObject.SetActive(false);
            inVendZone = false;
        }
    }

    private void Load()
    {

        adCount = PlayerPrefs.GetInt("adCount");

        if (adCount > 1)
        {
            adCount = 0;
            Save();
        }

    }

    private void Save()
    {
        PlayerPrefs.SetInt("adCount", adCount);
    }

}
