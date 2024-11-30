using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Burst.Intrinsics;

public class GunPickup : MonoBehaviour
{
    public static GunPickup instance;
    public Gun theGun;
    public PickupManager Pickups;
    public float waitToBeCollected = .1f;
    public GameObject Message, UpdatedOre, UpdatedOre2, UpdatedOre3, UpdateManager;
    [HideInInspector]
    public bool inPickupZone;
    //public string weaponName;
    public bool pickItemUp, isLocked;
    public bool Uncommon, Rare, Legend;
    public int gunCode, gunLvl;

    [Header("Saving Ammo Count")]
    public bool isDropped;
    public int ammo;

    private bool hasSameGun = false;

    [Header("Convert Parts")]
    public bool isConvert;


    // Start is called before the first frame update
    void Start()
    {
        Message.SetActive(false);

        instance = this;

        if (LevelExit.instance != null)
        {
            if(Vector3.Distance(transform.position, LevelExit.instance.transform.position) < 2)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            }
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (ContentManager.instance.unlockedGuns[gunCode] == 1)
        {
            isLocked = false;
        }

        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }

        CheckSameGun();

        if (!hasSameGun && inPickupZone)
        {

            if (PlayerController.instance.availableGuns.Count < PlayerController.instance.gunsHeld)
            {
                if (PickupManager.instance.pickItemUp == true)//Input.GetKeyDown(KeyCode.E))
                {
                    if(isLocked == true)
                    {
                        UIController.instance.SaveNewGun(1);
                        ContentManager.instance.saveGun(gunCode);
                    
                    }
                    
                    Gun.instance.counter = 0;
                    Gun gunClone = Instantiate(theGun);
                    gunClone.transform.parent = PlayerController.instance.gunArm;
                    gunClone.transform.position = PlayerController.instance.gunArm.position;
                    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    gunClone.transform.localScale = Vector3.one;
                    //gunClone.timeBetweenShots -= PlayerController.instance.fireRate;

                    PlayerController.instance.availableGuns.Add(gunClone);
                    PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                    PlayerController.instance.nextGun = PlayerController.instance.currentGun + 1;
                    if(PlayerController.instance.nextGun == PlayerController.instance.availableGuns.Count)
                    {
                        PlayerController.instance.nextGun = 1;
                    }
                    PlayerController.instance.SwitchGun();
                    if (isDropped)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = ammo;
                    }
                    else
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 500;
                    }

                    PickupManager.instance.pickItemUp = false;


                    Destroy(gameObject);

                }
            }
            else if (PlayerController.instance.availableGuns.Count == PlayerController.instance.gunsHeld)
            {

                if (/*Input.GetKeyDown(KeyCode.E)*/PickupManager.instance.pickItemUp && PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName != "Pistol")
                {
                    if (isLocked == true)
                    {
                        UIController.instance.SaveNewGun(1);
                        ContentManager.instance.saveGun(gunCode);
                    }

                    Gun.instance.counter = 0;
                    for (int i = 0; i < PickupManager.instance.gunPickups.Count; i++)
                    {
                        if (PickupManager.instance.gunPickups[i].name == PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName)
                        {
                            var a = Instantiate(PickupManager.instance.gunPickups[i], PlayerController.instance.transform.position, PlayerController.instance.transform.rotation);
                            a.GetComponent<GunPickup>().isDropped = true;
                            a.GetComponent<GunPickup>().ammo = PlayerController.instance.Ammo[PlayerController.instance.currentGun];
                            Destroy(PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gameObject);
                        }
                    }

                    Gun gunClone = Instantiate(theGun);
                    gunClone.transform.parent = PlayerController.instance.gunArm;
                    gunClone.transform.position = PlayerController.instance.gunArm.position;
                    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    gunClone.transform.localScale = Vector3.one;
                    //gunClone.timeBetweenShots -= PlayerController.instance.fireRate;

                    PlayerController.instance.availableGuns.Remove(PlayerController.instance.availableGuns[PlayerController.instance.currentGun]);
                    PlayerController.instance.availableGuns.Insert(PlayerController.instance.currentGun, gunClone);

                    if (isDropped)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = ammo;
                    }
                    else
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 500;
                    }

                    //PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                    PlayerController.instance.SwitchGun();

                    PickupManager.instance.pickItemUp = false;


                    Destroy(gameObject);

                    
                }
            }
          
        }
        else if (hasSameGun && inPickupZone)
        {
            PickupManager.instance.pickItemUp = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            if (!isConvert)
            {
                Message.SetActive(true);

                inPickupZone = true;

                PickupManager.instance.inGunZone = true;

                UIController.instance.interactButton.SetActive(true);
            }
            else if (isConvert)
            {
                if (Legend)
                {
                    var text = Instantiate(PickupManager.instance.convertMsg, transform.position, transform.rotation);
                    text.GetComponentInChildren<Text>().text = "+50";

                    LevelManager.instance.GetParts(50);
                    UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                    Destroy(gameObject);

                }
                else if (Rare)
                {
                    var text = Instantiate(PickupManager.instance.convertMsg, transform.position, transform.rotation);
                    text.GetComponentInChildren<Text>().text = "+30";

                    LevelManager.instance.GetParts(30);
                    UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                    Destroy(gameObject);
                }
                else if (Uncommon)
                {
                    var text = Instantiate(PickupManager.instance.convertMsg, transform.position, transform.rotation);
                    text.GetComponentInChildren<Text>().text = "+10";

                    LevelManager.instance.GetParts(10);
                    UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                    Destroy(gameObject);
                }
                else
                {
                    var text = Instantiate(PickupManager.instance.convertMsg, transform.position, transform.rotation);
                    text.GetComponentInChildren<Text>().text = "+2";

                    LevelManager.instance.GetParts(2);
                    UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                    Destroy(gameObject);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Message.SetActive(false);

            inPickupZone = false;

            PickupManager.instance.inGunZone = false;

            UIController.instance.interactButton.SetActive(false);
        }

    }


    public void IncreaseFR()
    {
        theGun.timeBetweenShots -= PlayerController.instance.fireRate;
       
    }

    public void CheckSameGun()
    {
        List<string> gunList = new List<string>();

        foreach(Gun checkGun in PlayerController.instance.availableGuns)
        {
            gunList.Add(checkGun.weaponName);
        }

        if (gunList.Contains(theGun.weaponName))
        {
            hasSameGun = true;
            //GetComponent<CircleCollider2D>().enabled = false;
            isConvert = true;
        }
        else
        {
            hasSameGun = false;
            //GetComponent<CircleCollider2D>().enabled = true;
            isConvert = false;
        }
        

    }
}
