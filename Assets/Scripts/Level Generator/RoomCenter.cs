using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{

    public Room theRoom;
    public GameObject Contents;
    public GameObject Enemies;
    public GameObject gunLoot, upgradeLoot, buffLoot;
    public GameObject[] maps;
    public Transform[] lootPoints1, lootPoints2, lootPoints3;
    public Transform centerPoint;
    public float Distance;
    public bool randomMap;

    // Start is called before the first frame update
    void Start()
    {
        /* if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= 10)
         {
             Contents.SetActive(false);
         }*/

        if (randomMap)
        {
            int random = Random.Range(0, maps.Length);

            maps[random].SetActive(true);
        }

        if (gunLoot != null)
        { 
            int gunRandom = Random.Range(0, lootPoints1.Length);
            Instantiate(gunLoot, lootPoints1[gunRandom].position, Quaternion.Euler(0f, 0f, 0f));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Distance == 0)
        {
            if (Contents != null)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 25)
                {
                    Contents.SetActive(true);
                }
                else
                {
                    Contents.SetActive(false);
                }

            }
            if (Enemies != null)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 20)
                {
                    Enemies.SetActive(true);
                }
                else
                {
                    Enemies.SetActive(false);
                }
            }
        }
        else
        {
            if (Contents != null)
            {
                if (Vector3.Distance(centerPoint.position, PlayerController.instance.transform.position) < Distance)
                {
                    Contents.SetActive(true);
                }
                else
                {
                    Contents.SetActive(false);
                }

            }
            if (Enemies != null)
            {
                if (Vector3.Distance(centerPoint.position, PlayerController.instance.transform.position) < 50)
                {
                    Enemies.SetActive(true);
                }
                else
                {
                    Enemies.SetActive(false);
                }
            }
        }
    }
}
