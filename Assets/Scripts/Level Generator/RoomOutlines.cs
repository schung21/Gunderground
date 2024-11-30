using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOutlines : MonoBehaviour
{
    public GameObject Contents;
    public int playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        /* if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= 10)
         {
             Contents.SetActive(false);
         }*/

        if(playerDistance == 0)
        {
            playerDistance = 50;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Contents != null)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerDistance)
            {
                Contents.SetActive(true);
            }

            else
            {
                Contents.SetActive(false);
            }
        }
    }
}
