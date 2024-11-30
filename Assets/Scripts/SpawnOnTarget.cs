using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTarget : MonoBehaviour
{
    public bool Enemy, Player;
    public float Length;
    private float Timer;
    public GameObject spawnObject;

    // Start is called before the first frame update
    void Start()
    {
        Timer = Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy)
        {
           
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            else if (Timer <= 0)
            {
                Timer = Length;
                if (DecoyController.instance != null)
                {
                    Instantiate(spawnObject, DecoyController.instance.transform.position, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    Instantiate(spawnObject, PlayerController.instance.transform.position, Quaternion.Euler(0, 0, 0));
                }
             
            }
        }
    }
}
