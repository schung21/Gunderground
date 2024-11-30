using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCC : MonoBehaviour
{

    public static TriggerCC instance;
    public float knockbackForce, knockbackDuration, stunDuration, dmgDuration, Duration;
    private float timer1 = 0, dmgSeconds;
    public bool isDot, isTrigger;
    public bool isFireCC, isIceCC;
    public int ccDmg;
    private PlayerBullet playerBullet;
    private bool dmgEnemy;

    // Start is called before the first frame update
    void Start()
    {
       
        instance = this;

        dmgSeconds = dmgDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDot)
        {
            if (Duration > 0)
            {
                Duration -= Time.deltaTime;

                if (isFireCC)
                {
                    if (dmgDuration > 0)
                    {
                        dmgEnemy = false;
                        dmgDuration -= Time.deltaTime;
                    }
                    else if (dmgDuration < 1)
                    {
                        StartCoroutine(DamageEnemy());

                    }
                }
             
            }
           
        }

    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Dirt"))
        {

          
        }
 
        if (other.gameObject.CompareTag("Enemy"))
        {

            if (isDot)
            {
                if (dmgDuration > 0)
                {
                    dmgDuration -= Time.deltaTime;
                }
                else if (dmgDuration <= 0)
                {

                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        other.gameObject.GetComponent<EnemyController>().DamageEnemy(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                        dmgDuration = dmgSeconds;

                        other.gameObject.GetComponent<EnemyController>().DamagePop(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                    }


                }
            }

            if (isIceCC)
            {
                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    other.gameObject.GetComponent<EnemyController>().canMove = false;
             
                }
            }


        }

    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            if (dmgEnemy == true)
            {
                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    other.gameObject.GetComponent<EnemyController>().DamageEnemy(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                    dmgDuration = dmgSeconds;

                    other.gameObject.GetComponent<EnemyController>().DamagePop(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                }

                if (other.gameObject.GetComponent<MiniBossController>() != null)
                {

                    other.gameObject.GetComponent<MiniBossController>().DamageEnemy(ccDmg);
                    dmgDuration = dmgSeconds;

                    other.gameObject.GetComponent<MiniBossController>().DamagePop(ccDmg);
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (isDot)
            {
                if (dmgDuration > 0)
                {
                    dmgDuration -= Time.deltaTime;
                }
                else if (dmgDuration <= 0)
                {

                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        other.gameObject.GetComponent<EnemyController>().DamageEnemy(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                        dmgDuration = dmgSeconds;

                        other.gameObject.GetComponent<EnemyController>().DamagePop(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                    }


                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            if (dmgEnemy == true)
            {
                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    other.gameObject.GetComponent<EnemyController>().DamageEnemy(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                    dmgDuration = dmgSeconds;

                    other.gameObject.GetComponent<EnemyController>().DamagePop(ccDmg + Mathf.RoundToInt(ccDmg * PlayerController.instance.playerDamage));
                }

                if (other.gameObject.GetComponent<MiniBossController>() != null)
                {

                    other.gameObject.GetComponent<MiniBossController>().DamageEnemy(ccDmg);
                    dmgDuration = dmgSeconds;

                    other.gameObject.GetComponent<MiniBossController>().DamagePop(ccDmg);
                }
            }

        }
    }


    public IEnumerator DamageEnemy()
    {
        dmgEnemy = true;
        yield return null;

    }
}
