using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{

    public GameObject shopLayout;
    public GameObject blackScreen;
    public GameObject activBox, deactivBox;
    public GameObject Door;
    public Sprite Open, Close;
    public bool activate, deactivate, needKey, isBlackScreen;

    [Header("Audio")]
    public GameObject openSound, closeSound;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (activate)
            {
                if (needKey == false)
                {
                    if (isBlackScreen)
                    {
                        blackScreen.SetActive(false);
                    }
                    deactivBox.SetActive(true);
                    activBox.SetActive(false);
                    Door.GetComponent<SpriteRenderer>().sprite = Open;
                    Door.GetComponent<BoxCollider2D>().enabled = false;
                    /*if (shopLayout != null)
                    {
                        shopLayout.SetActive(true);
                    }*/

                    Instantiate(openSound, transform.position, transform.rotation);
                }
                else
                {
                    if(LevelManager.instance.currentKeys > 3)
                    {
                        if (blackScreen != null)
                        {
                            blackScreen.SetActive(false);
                        }
                        activBox.SetActive(false);
                        Door.GetComponent<SpriteRenderer>().sprite = Open;
                        Door.GetComponent<BoxCollider2D>().enabled = false;
                       /* if (shopLayout != null)
                        {
                            shopLayout.SetActive(true);
                        }*/
                       
                    }
                }

            }
            else if (deactivate)
            {
                if (!Door.GetComponent<BoxCollider2D>().enabled)
                {
                    Instantiate(closeSound, transform.position, transform.rotation);
                }
                if (isBlackScreen)
                {
                    blackScreen.SetActive(true);
                }
                deactivBox.SetActive(false);
                activBox.SetActive(true);
                Door.GetComponent<SpriteRenderer>().sprite = Close;
                Door.GetComponent<BoxCollider2D>().enabled = true;
                /*  if (shopLayout != null)
                  {
                      shopLayout.SetActive(false);
                  }*/
                
            }

        }
    }

 
}
