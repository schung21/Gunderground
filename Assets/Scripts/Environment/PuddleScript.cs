using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleScript : MonoBehaviour
{

    public GameObject Effect, Effect2;
    private bool makeRipple, makeStillRipple, makeEnemyRipple, makeBossRipple;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            makeRipple = true;
            makeStillRipple = true;
            Instantiate(Effect, PlayerController.instance.theShadow.transform.position, PlayerController.instance.theShadow.transform.rotation);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            makeEnemyRipple = true;
            if (other.gameObject.GetComponent<MiniBossController>() != null)
            {
                Instantiate(Effect, other.gameObject.GetComponent<MiniBossController>().Shadow.transform.position, other.gameObject.GetComponent<MiniBossController>().Shadow.transform.rotation);
            }
            else
            {
                Instantiate(Effect, other.gameObject.GetComponent<EnemyController>().Shadow.transform.position, other.gameObject.GetComponent<EnemyController>().Shadow.transform.rotation);
            }
        }

        if (other.gameObject.CompareTag("Ripple Part"))
        {
            makeBossRipple = true;
            Instantiate(Effect2, other.gameObject.transform.position, other.gameObject.transform.rotation);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.anim.GetBool("isMoving") == true)
            {

                if (makeRipple)
                {
                    Invoke("Ripple", 0.08f);
                    makeRipple = false;
                }
                
            }

            else 
            {
                if (makeStillRipple)
                {
                    Invoke("Ripple2", 0.7f);
                    makeStillRipple = false;
                }

            }
            
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (makeEnemyRipple)
            {
                if (other.gameObject.GetComponent<MiniBossController>() != null)
                {
                    StartCoroutine(RippleMiniBoss(other.gameObject.GetComponent<MiniBossController>().Shadow));

                    makeEnemyRipple = false;

                }
                else
                {
                    StartCoroutine(RippleEnemy(other.gameObject.GetComponent<EnemyController>().Shadow));

                    makeEnemyRipple = false;
                }

            }
        }

        if (other.gameObject.CompareTag("Ripple Part"))
        {
            if (makeBossRipple)
            {

                StartCoroutine(RippleBoss(other.gameObject));

                makeBossRipple = false;

            }
        }
    }

    public void Ripple()
    {

        Instantiate(Effect, PlayerController.instance.theShadow.transform.position, PlayerController.instance.theShadow.transform.rotation);
        makeRipple = true;
  
    }
    public void Ripple2()
    {

        Instantiate(Effect, PlayerController.instance.theShadow.transform.position, PlayerController.instance.theShadow.transform.rotation);
        makeStillRipple = true;
    }

    public IEnumerator RippleEnemy (GameObject Shadow)
    {
        yield return new WaitForSeconds(0.2f);
        if (Shadow != null)
        {
            Instantiate(Effect, Shadow.transform.position, Shadow.transform.rotation);
        }
        makeEnemyRipple = true;
    }

    public IEnumerator RippleMiniBoss(GameObject Shadow)
    {
        yield return new WaitForSeconds(0.05f);
        if (Shadow != null)
        {
            Instantiate(Effect, Shadow.transform.position, Shadow.transform.rotation);
        }
        makeEnemyRipple = true;
    }


    public IEnumerator RippleBoss(GameObject Shadow)
    {
        yield return new WaitForSeconds(0.4f);
        if (Shadow != null)
        {
            Instantiate(Effect2, Shadow.transform.position, Shadow.transform.rotation);
        }
        makeBossRipple = true;
    }




}
