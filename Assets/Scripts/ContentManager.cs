using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;



public class ContentManager : MonoBehaviour
{
    public static ContentManager instance;
    public List<int> unlockedGuns;
    public List<int> unlockedChars;
    public List<int> unlockedParts;

    public List<GameObject> charSelectors;

    // Start is called before the first frame update
    void Start()
    {
        unlockedGuns = CharTracker.instance.unlockedGuns;
        unlockedChars = CharTracker.instance.unlockedChars;
      
        instance = this;

        if (SceneManager.GetActiveScene().name == "0")
        {
            //load unlocked characters
            for (int i = 0; i < charSelectors.Count; i++)
            {
                if (unlockedChars[i] == 1)
                {
                    charSelectors[i].SetActive(true);
                }
                if (unlockedChars[i] == 2)
                {
                    charSelectors[i].SetActive(true);
                    charSelectors[i].GetComponent<CharacterSelector>().isAvailable = true;
                }
            }

            //PrefabUtility.SavePrefabAsset(Resources.Load("prefabs/Breakable/Ores/Gun Ore 1") as GameObject);

            //var managerPrefab = Resources.Load("prefabs/Level Essentials/UI - Managers/Managers") as GameObject;
            var orePrefab = Resources.Load("prefabs/Breakable/Ores/Gun Ore 1") as GameObject;
            var orePrefab2 = Resources.Load("prefabs/Breakable/Ores/Shop Ores/Shop Gun Ore") as GameObject;

            //adding found guns to shop
            for (int i = 0; i < unlockedGuns.Count; i++)
            {
                if (unlockedGuns[i] == 1)
                {

                    GameObject[] gunPrefab = Resources.LoadAll<GameObject>("prefabs/Items/Weapon Pickups/");
                    //var a = Resources.Load("prefabs/Items/Weapon Pickups/Rare/Red's AK") as GameObject;

                    for (int a = 0; a < gunPrefab.Length; a++)
                    {
                        if (i == gunPrefab[a].GetComponent<GunPickup>().gunCode && gunPrefab[a].GetComponent<GunPickup>().isLocked == true)
                        {
                            if (gunPrefab[a].GetComponent<GunPickup>().Rare == true)
                            {

                                if (//!orePrefab.GetComponent<DestructibleTile>().rareDrops.Contains(gunPrefab[a]) &&
                                    !orePrefab2.GetComponent<DestructibleTile>().rareDrops.Contains(gunPrefab[a]))
                                {
                                    //orePrefab.GetComponent<DestructibleTile>().rareDrops.Add(gunPrefab[a]);
                                    orePrefab2.GetComponent<DestructibleTile>().rareDrops.Add(gunPrefab[a]);

                                    //managerPrefab.GetComponent<PickupManager>().gunPickups.Add(gunPrefab[a]);
                                }
                            }

                            if (gunPrefab[a].GetComponent<GunPickup>().Legend == true)
                            {
                                if (//!orePrefab.GetComponent<DestructibleTile>().legendDrops.Contains(gunPrefab[a]) &&
                                    !orePrefab2.GetComponent<DestructibleTile>().legendDrops.Contains(gunPrefab[a]))
                                {
                                    //orePrefab.GetComponent<DestructibleTile>().legendDrops.Add(gunPrefab[a]);
                                    orePrefab2.GetComponent<DestructibleTile>().legendDrops.Add(gunPrefab[a]);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    // Update is called once per frame
    /*  void Update()
      {

          *//*for (int i = 0; i < unlockedGuns.Count; i++)
          {
              if (unlockedGuns[i] == 1)
              {

                  GameObject[] gunPrefab = Resources.LoadAll<GameObject>("prefabs/Items/Weapon Pickups/");

                  for (int a = 0; a < gunPrefab.Length; a++)
                  {
                      if (i == gunPrefab[a].GetComponent<GunPickup>().gunCode && gunPrefab[a].GetComponent<GunPickup>().isLocked == true)
                      {
                          if (!PickupManager.instance.gunPickups.Contains(gunPrefab[a]))
                          {
                              PickupManager.instance.gunPickups.Add(gunPrefab[a]);
                          }

                      }
                  }


              }
          }*//*

      }*/

    public void saveGun(int gunCode)
    {
        unlockedGuns[gunCode] = 1;
       
        CharTracker.instance.SavePlayer();
    }
    public void saveCharacter1(int charCode)
    {
        unlockedChars[charCode] = 1;

        CharTracker.instance.SavePlayer();
    }
    public void saveCharacter2(int charCode)
    {
        unlockedChars[charCode] = 2;

        CharTracker.instance.SavePlayer();
    }
}
