using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public static Merchant instance;
    public GameObject text1, text2, text3, text4, text5, text6;
    public GameObject merchant1, merchant2;
    [HideInInspector]
    public GameObject[] shopItems;
    [HideInInspector]
    public int buyCount;
 
    public bool randomText, secretMerc, gemMerc;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        shopItems = GameObject.FindGameObjectsWithTag("Shop Item");

        if (!secretMerc)
        {
            if (PlayerController.instance.isRaccoon)
            {
                merchant1.SetActive(false);
                merchant2.SetActive(true);
            }
            else
            {
                merchant2.SetActive(false);
                merchant1.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (shopItems != null)
        {

            foreach (GameObject items in shopItems)
            {
                if (items.GetComponent<ShopItem>().isInZone == true)
                {
                    if (!gemMerc)
                    {
                        if (PlayerController.instance.buyItem == true && items.GetComponent<ShopItem>().Cost <= LevelManager.instance.currentCoins)
                        {
                            LevelManager.instance.currentCoins -= items.GetComponent<ShopItem>().Cost;
                            LevelManager.instance.GetBuys();
                            PlayerController.instance.buyItem = false;

                            if (PlayerController.instance.isRaccoon && !secretMerc)
                            {
                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text4.SetActive(false);
                                text5.SetActive(true);
                                text6.SetActive(false);
                            }
                            else
                            {

                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text1.SetActive(false);
                                text2.SetActive(true);
                                text3.SetActive(false);
                            }

                            UIController.instance.coins.text = "x" + LevelManager.instance.currentCoins.ToString();
                        }
                        else if (PlayerController.instance.buyItem == true && items.GetComponent<ShopItem>().Cost > LevelManager.instance.currentCoins)
                        {
                            if (PlayerController.instance.isRaccoon && !secretMerc)
                            {
                                text4.SetActive(false);
                                text5.SetActive(false);
                                text6.SetActive(true);
                            }
                            else
                            {
                                text1.SetActive(false);
                                text2.SetActive(false);
                                text3.SetActive(true);
                            }
                            PlayerController.instance.buyItem = false;
                        }
                    }
                    else if (gemMerc)
                    {
                        if (PlayerController.instance.confirmAdBuy == true)
                        {
                            
                            PlayerController.instance.confirmAdBuy = false;
                            buyCount += 1;

                            if (PlayerController.instance.isRaccoon)
                            {
                                if (items.GetComponent<ShopItemTimer>() != null)
                                {
                                    items.GetComponent<ShopItemTimer>().lastBuyTime = DateTime.UtcNow.ToString();
                                    items.GetComponent<ShopItemTimer>().isBought = true;
                                }
                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text3.SetActive(false);
                                text5.SetActive(false);
                                text6.SetActive(true);
                            }
                            else
                            {
                                if (items.GetComponent<ShopItemTimer>() != null)
                                {
                                    items.GetComponent<ShopItemTimer>().lastBuyTime = DateTime.UtcNow.ToString();
                                    items.GetComponent<ShopItemTimer>().isBought = true;
                                }
                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text1.SetActive(false);
                                text2.SetActive(true);
                                text3.SetActive(false);
                            }

                     
                        }

                        if (PlayerController.instance.confirmBuy == true && items.GetComponent<ShopItem>().Cost <= LevelManager.instance.currentGems)
                        {
                            LevelManager.instance.currentGems -= items.GetComponent<ShopItem>().Cost;
                            PlayerController.instance.confirmBuy = false;
                            buyCount += 1;

                         
                            if (PlayerController.instance.isRaccoon)
                            {
                                if (items.GetComponent<ShopItemTimer>() != null)
                                {
                                    items.GetComponent<ShopItemTimer>().lastBuyTime = DateTime.UtcNow.ToString();
                                    items.GetComponent<ShopItemTimer>().isBought = true;
                                }
                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text3.SetActive(false);
                                text5.SetActive(false);
                                text6.SetActive(true);
                            }
                            else
                            {
                                if(items.GetComponent<ShopItemTimer>() != null)
                                {
                                    items.GetComponent<ShopItemTimer>().lastBuyTime = DateTime.UtcNow.ToString();
                                    items.GetComponent<ShopItemTimer>().isBought = true;
                                }
                                items.GetComponent<ShopItem>().gameObject.SetActive(false);
                                text1.SetActive(false);
                                text2.SetActive(true);
                                text3.SetActive(false);
                            }

                            UIController.instance.gems.text = "x" + LevelManager.instance.currentGems.ToString();
                            CharTracker.instance.SavePlayer();
                            UIController.instance.closeWindow();
                        
                        }
                        else if (PlayerController.instance.confirmBuy == true && items.GetComponent<ShopItem>().Cost > LevelManager.instance.currentGems)
                        {
                            if (PlayerController.instance.isRaccoon)
                            {
                                text3.SetActive(true);
                                text5.SetActive(false);
                                text6.SetActive(false);
                            }
                            else
                            {
                                text1.SetActive(false);
                                text2.SetActive(false);
                                text3.SetActive(true);
                            }
                            PlayerController.instance.confirmBuy = false;
                            UIController.instance.notEnoughWindow.SetActive(true);
                        }
                    }

                }

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].isRifle)
            {
                if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponent<BoxCollider2D>() != null)
                {

                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            if (PlayerController.instance.isRaccoon && !secretMerc)
            {
                text4.gameObject.SetActive(true);
            }
            else
            {
                if (gemMerc && buyCount == 2)
                {
                    text4.gameObject.SetActive(true);
                    text3.gameObject.SetActive(false);
                    text6.gameObject.SetActive(false);
                    text2.gameObject.SetActive(false);
                }
                else
                {
                    if (PlayerController.instance.isRaccoon)
                    {
                        text5.gameObject.SetActive(true);
                        text6.SetActive(false);
                    }
                    else
                    {
                        text1.gameObject.SetActive(true);
                        text2.gameObject.SetActive(false);
                    }
                    
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {   
         
            if (PlayerController.instance.isRaccoon && !secretMerc)
            {
                text4.gameObject.SetActive(false);
                text5.gameObject.SetActive(false);
                text6.gameObject.SetActive(false);
            }
            else
            {
                text1.gameObject.SetActive(false);
                text2.gameObject.SetActive(false);
                text3.gameObject.SetActive(false);

                if (gemMerc)
                {
                    text4.gameObject.SetActive(false);
                    text5.gameObject.SetActive(false);
                    text6.gameObject.SetActive(false);
                }
            }

            if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].isRifle)
            {
                if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponent<BoxCollider2D>() != null)
                {
                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponent<BoxCollider2D>().enabled = true;
                }
            }

        }
    }

}
