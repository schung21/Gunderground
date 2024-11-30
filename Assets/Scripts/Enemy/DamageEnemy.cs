using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public static DamageEnemy instance;
    public int stabDamage;
    public float knockbackForce, knockbackDuration;
    public bool isMelee, hitsBoss, hitsDirt, isIce;

    Gun currentGunData;
    public float gunCritRate, critDamage = 1.5f; //burnRate, freezeRate;
    //public int bulletDamage = 50, bulletDamage2, reducedDmg;
    [HideInInspector]
    public int critChance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        stabDamage += Mathf.RoundToInt(PlayerController.instance.playerDamage * stabDamage);

        if (critChance != 0 && critDamage != 0)
        {
            if (!currentGunData.isRifle)
            {
                gunCritRate += PlayerController.instance.gunCrit1;
                critDamage += PlayerController.instance.critDmg1;
            }
            else if (currentGunData.isRifle)
            {
                gunCritRate += PlayerController.instance.gunCrit2;
                critDamage += PlayerController.instance.critDmg2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (EnemyBullet.instance != null && isMelee == true)
        {
            Physics2D.IgnoreCollision(EnemyBullet.instance.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {

            float timer = 0;

            while (knockbackDuration > timer)
            {
                timer += Time.deltaTime;
                Vector2 direction = (transform.position - other.gameObject.transform.position).normalized;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
            }

            if (isIce)
            {
                if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                {
                    if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                    {
                        other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                    }
                }
            }

            if (other.gameObject.GetComponent<EnemyController>() != null)
            {
                if (other.gameObject.GetComponent<EnemyController>().health > 0)
                {
                    other.gameObject.GetComponent<EnemyController>().DamageEnemy(stabDamage);
                    other.gameObject.GetComponent<EnemyController>().DamagePop(stabDamage);
                }
            }
            if (other.gameObject.GetComponent<MiniBossController>() != null)
            {
                if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                {
                    other.gameObject.GetComponent<MiniBossController>().DamageEnemy(stabDamage);
                    other.gameObject.GetComponent<MiniBossController>().DamagePop(stabDamage);
                }
            }

        }

        if (hitsBoss)
        {
            if (other.tag == "Boss")
            {
                if (other.gameObject.GetComponent<BossController>() != null)
                {
                    other.gameObject.GetComponent<BossController>().TakeDamage(stabDamage);
                    other.gameObject.GetComponent<BossController>().DamagePop(stabDamage);
                }
            }
        }

        if (hitsDirt)
        {
            if (other.tag == "Dirt")
            {
                if (other.gameObject.GetComponent<DestructibleTile>() != null)
                {
                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();
                }
            }

        }

    }

    /* private void OnTriggerStay2D(Collider2D other)
     {
         if (other.tag == "Enemy")
         {
             stabDamage += PlayerController.instance.playerDamage;

             PlayerHealth.instance.DamagePlayer();
             if (other.gameObject.GetComponent<EnemyController>() != null)
             {
                 other.gameObject.GetComponent<EnemyController>().DamageEnemy(stabDamage/10);
             }
             if (other.gameObject.GetComponent<MiniBossController>() != null)
             {
                 other.gameObject.GetComponent<MiniBossController>().DamageEnemy(stabDamage/10);
             }

         }
     }*/
}
