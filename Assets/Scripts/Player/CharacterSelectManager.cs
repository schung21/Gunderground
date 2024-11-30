using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    public PlayerController activePlayer;
    public CharacterSelector activeCharSelect;
    public bool change, notNear, canChange;

    //[HideInInspector]
    public Gun gun1, gun2, gun3;
    //[HideInInspector]
    public string name1, name2, name3;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update

    private void Update()
    {
        if (PlayerController.instance.availableGuns.Count > 1)
        {
            name1 = PlayerController.instance.availableGuns[1].weaponName;

            if (gun1 == null || gun1.weaponName != name1)
            {
                for (int i = 0; i < PickupManager.instance.gunPickups.Count; i++)
                {
                    if (PickupManager.instance.gunPickups[i].name == name1)
                    {
                        gun1 = PickupManager.instance.gunPickups[i].GetComponent<GunPickup>().theGun;
                    }
                }
            }

            if (PlayerController.instance.availableGuns.Count > 2)
            {
                name2 = PlayerController.instance.availableGuns[2].weaponName;

                if (gun2 == null || gun2.weaponName != name2)
                {
                    for (int i = 0; i < PickupManager.instance.gunPickups.Count; i++)
                    {
                        if (PickupManager.instance.gunPickups[i].name == name2)
                        {
                            gun2 = PickupManager.instance.gunPickups[i].GetComponent<GunPickup>().theGun;
                        }

                    }
                }
            }

            if (PlayerController.instance.availableGuns.Count > 3)
            {
                name3 = PlayerController.instance.availableGuns[3].weaponName;

                if (gun3 == null || gun3.weaponName != name3)
                {
                    for (int i = 0; i < PickupManager.instance.gunPickups.Count; i++)
                    {
                        if (PickupManager.instance.gunPickups[i].name == name3)
                        {
                            gun3 = PickupManager.instance.gunPickups[i].GetComponent<GunPickup>().theGun;
                        }

                    }
                }
            }
        }

        if (notNear)
        {
            UIController.instance.interactButton.SetActive(false);
            notNear = false;
        }
    }
}
