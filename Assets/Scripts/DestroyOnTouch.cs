using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    public bool isParticle;
  
    // Update is called once per frame
    void Update()
    {
        if (BossController.instance != null)
        {
            GameObject[] immuneObj = GameObject.FindGameObjectsWithTag("Skeleton");
            foreach (var a in immuneObj)
            {
                if (a.GetComponent<CircleCollider2D>().enabled == true)
                {
                    if (Vector3.Distance(transform.position, a.transform.position) <= 2)
                    {
                        Destroy(gameObject);
                    }
                }
            }
         
        }
    }

}
