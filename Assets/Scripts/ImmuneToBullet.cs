using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmuneToBullet : MonoBehaviour
{

    public float distancex;
 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullets"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullets"))
        { 
            if (other.gameObject.GetComponent<PlayerBullet>() != null)
            {
                if (other.gameObject.GetComponent<PlayerBullet>().dotDmg)
                {
                    
                }
                else
                {
                    other.gameObject.GetComponent<PlayerBullet>().DestroyBullet();
                }
                
            }
            if(other.gameObject.GetComponent<RicochetScript>() != null)
            {
                other.gameObject.GetComponent<RicochetScript>().DestroyBullet();
            }
            if (other.gameObject.GetComponent<HomingProjectile>() != null)
            {
                if (other.gameObject.GetComponent<HomingProjectile>().isCollide)
                {
                    other.gameObject.GetComponent<HomingProjectile>().DestroyBullet();
                }
               
            }

        }
    }

}
