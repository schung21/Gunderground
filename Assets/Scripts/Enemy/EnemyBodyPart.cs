using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{

    public SpriteRenderer theBody;
    private Color originColor;
    public bool isWorm;

    public List<SpriteRenderer> bodies;

    private void Start()
    {
       
        originColor = theBody.color;

        /*  if (isWorm)
          {
              Destroy(GetComponent<Rigidbody2D>());
              GetComponent<BoxCollider2D>().usedByComposite = true;
          }*/

        Invoke("FillList", 1f);
    }
    private void Update()
    {
        if (GetComponentInParent<EnemyController>().health <= 0)
        {
            foreach (var a in bodies)
            {
                a.color = Color.gray;

                a.GetComponentInParent<BoxCollider2D>().enabled = false;
               
            }

            if (GetComponent<BoxCollider2D>() != null)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            theBody.color = new Color(120, 120, 120, 255);

        }


    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (GetComponentInParent<EnemyController>().health > 0)
        {

            if (other.gameObject.CompareTag("Bullets"))
            {
                if (other.gameObject.GetComponent<PlayerBullet>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<PlayerBullet>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<PlayerBullet>().avgDmg);
                }
                else if (other.gameObject.GetComponent<HomingProjectile>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<HomingProjectile>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<HomingProjectile>().avgDmg);
                }
                else if (other.gameObject.GetComponent<RicochetScript>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<RicochetScript>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<RicochetScript>().avgDmg);
                }

                foreach (var a in bodies)
                {
                    a.color = Color.red;
                }
                //theBody.color = Color.red;
                Invoke("ResetColor", 0.1f);
            }

            if (other.gameObject.CompareTag("Area Damage"))
            {
                if (other.gameObject.GetComponent<DamageArea>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<DamageArea>().areaDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<DamageArea>().areaDmg);

                    foreach (var a in bodies)
                    {
                        a.color = Color.red;
                    }
                    //theBody.color = Color.red;
                    Invoke("ResetColor", 0.1f);
                }
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponentInParent<EnemyController>().health > 0)
        {
            if (other.gameObject.CompareTag("Bullets"))
            {
                if (other.gameObject.GetComponent<PlayerBullet>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<PlayerBullet>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<PlayerBullet>().avgDmg);
                }
                else if (other.gameObject.GetComponent<HomingProjectile>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<HomingProjectile>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<HomingProjectile>().avgDmg);
                }
                else if (other.gameObject.GetComponent<RicochetScript>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<RicochetScript>().avgDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<RicochetScript>().avgDmg);
                }
                else if (other.gameObject.GetComponent<DamageEnemy>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<DamageEnemy>().stabDamage);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<DamageEnemy>().stabDamage);
                }

                foreach (var a in bodies)
                {
                    a.color = Color.red;
                }
                Invoke("ResetColor", 0.1f);
            }

            if(other.gameObject.CompareTag("Buff Effect"))
            {
                if (other.gameObject.GetComponent<DamageEnemy>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<DamageEnemy>().stabDamage);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<DamageEnemy>().stabDamage);
                }

                foreach (var a in bodies)
                {
                    a.color = Color.red;
                }
                Invoke("ResetColor", 0.1f);
            }

            if (other.gameObject.CompareTag("Area Damage"))
            {
                if (other.gameObject.GetComponent<DamageArea>() != null)
                {
                    GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<DamageArea>().areaDmg);
                    GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<DamageArea>().areaDmg);

                    foreach (var a in bodies)
                    {
                        a.color = Color.red;
                    }
                    //theBody.color = Color.red;
                    Invoke("ResetColor", 0.1f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GetComponentInParent<EnemyController>().health > 0)
        {
            if (other.gameObject.CompareTag("Bullets"))
            {
                if (other.gameObject.GetComponent<PlayerBullet>() != null)
                {
                    if (other.gameObject.GetComponent<PlayerBullet>().dotDmg)
                    {
                        GetComponentInParent<EnemyController>().DamageEnemy(other.gameObject.GetComponent<PlayerBullet>().avgDmg);
                        GetComponentInParent<EnemyController>().DamagePop(other.gameObject.GetComponent<PlayerBullet>().avgDmg);

                        Instantiate(other.gameObject.GetComponent<PlayerBullet>().impactEffect, GetComponentsInChildren<Marker>()[1].transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }

                foreach (var a in bodies)
                {
                    a.color = Color.red;
                }
                Invoke("ResetColor", 0.1f);
            }

        }
    }

    void ResetColor()
    {
        if (GetComponentInParent<EnemyController>().health > 0)
        {
            foreach (var a in bodies)
            {
                a.color = originColor;
            }
        }
  
    }

    public void FillList()
    {
        foreach(var a in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            bodies.Add(a);
        }
    }


}
