using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRack : MonoBehaviour
{
    public static GunRack instance;
    public GameObject Message, Message2;
    public bool isRack;
    public Sprite oldSprite, newSprite;

    [Header("Cutscene")]
    public bool isCutscene;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

     
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCutscene)
        {
            if (PlayerPrefs.GetInt("newgun") == 1)
            {
                if (!isRack)
                {
                    Message2.gameObject.SetActive(true);
                }
            }
            else
            {
                Message2.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Message2.SetActive(false);
            Message.SetActive(true);


            isRack = true;

            UIController.instance.interactButton.SetActive(true);


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            Message.SetActive(false);

            isRack = false;

            UIController.instance.interactButton.SetActive(false);


        }
    }


}
