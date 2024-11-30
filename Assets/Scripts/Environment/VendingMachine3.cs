using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VendingMachine3 : MonoBehaviour
{
    public static VendingMachine3 instance;
    public List<GameObject> common, uncommon, unique, legend;
    public GameObject Message1, Message2;
    public bool canVend, cantVend, inVendZone, isPrem;
    public string lastVendTime;
    public Text coolDown;
    public int cost, minutes, seconds;
    DateTime currentTime;
    DateTime lastTimeClicked;
    TimeSpan span;
    //reseting the health every time button is clicked and saving the time of it
    private bool checkGunsList;
    private void Start()
    {
        if (CharTracker.instance.vendingTime3 != "" || CharTracker.instance.vendingTime3 != null)
        {
            lastVendTime = CharTracker.instance.vendingTime3;
        }

        instance = this;
        Invoke("AddGunsList", 2f);
        checkGunsList = false;
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

        /*for (int i = 0; i < ContentManager.instance.unlockedGuns.Count; i++)
        {
            if (ContentManager.instance.unlockedGuns[i] == 1)
            {
                foreach (GameObject a in PickupManager.instance.gunPickups)
                {
                    
                    if (a.GetComponent<GunPickup>().gunCode == i && a.GetComponent<GunPickup>().Rare)
                    {
                        unique.Add(a);
                    }
                    if (a.GetComponent<GunPickup>().gunCode == i && a.GetComponent<GunPickup>().Legend)
                    {
                        legend.Add(a);
                    }


                }
            }

            if(i == ContentManager.instance.unlockedGuns.Count - 1)
            {
                foreach (GameObject a in PickupManager.instance.gunPickups)
                {
                    if (a.GetComponent<GunPickup>().Uncommon)
                    {
                        uncommon.Add(a);
                    }
                    else if (!a.GetComponent<GunPickup>().Uncommon && !a.GetComponent<GunPickup>().Rare && !a.GetComponent<GunPickup>().Legend)
                    {
                        common.Add(a);
                    }
                }
            }
        }*/


    }
    public void Update()
    {
        if (checkGunsList && legend.Count == 0)
        {
            AddGunsList();
        }

        currentTime = DateTime.UtcNow;

        if (lastVendTime != "" && !canVend)
        {
            lastTimeClicked = DateTime.Parse(lastVendTime);
            span = (currentTime - lastTimeClicked);
            coolDown.text = string.Format("{0:00}:{1:00}", (0 - span.Minutes), (10 - span.Seconds));
            coolDown.gameObject.SetActive(true);
        }

        if (span.TotalMinutes >= 0.17 || currentTime.Date > lastTimeClicked.Date || lastVendTime == "")
        {
            coolDown.gameObject.SetActive(false);
            canVend = true;

            lastVendTime = "";

            if (CharTracker.instance.vendingTime3 != "")
            {
                CharTracker.instance.vendingTime3 = "";
                CharTracker.instance.SavePlayer();
            }
        }

    }
    public void buttonClicked()
    {
        if (LevelManager.instance.currentGems >= cost)
        {
            LevelManager.instance.currentGems -= cost;
            UIController.instance.gems.text = "x" + LevelManager.instance.currentGems.ToString();

            if (isPrem)
            {
                int randomNum = Random.Range(0, 100);

                if (randomNum >= 30)
                {
                    int randomGun = Random.Range(0, unique.Count);

                    Instantiate(unique[randomGun], new Vector3(PlayerController.instance.transform.position.x + 1.5f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x + 1.5f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                }
                if (29 >= randomNum && randomNum >= 0)
                {
                    int randomGun = Random.Range(0, legend.Count);

                    Instantiate(legend[randomGun], new Vector3(PlayerController.instance.transform.position.x + 1.5f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x + 1.5f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));

                }

            }
            else
            {
                int randomNum = Random.Range(0, 100);

                if (randomNum >= 60)
                {
                    int randomGun = Random.Range(0, common.Count);

                    Instantiate(common[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                }
                if (59 >= randomNum && randomNum >= 30)
                {
                    int randomGun = Random.Range(0, uncommon.Count);

                    Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                    PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));

                }
                if (29 >= randomNum && randomNum >= 10)
                {
                    if (unique.Count != 0)
                    {
                        int randomGun = Random.Range(0, unique.Count);

                        Instantiate(unique[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        int randomGun = Random.Range(0, uncommon.Count);

                        Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    }
                }
                if (9 >= randomNum && randomNum >= 0)
                {
                    if (legend.Count != 0)
                    {
                        int randomGun = Random.Range(0, legend.Count);

                        Instantiate(legend[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        int randomGun = Random.Range(0, uncommon.Count);

                        Instantiate(uncommon[randomGun], new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(PickupManager.instance.spawnEffect, new Vector3(PlayerController.instance.transform.position.x - 1.3f, PlayerController.instance.transform.position.y,
                        PlayerController.instance.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }

            lastVendTime = DateTime.UtcNow.ToString();

            Message1.gameObject.SetActive(false);
            coolDown.gameObject.SetActive(true);

            canVend = false;

            UIController.instance.interactButton.SetActive(false);
            CharTracker.instance.SavePlayer();
            GoogleSaveManager.instance.OpenSave(true);
        }
        else
        {
            Message2.gameObject.SetActive(true);
            Message1.gameObject.SetActive(false);
        }

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
            else
            {
                coolDown.gameObject.SetActive(true);

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
}
