using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public static EnemyBullet instance;
    public float speed;
    [HideInInspector]
    public Vector3 direction;
    public bool allDirection, isPenetrate, noDamage, changeToNoDmg, isUp, isArrow, ignoreBlock, dontDestroy;
    public Rigidbody2D theRB;
    public GameObject impactDirtEffect, impactEffect, newImpactEffect;
    public Sprite newSprite;

    /*[HideInInspector]*/
    public bool aimDecoy, aimDecoy2;
    [HideInInspector]
    public GameObject allyDecoy;

    private int random;
  

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        /*    if (Vector3.Distance(DecoyController.instance.transform.position, transform.position) <= 8)
            {
                direction = (DecoyController.instance.transform.position - transform.position).normalized;
            }
            else
            {
                direction = PlayerController.instance.transform.position - transform.position;
            }*/
       

        if (aimDecoy)
        {
            
            direction = (allyDecoy.transform.position - transform.position).normalized;
        }

        else if (aimDecoy2)
        {
            if (DecoyController.instance != null)
            {
                direction = (DecoyController.instance.transform.position - transform.position).normalized;
            }
            else
            {
                direction = PlayerController.instance.transform.position - transform.position;
            }
        }

        else
        {
          
            direction = PlayerController.instance.transform.position - transform.position;
        }
  
     
        direction.Normalize();

        random = Random.Range(0, 10);

    }

    // Update is called once per frame
    void Update()
    {
       
        if (allDirection)
        {
            if (!isUp)
            {
                theRB.velocity = transform.right * speed;
            }
            else
            {
                theRB.velocity = transform.up * speed;
            }
        }
        else
        {
            //make bullet go towards player
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
        if(newSprite != null && BossController.instance != null && BossController.instance.secondPhase == true)
        {
            
            if (random >= 5)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
                if (newImpactEffect != null)
                {
                    impactEffect = newImpactEffect;
                }
                if (changeToNoDmg)
                {
                    noDamage = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(other.GetComponent<EnemyController>() != null)
            {
                if (other.GetComponent<EnemyController>().isObject)
                {
                    other.GetComponent<EnemyController>().DamageEnemy(20);
                }
            }
        }
        if (other.CompareTag("Player"))
        {
            if (!noDamage)
            {
                PlayerHealth.instance.DamagePlayer();

            }
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
         
        }

        if (other.CompareTag("Companion"))
        {
            if (!noDamage)
            {
                other.gameObject.GetComponent<CompanionController>().DamageCompanion();

            }
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }


        }

        /* if (other.tag == "Breakable")
         {
             Instantiate(PlayerBullet.instance.impactEffect, transform.position, transform.rotation);
             other.GetComponent<Breakable>().BulletCollide();
         }
 */
        if (!ignoreBlock)
        {
            if (other.tag == "Dirt")
            {
                other.GetComponent<DestructibleTile>().BulletCollide();

                if (impactDirtEffect != null)
                {
                    Instantiate(impactDirtEffect, transform.position, transform.rotation);
                }

                if (impactEffect != null)
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }

            }


            if (other.tag == "Wall")
            {
                if (impactEffect != null)
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
                if (isArrow)
                {
                    speed = 0;

                    GetComponent<CircleCollider2D>().enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }



        if (!isPenetrate && !ignoreBlock)
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(gameObject);
        }

    }

 

    private void OnBecameInvisible()
    {
        if (!dontDestroy)
        {
            Destroy(gameObject);
        }
    }

}
