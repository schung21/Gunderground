using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCraft : MonoBehaviour
{

    public static GrenadeCraft instance;
    public GameObject[] bombs;
    public Transform spawnPoint;
    public int craftCost;
    public GameObject Message1, Message2;
 

    [HideInInspector]
    public bool inZone;
   
    private bool inactive;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        inactive = false;
    }

    public void craftConfirm()
    {
        if (!inactive)
        {
            if (LevelManager.instance.currentParts >= craftCost)
            {

                LevelManager.instance.currentParts -= craftCost;
                UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                CharTracker.instance.SavePlayer();

                int random = Random.Range(0, bombs.Length);
                Instantiate(bombs[random], spawnPoint.position, spawnPoint.rotation);
                Instantiate(PickupManager.instance.spawnEffect, spawnPoint.position, spawnPoint.rotation);

                inactive = true;

                Message1.gameObject.SetActive(false);
                UIController.instance.interactButton.SetActive(false);
            }
            else
            {
                Message2.gameObject.SetActive(true);
                Message1.gameObject.SetActive(false);
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inactive)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                UIController.instance.interactButton.SetActive(true);
                Message1.gameObject.SetActive(true);
                inZone = true;

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
            inZone = false;
        }
    }

}
