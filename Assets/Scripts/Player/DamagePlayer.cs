using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public bool knockBack, isProjectile, collideDirt, bossObject, isTrigger, autoOff, dmgCompanion;
    public bool bossPoint1, bossPoint2, enemyPoint1, enemyPoint2;
    public float knockbackForce, knockbackDuration, stunDuration;
    private float timer1 = 0;
    private float timer2 = 0;

    [Header("Charged")]
    public bool chargedAtk;
    [Header("Companion")]
    public bool isCompanion;

    [HideInInspector]
    public bool followEnemy;
    [HideInInspector]
    public GameObject parentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        if (autoOff)
        {
            Invoke("Deactivate", 0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (DestructibleTile.instance != null)
        {
            Physics2D.IgnoreCollision(DestructibleTile.instance.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }*/
        if (followEnemy)
        {
            if (parentEnemy != null)
            {
                transform.position = parentEnemy.GetComponent<EnemyController>().firePoint.transform.position;
            }
        }


        if (bossPoint1)
        {
           
            transform.position = BossController.instance.effectPoint1.transform.position;
 
        }

        if (bossObject)
        {
            if (BossController.instance.currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (isTrigger)
        {
            if (PlayerHealth.instance.currentHealth > 0)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 1.5f)
                {
                    PlayerHealth.instance.DamagePlayer();
                }
            }
        }

        if (!isCompanion)
        {
            if (LevelExit.instance != null)
            {
                if (LevelExit.instance.isInvinc)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (chargedAtk)
        {

            if (GetComponentInParent<EnemyController>().stunned)
            {
                GetComponentInParent<EnemyController>().GetComponent<Animator>().Rebind();
                GetComponentInParent<EnemyController>().GetComponent<Animator>().Update(0f);
            }
          
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (knockBack == true)
            {
                if (PlayerHealth.instance.invincCount <= 0)
                {

                    StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                    while (knockbackDuration > timer1)
                    {
                        timer1 += Time.deltaTime;
                        Vector2 direction = (other.transform.position - transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                    }

                    PlayerHealth.instance.DamagePlayer();
                }

                timer1 = 0;
            }
            else
            {
                PlayerHealth.instance.DamagePlayer();
            }
        }

        if (other.CompareTag("Companion"))
        {
            if (dmgCompanion)
            {
                other.gameObject.GetComponent<CompanionController>().DamageCompanion();
            }


        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            PlayerHealth.instance.DamagePlayer();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (autoOff)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            if (knockBack == true)
            {
                if (PlayerHealth.instance.invincCount <= 0)
                {

                    StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                    while (knockbackDuration > timer1)
                    {
                        timer1 += Time.deltaTime;
                        Vector2 direction = (other.transform.position - transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                    }

                    PlayerHealth.instance.DamagePlayer();
                }

                timer1 = 0;
            }
            else
            {
                PlayerHealth.instance.DamagePlayer();
            }

        }
        if (isProjectile)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }

        if (collideDirt)
        {
            if (other.gameObject.CompareTag("Dirt"))
            {
                other.gameObject.GetComponent<DestructibleTile>().BulletCollide();
            }
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerHealth.instance.DamagePlayer();

        }
       
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (knockBack == true)
            {
                if (PlayerHealth.instance.invincCount <= 0)
                {

                    StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                    while (knockbackDuration > timer1)
                    {
                        timer1 += Time.deltaTime;
                        Vector2 direction = (other.transform.position - transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                    }

                    PlayerHealth.instance.DamagePlayer();
                }

                timer1 = 0;
            }
            else
            {
                PlayerHealth.instance.DamagePlayer();
            }

        }
        if (isProjectile)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }

    }

    public void Deactivate()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        if (GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
