using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{

    public static BossAnimationController instance;
    public Animator anim;
    public bool isAttack1, isAttack2, isAttack3, isAttack4, isAttack5, isAttack6, isAttack7, isAttack8, isAttack9, isAttack10;
    public bool phaseSwitch;
    public bool Idle1, Idle2;
    public bool isClone, noMoving;

    private CameraController camera;

    public GameObject feetStomp;
    public GameObject Audio1, Audio2;

    [Header("Sprite Order Change")]
    public SpriteRenderer object1, object2;
    private int ogSortOrder;
    private string ogSortName;

    [HideInInspector]
    public bool playMovingSound;

    void Start()
    {
        instance = this;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playMovingSound = true;

    }

    // Update is called once per frame
    void Update()
    {
     
        if(isAttack1 == true)
        {
            anim.SetTrigger("Attack1");
            isAttack1 = false;
        }
        if (isAttack2 == true)
        {
            anim.SetTrigger("Attack2");
            isAttack2 = false;
        }
        if(isAttack3 == true)
        {
            anim.SetTrigger("Attack3");
            isAttack3 = false;
        }
        if (isAttack4 == true)
        {
            anim.SetTrigger("Attack4");
            isAttack4 = false;
        }
        if (isAttack5 == true)
        {
            anim.SetTrigger("Attack5");
            isAttack5 = false;
        }
        if (isAttack6 == true)
        {
            anim.SetTrigger("Attack6");
            isAttack6 = false;
        }
        if (isAttack7 == true)
        {
            anim.SetTrigger("Attack7");
            isAttack7 = false;
        }
        if (isAttack8 == true)
        {
            anim.SetTrigger("Attack8");
            isAttack8 = false;
        }
        if (isAttack9 == true)
        {
            anim.SetTrigger("Attack9");
            isAttack9 = false;
        }
        if (isAttack10 == true)
        {
            anim.SetTrigger("Attack10");
            isAttack10 = false;
        }
        if (phaseSwitch == true)
        {
            anim.SetTrigger("phaseSwitch");
            phaseSwitch = false;
        }
        if (Idle1)
        {
            anim.SetBool("Idle1", true);
        }
        if (Idle2)
        {

            anim.SetBool("Idle2", true);
        }


    }

    public void attack1Effect()
    {

        if (BossController.instance.Effects2[0] != null)
        {
            int random = Random.Range(0, 2);

            if (random == 1)
            {
                Instantiate(BossController.instance.Effects[0], BossController.instance.effectPoint1.position, BossController.instance.effectPoint1.rotation);

            }
            else
            {
                Instantiate(BossController.instance.Effects2[0], BossController.instance.effectPoint1.position, BossController.instance.effectPoint1.rotation);

            }
        }
        else
        {

            Instantiate(BossController.instance.Effects[0], BossController.instance.effectPoint1.position, BossController.instance.effectPoint1.rotation);

        }

    }
    public void attack2Effect()
    {

        if (BossController.instance.Effects2[1] != null)
        {
            int random = Random.Range(0, 2);

            if (random == 1)
            {
                Instantiate(BossController.instance.Effects[1], BossController.instance.effectPoint2.position, BossController.instance.effectPoint2.rotation);

            }
            else
            {
                Instantiate(BossController.instance.Effects2[1], BossController.instance.effectPoint2.position, BossController.instance.effectPoint2.rotation);

            }
        }
        else
        {
            Instantiate(BossController.instance.Effects[1], BossController.instance.effectPoint2.position, BossController.instance.effectPoint2.rotation);
        }
        
    }
    public void attack3Effect()
    {

        if (BossController.instance.Effects2[2] != null)
        {
            int random = Random.Range(0, 2);

            if (random == 1)
            {
                Instantiate(BossController.instance.Effects[2], BossController.instance.effectPoint3.position, BossController.instance.effectPoint3.rotation);

            }
            else
            {
                Instantiate(BossController.instance.Effects2[2], BossController.instance.effectPoint3.position, BossController.instance.effectPoint3.rotation);

            }
        }
        else
        {
            Instantiate(BossController.instance.Effects[2], BossController.instance.effectPoint3.position, BossController.instance.effectPoint3.rotation);
        }
      
 
    }
    public void attack4Effect()
    {
        if (BossController.instance.Effects2[3] != null)
        {
            int random = Random.Range(0, 2);

            if (random == 1)
            {
                Instantiate(BossController.instance.Effects[3], BossController.instance.effectPoint4.position, BossController.instance.effectPoint4.rotation);

            }
            else
            {
                Instantiate(BossController.instance.Effects2[3], BossController.instance.effectPoint4.position, BossController.instance.effectPoint4.rotation);

            }
        }
        else
        {
            Instantiate(BossController.instance.Effects[3], BossController.instance.effectPoint4.position, BossController.instance.effectPoint4.rotation);
        }


    }

    public void attack5Effect()
    {
        Instantiate(BossController.instance.Effects[4], BossController.instance.effectPoint5.position, BossController.instance.effectPoint5.rotation);

    }

    public void attack6Effect()
    {
        Instantiate(BossController.instance.Effects[5], BossController.instance.effectPoint6.position, BossController.instance.effectPoint6.rotation);

    }

    public void attack7Effect()
    {
        Instantiate(BossController.instance.Effects[6], BossController.instance.effectPoint7.position, BossController.instance.effectPoint7.rotation);

    }



    //Trigger Effect1 two times 
    public void doubleEffect1()
    {
        
            Instantiate(BossController.instance.Effects[0], BossController.instance.effectPoint1.position, BossController.instance.effectPoint1.rotation);
            Instantiate(BossController.instance.Effects[0], BossController.instance.effectPoint2.position, BossController.instance.effectPoint2.rotation);
        
    }

    public void attack1Follow()
    {

        Instantiate(BossController.instance.Effects[0], BossController.instance.effectPoint1.transform);

    }
    public void attack2Follow()
    {

        Instantiate(BossController.instance.Effects[1], BossController.instance.effectPoint2.transform);

    }
    public void attack3Follow()
    {

        Instantiate(BossController.instance.Effects[2], BossController.instance.effectPoint3.transform);

    }
    public void attack5Follow()
    {

        Instantiate(BossController.instance.Effects[4], BossController.instance.effectPoint5.transform);

    }

    public void targetPlayer()
    {
        FollowPlayer.instance.Return = false;
        FollowPlayer.instance.Follow = true;

    }

    public void stopChasing()
    {
        FollowPlayer.instance.Follow = false;
        FollowPlayer.instance.Stop = true;

    }

    public void returnToPoint()
    {
        FollowPlayer.instance.Stop = false;
        FollowPlayer.instance.Return = true;

    }

    public void adjustPos()
    {
        if (FollowPlayer.instance.transform.position.y != FollowPlayer.instance.startPoint.y)
        {
            FollowPlayer.instance.transform.position = BossController.instance.partPoints[0].transform.position;
        }

    }

    public void ReleaseCharge()
    {
        BossController.instance.ReleaseCharge();
    }

    public void ShakeCamera()
    {
        camera.camShake();
    }

    public void ShakeCamera2()
    {
        camera.weakCamShake();
    }

    public void SpawnOnPlayer()
    {

    }
    
    public void EndStartAnimation()
    {
        GetComponentInChildren<BossController>().endStartAnim = true;
        GetComponentInChildren<BossController>().theBody.enabled = true;
        GetComponentInChildren<BossController>().theBody.sortingLayerName = "Player";
        GetComponentInChildren<BossController>().theBody.sortingOrder = 2;
        GetComponentInChildren<BossController>().rangeFromPlayer = 100;
        GetComponentInChildren<CircleCollider2D>().offset = new Vector2(0f, 0f);

        if (GetComponentInChildren<BossController>().gunArm != null)
        {
            GetComponentInChildren<BossController>().gunArm.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
        if(GetComponentInChildren<BossController>().weapons.Length != 0) 
        {
            
            GetComponentInChildren<BossController>().weapons[0].GetComponent<SpriteRenderer>().enabled = true;
            
        }
        if (GetComponentInChildren<BossController>().theShadow != null)
        {
            GetComponentInChildren<BossController>().theShadow.enabled = true;
        }
        if (GetComponentInChildren<BossController>().Parts != null)
        {
            GetComponentInChildren<BossController>().Parts.transform.position = new Vector3(GetComponentInChildren<BossController>().Parts.transform.position.x,
                GetComponentInChildren<BossController>().Parts.transform.position.y, 0f);
        }

    }

    public void switchDone()
    {
        BossController.instance.phaseSwitch = false;
    }

    public void playDashSound()
    {

        if (playMovingSound)
        {
            if (Audio1 != null)
            {
                Instantiate(Audio1, transform.position, transform.rotation);
                playMovingSound = false;
            }
        }
    }

    public void playSound1()
    {
        Instantiate(Audio2, transform.position, transform.rotation);
    }

    public void StompSound()
    {
        //AudioSource.PlayClipAtPoint(feetStomp, transform.position);
        Instantiate(feetStomp, transform.position, transform.rotation);
    }

    public void DestroyBar()
    {
        BossController.instance.exitBarrier.GetComponent<DestroyOnTime>().enabled = true;
        BossController.instance.exitBarrier.GetComponent<DestroyOnTime>().countDown = 0;
    }

    public void SortGunLayer()
    {
        GetComponentInChildren<BossController>().theBody.sortingOrder = 7;
    }

    public void GoOverPlayer()
    {
        ogSortName = object1.sortingLayerName;
        ogSortOrder = object1.sortingOrder;

        object1.sortingLayerName = "Player";
        object1.sortingOrder = 7;

        if (object2 != null)
        {
            object2.sortingLayerName = "Player";
            object2.sortingOrder = 7;
        }
    }

    public void ResetLayer()
    {
        object1.sortingLayerName = ogSortName;
        object1.sortingOrder = ogSortOrder;

        if (object2 != null)
        {

            object2.sortingLayerName = ogSortName;
            object2.sortingOrder = ogSortOrder;

        }
    }
}
