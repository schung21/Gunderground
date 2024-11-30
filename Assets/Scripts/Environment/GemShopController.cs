using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemShopController : MonoBehaviour
{
    public static GemShopController instance;
    public bool inGemShopZone;

    private void Start()
    {
        instance = this; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            UIController.instance.interactButton.SetActive(true);
            inGemShopZone = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            UIController.instance.interactButton.SetActive(false);
            inGemShopZone = false;

        }
    }
    public void openGemShop()
    {
        UIController.instance.GemShop();
    }
}
