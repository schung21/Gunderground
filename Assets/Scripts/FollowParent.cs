using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public bool playerObj;
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position;

        if (playerObj)
        {
            if (PlayerController.instance.isRaccoon)
            {
                if (PlayerController.instance.dashCounter > 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
