using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PickupButton : Button
{
    public static PickupButton instance;
    public bool pickupZone;


    protected override void Start()
    {
        onClick.AddListener(Interact);
        
    }

    public void Interact()
    {

        if (!LevelManager.instance.isCamp)
        {
            if (CharacterSelector.instance != null)
            {
                if (Vector3.Distance(CharacterSelector.instance.transform.position, PlayerController.instance.transform.position) < 3f)
                {
                    if (CharacterSelector.instance.isCompanion)
                    {
                        CharacterSelector.instance.change2 = true;
                    }
                }
            }
        }

        if (PickupManager.instance != null)
        {
            if (PickupManager.instance.inGunZone == true)
            {

                if (PlayerController.instance.availableGuns.Count <= 2)
                {
                    PickupManager.instance.pickItemUp = true;
                }
                else if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName != "Pistol")
                {
                    PickupManager.instance.pickItemUp = true;
                }
            }
            else if (PickupManager.instance.inShopZone == true)
            {
                if (LevelManager.instance.isCamp)
                {
                    UIController.instance.confirmGemItem();
                }
                else
                {
                    PlayerController.instance.buyItem = true;
                }

            }
            else if (LevelManager.instance.isCamp)
            {

                if (CharacterSelectManager.instance.canChange)
                {
                    CharacterSelectManager.instance.change = true;
                }
                else if (GunRack.instance.isRack == true)
                {
                    UIController.instance.GunLog();
                }
                else if (Locker.instance.isLocker == true)
                {
                    UIController.instance.CheckCharacter();
                }
                else if (VendingMachine.instance.inVendZone)
                {
                    VendingMachine.instance.buttonClicked();
                }
                else if (VendingMachine2.instance.inVendZone)
                {
                    VendingMachine2.instance.buttonClicked();
                }
                else if (VendingMachine3.instance.inVendZone)
                {
                    VendingMachine3.instance.buttonClicked();
                }
                else if (GrenadeCraft.instance.inZone)
                {
                    GrenadeCraft.instance.craftConfirm();
                }
                else if (GemShopController.instance.inGemShopZone)
                {
                    GemShopController.instance.openGemShop();
                }
                else if (Leaderboards.instance.inBoardZone)
                {
                    Leaderboards.instance.ShowLeaderBoard();
                }
                else if (RuneObject.instance.inRuneZone)
                {
                    UIController.instance.OpenRuneMenu();
                }
            }
        }
    }

    //Extra 
    /*CharacterSelector.instance != null && !GunRack.instance.isRack && !Locker.instance.isLocker &&
                !VendingMachine.instance.inVendZone && !VendingMachine2.instance.inVendZone && !VendingMachine3.instance.inVendZone
                && !PickupManager.instance.inGunZone && !GrenadeCraft.instance.inZone && !GemShopController.instance.inGemShopZone && !Leaderboards.instance.inBoardZone
                && !RuneObject.instance.inRuneZone*/


}
