using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;

    //[HideInInspector]
    public bool canSelect, isNear;

    public GameObject message4, switchText;
    public List<GameObject> messages;

    public PlayerController playerToSpawn;
    public CompanionController companionToSpawn;

    public bool isAvailable, isCompanion, change, change2, orderedText;
    // Start is called before the first frame update
    public int textOrder;


    void Start()
    {
        instance = this;

        textOrder = messages.Count;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (isAvailable)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= 2f && CharacterSelectManager.instance.notNear == false)
            {
                UIController.instance.interactButton.gameObject.SetActive(true);
                message4.SetActive(true);
                switchText.SetActive(true);

                CharacterSelectManager.instance.canChange = true;
                CharacterSelectManager.instance.activeCharSelect.gameObject.GetComponent<CharacterSelector>().message4.SetActive(false);
                CharacterSelectManager.instance.activeCharSelect.gameObject.GetComponent<CharacterSelector>().switchText.SetActive(false);

                if (CharacterSelectManager.instance.change == true)//Input.GetKeyDown(KeyCode.E))
                {
                    CharacterSelectManager.instance.canChange = false;
                    CharacterSelectManager.instance.notNear = true;
                    switchText.SetActive(false);
                    message4.SetActive(false);
                    Vector3 playerPos = PlayerController.instance.transform.position;


                    Destroy(PlayerController.instance.gameObject);

                    PlayerController newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);
                    PlayerController.instance = newPlayer;

                    //add gun
                    if (CharacterSelectManager.instance.gun1 != null)
                    {
                        TransferGuns(CharacterSelectManager.instance.gun1);
                        PlayerController.instance.availableGuns[1].gameObject.SetActive(false);
                       
                    }
                    if (CharacterSelectManager.instance.gun2 != null)
                    {
                        TransferGuns(CharacterSelectManager.instance.gun2);
                        PlayerController.instance.availableGuns[2].gameObject.SetActive(false);

                    }
                    if (CharacterSelectManager.instance.gun3 != null)
                    {
                        TransferGuns(CharacterSelectManager.instance.gun3);
                        PlayerController.instance.availableGuns[3].gameObject.SetActive(false);

                    }

                    gameObject.SetActive(false);

                    CharacterSelectManager.instance.activePlayer = newPlayer;
                    CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
                    CharacterSelectManager.instance.activeCharSelect = this;
                    

                    CharacterSelectManager.instance.change = false;
                    canSelect = false;

                    if(Locker.instance != null)
                    {
                        Locker.instance.Message2.SetActive(false);
                        Locker.instance.CheckSkills();
                    }
       
                }

            }
            else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 2f &&
                Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 6f)
            {
                message4.SetActive(false);
                switchText.SetActive(false);
                UIController.instance.interactButton.gameObject.SetActive(false);
                CharacterSelectManager.instance.canChange = false;

            }
        }

        if (isCompanion)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= 2f)
            {
                UIController.instance.interactButton.gameObject.SetActive(true);
                message4.SetActive(true);
                switchText.SetActive(true);

                if (change2 == true)//Input.GetKeyDown(KeyCode.E))
                {
                    switchText.SetActive(false);
                    message4.SetActive(false);

                    Instantiate(companionToSpawn, transform.position, transform.rotation);

                    gameObject.SetActive(false);

                    change2 = false;
                    canSelect = false;

                    UIController.instance.interactButton.gameObject.SetActive(false);
                }

            }
            else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 2f &&
                Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 4f)
            {
                message4.SetActive(false);
                switchText.SetActive(false);
                UIController.instance.interactButton.gameObject.SetActive(false);

            }
        }
/*
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 2f)
        {
            isNear = true;
            canSelect = true;
            UIController.instance.interactButton.gameObject.SetActive(true);
       
        }
        else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= 2f &&
            Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 5f)
        {
            message.SetActive(false);
            canSelect = false;
            UIController.instance.interactButton.gameObject.SetActive(false);

        }*/

 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAvailable && !isCompanion)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!orderedText)
                {
                    int random = Random.Range(0, 11);

                    if (messages.Count > 3)
                    {

                        if (random >= 0 && random < 3)
                        {
                            messages[0].gameObject.SetActive(true);
                        }
                        else if (random >= 3 && random < 6)
                        {
                            messages[1].gameObject.SetActive(true);
                        }
                        else if (random >= 6 && random < 9)
                        {
                            messages[2].gameObject.SetActive(true);
                        }
                        else
                        {
                            messages[3].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (random >= 0 && random < 4)
                        {
                            messages[0].gameObject.SetActive(true);
                        }
                        else if (random >= 4 && random < 8)
                        {
                            messages[1].gameObject.SetActive(true);
                        }
                        else
                        {
                            messages[2].gameObject.SetActive(true);
                        }
                    }

                }
                else if (orderedText)
                {
                    messages[textOrder - 1].gameObject.SetActive(true);
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       
        
        if (!isAvailable && !isCompanion)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                for(int i = 0; i < messages.Count; i++)
                {
                    messages[i].SetActive(false);

                }

                if (orderedText)
                {
                    textOrder -= 1;

                    if (textOrder == 0)
                    {
                        textOrder = messages.Count;
                    }
                }

            }
        }
    }

    public void SelectFromMenu(PlayerController selectedPlayer, CharacterSelector CharSelector)
    {
        Vector3 playerPos = PlayerController.instance.transform.position;

        Destroy(PlayerController.instance.gameObject);

        PlayerController newPlayer = Instantiate(selectedPlayer, playerPos, Quaternion.Euler(0f, 0f, 0f));
        PlayerController.instance = newPlayer;

        CharSelector.gameObject.SetActive(false);

        CharacterSelectManager.instance.activePlayer = newPlayer;
        CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
        CharacterSelectManager.instance.activeCharSelect = CharSelector;

        CharacterSelectManager.instance.change = false;
  
    }

    public void TransferGuns(Gun gun)
    {
        Gun gunClone = Instantiate(gun);
        gunClone.transform.parent = PlayerController.instance.gunArm;
        gunClone.transform.position = PlayerController.instance.gunArm.position;
        gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
        gunClone.transform.localScale = Vector3.one;

        PlayerController.instance.availableGuns.Add(gunClone);
    }

}
