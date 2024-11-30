using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLog : MonoBehaviour
{
    public static GunLog instance;
    public GameObject unlockedUI, lockedUI;
    public int gunCode;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < ContentManager.instance.unlockedGuns.Count; i++)
        {
            if (ContentManager.instance.unlockedGuns[i] == 1)
            {

                if (i == gunCode)
                {
                    unlockedUI.SetActive(true);
                    lockedUI.SetActive(false);
                }


            }
        }
    }
}
