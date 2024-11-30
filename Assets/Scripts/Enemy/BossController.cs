using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    public BossAnimationController bossAnim;
    public Animator anim;
    public bool isClone, animFin;
    public BossAction[] actions;
    public BossSequence[] sequences;
    public int currentSequence;

    private int currentAction;
    private float actionCounter, initialCounter;

    public float rangeFromPlayer;
    private float shotCounter;
    public Rigidbody2D theRB;
    private Vector2 moveDirection;
    public int currentHealth;

    public GameObject deathEffect;
    public GameObject levelExit, exitBarrier;
    public GameObject healthDrop;
    public GameObject critEffect;
    public GameObject Parts;
    public GameObject[] Effects, Effects2, Drops;


    public Transform critPoint, effectPoint1, effectPoint2, effectPoint3, effectPoint4, effectPoint5, effectPoint6, effectPoint7;
    public Transform[] partPoints;

    public SpriteRenderer theBody, theShadow;
    public Sprite newSprite;
    Color originColor;

    public float vectorDis;

    public bool isFloat, hasGun, endStartAnim, noDodge, lastBoss;
    [HideInInspector]
    public bool stopTeleport, canMove, secondPhase, startAnimPlaying, phaseSwitch;

    public Transform gunArm;
    public GameObject[] weapons;
    private Vector2 gunDirection;

    public GhostEffect ghostEffect;
    private bool createNumb;
    private int randomNumb;
    public int expPoints;

    [Header("Character Unlock")]
    public bool unlockChar1;
    public bool unlockChar2;
    public int codeToUnlock;
    public GameObject Message;

    private bool noHealth, expGained;
    public bool cutAnim;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        expGained = false;
        phaseSwitch = true;
        stopTeleport = true;
        canMove = true;
        endStartAnim = false;
        startAnimPlaying = true;
        actions = sequences[currentSequence].actions;
        actionCounter = Random.Range(actions[currentAction].actionLength1, actions[currentAction].actionLength2);
        originColor = theBody.color;
        createNumb = true;

        UIController.instance.bossHealthSlider.maxValue = currentHealth;
        UIController.instance.bossHealthSlider.value = currentHealth;

        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = false;
        }

        if (isFloat)
        {
            Physics2D.IgnoreLayerCollision(8, 12);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(8, 12, false);
        }
  
        Physics2D.IgnoreLayerCollision(8, 15);

        if (startAnimPlaying)
        {
            GetComponent<CircleCollider2D>().offset = new Vector2(0f, 1000f);

            theBody.enabled = false;

            theBody.sortingLayerName = "Ground";
            theBody.sortingOrder = -10;

            if(theShadow != null)
            {
                theShadow.enabled = false;
            }
            if(gunArm != null)
            {
                gunArm.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            if(Parts != null)
            {
                Parts.transform.position = new Vector3(Parts.transform.position.x, Parts.transform.position.y, -50f);
            }
            if(weapons.Length != 0)
            {
             
               weapons[0].GetComponent<SpriteRenderer>().enabled = false;
                
            }

        }

        noHealth = false;

    }

    // Update is called once per frame
    void Update()
    {
       
        if(currentHealth <= 0)
        {
            Physics2D.IgnoreLayerCollision(8, 7, true);

            if (!noHealth && cutAnim)
            {
                noHealth = true;
                TakeDamage(0);
            }
        }

        if (hasGun && currentHealth > 0)
        {

            Vector2 targetPos = PlayerController.instance.transform.position;
            gunDirection = targetPos - (Vector2)transform.position;


            if (PlayerController.instance.transform.position.x > transform.position.x)
            {

                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;

            }
            else if (PlayerController.instance.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (gunDirection != null)
            {

                gunArm.transform.up = gunDirection;

            }
        }

        if (canMove)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeFromPlayer && !endStartAnim && startAnimPlaying)
            {
                
                if (BossAnimationController.instance != null)
                {
                    if (lastBoss && LevelManager.instance.storyCode == 0)
                    {
                        BossAnimationController.instance.anim.SetTrigger("Start2");
                    }
                    else
                    {
                        BossAnimationController.instance.anim.SetTrigger("Start");
                    }
                }
                else
                {
                    GetComponent<Animator>().enabled = true;
                    anim.SetTrigger("Start");
                }

                startAnimPlaying = false;

                if (LevelManager.instance.isTallBoss)
                {
                    PlayerController.instance.mainCam.GetComponent<CameraController>().shiftCam();
                }
            }

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 0 &&
                Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 0.5)
            {
                actions[currentAction].shouldMove = false;

            }
            else
            {
                actions[currentAction].shouldMove = true;
            }

            if (actions[currentAction].resetShotCount)
            {
                if (shotCounter > 0.1)
                {
                    shotCounter = 0.1f;
                }
                //actions[currentAction].resetShotCount = false;


            }

            if (actions[currentAction].resetFireTime)
            {
               
                shotCounter = 0.1f;
                actions[currentAction].resetFireTime = false;


            }
            if (actions[currentAction].skipAction && !phaseSwitch && secondPhase)
            {
                actions[currentAction].actionLength1 = 0;
                actions[currentAction].actionLength2 = 0;
                currentAction++;
            }

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeFromPlayer && endStartAnim)
            {
               
                UIController.instance.bossHealthSlider.gameObject.SetActive(true);
                rangeFromPlayer = 100;

                if (actionCounter > 0)
                {
                    if(actions[currentAction].itemsToActive.Length > 0)
                    {
                        actions[currentAction].itemsToActive[0].SetActive(true);

                        if (actions[currentAction].itemsToActive[1] != null)
                        {
                            actions[currentAction].itemsToActive[1].SetActive(true);
                        }
                    }

                    actionCounter -= Time.deltaTime;

                    //movements
                    moveDirection = Vector2.zero;

                    if (actions[currentAction].shouldMove)
                    {

                        if (actions[currentAction].shouldChasePlayer)
                        {
                            if (BossAnimationController.instance != null)
                            {
                                if (!BossAnimationController.instance.noMoving)
                                {
                                    BossAnimationController.instance.anim.SetBool("isMoving", true);
                                }
                            }

                            moveDirection = PlayerController.instance.transform.position - transform.position;
                            moveDirection.Normalize();

                            if (ghostEffect != null)
                            {
                                Physics2D.IgnoreLayerCollision(7, 8);
                                ghostEffect.isActive = true;
                            }
                        }
                        if (actions[currentAction].moveToPoints) //Vector3.Distance(transform.position, actions[currentAction].pointToMove.position) > 1f)
                        {
                            if (BossAnimationController.instance != null)
                            {
                                if (!BossAnimationController.instance.noMoving)
                                {
                                    BossAnimationController.instance.anim.SetBool("isMoving", true);
                                }
                            }

                            moveDirection = actions[currentAction].pointToMove.position - transform.position;
                            
                            //moveDirection.Normalize();

                            if (ghostEffect != null)
                            {
                               
                                if (!actions[currentAction].noGhost)
                                {
                                   
                                    if (Vector3.Distance(transform.position, actions[currentAction].pointToMove.position) > 1f)
                                    {
                                        if (BossAnimationController.instance.playMovingSound)
                                        {
                                            BossAnimationController.instance.playDashSound();
                                        }

                                        ghostEffect.isActive = true;
                                        if (!noDodge)
                                        {
                                            Physics2D.IgnoreLayerCollision(7, 8);
                                        }


                                    }
                                    else
                                    {

                                        ghostEffect.isActive = false;
                                        Physics2D.IgnoreLayerCollision(8, 7, false);

                                    }
                                }
                                

                            }
                        }
                        if (actions[currentAction].teleportToPoint)
                        {
                            if (stopTeleport)
                            {
                                Invoke("Teleport", 0.5f);
                            }
                            
                        }
                    }


                    theRB.velocity = moveDirection * actions[currentAction].moveSpeed;
                    
                   

                    //shooting
                    if (actions[currentAction].shouldShoot || actions[currentAction].shouldShootPlayer)
                    {
                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            shotCounter = actions[currentAction].timeBetweenShots;

                            if (actions[currentAction].shouldShoot)
                            {
                                foreach (Transform t in actions[currentAction].shotPoints)
                                {
                                    Instantiate(actions[currentAction].itemToShoot, t.position, t.rotation);
                                }

                                if (hasGun)
                                {
                                    bossAnim.anim.SetTrigger("Attack");
                                }
                            }

                            if (actions[currentAction].shouldShootPlayer)
                            {
                                Instantiate(actions[currentAction].itemToShootPlayer, actions[currentAction].mainShotPoint.position,
                                    actions[currentAction].mainShotPoint.rotation);

                                if (hasGun)
                                {
                                    bossAnim.anim.SetTrigger("Attack");
                                }
                            }
                        }
                    }
                 

                    if (actions[currentAction].Attack1 == true)
                    {                 
                        BossAnimationController.instance.isAttack1 = true;
                        actions[currentAction].Attack1 = false;
                    }
                    if (actions[currentAction].Attack2 == true)
                    {
                        BossAnimationController.instance.isAttack2 = true;
                        actions[currentAction].Attack2 = false;
                    }
                    if (actions[currentAction].Attack3 == true)
                    {
                        BossAnimationController.instance.isAttack3 = true;
                        actions[currentAction].Attack3 = false;
                    }
                    if (actions[currentAction].Attack4 == true)
                    {
                        BossAnimationController.instance.isAttack4 = true;
                        actions[currentAction].Attack4 = false;
                    }
                    if (actions[currentAction].Attack5 == true)
                    {
                        BossAnimationController.instance.isAttack5 = true;
                        actions[currentAction].Attack5 = false;
                    }
                    if (actions[currentAction].Attack6 == true)
                    {
                        BossAnimationController.instance.isAttack6 = true;
                        actions[currentAction].Attack6 = false;
                    }
                    if (actions[currentAction].Attack7 == true)
                    {
                        BossAnimationController.instance.isAttack7 = true;
                        actions[currentAction].Attack7 = false;
                    }
                    if (actions[currentAction].Attack8 == true)
                    {
                        BossAnimationController.instance.isAttack8 = true;
                        actions[currentAction].Attack8 = false;
                    }
                    if (actions[currentAction].Attack9 == true)
                    {
                        BossAnimationController.instance.isAttack9 = true;
                        actions[currentAction].Attack9 = false;
                    }
                    if (actions[currentAction].Attack10 == true)
                    {
                        BossAnimationController.instance.isAttack10 = true;
                        actions[currentAction].Attack10 = false;
                    }
                    if (actions[currentAction].phaseSwitch && phaseSwitch)
                    {
                        BossAnimationController.instance.phaseSwitch = true;
                        actions[currentAction].phaseSwitch = false;
                    }

                }
                else
                {
                    if (animFin)
                    {
                        if (currentHealth <= sequences[currentSequence].endSequenceHealth && !actions[currentAction].noTransition)
                        {
                            ChangePhase();
                        }
                        else
                        {
                            if (actions[currentAction].itemsToActive.Length > 0)
                            {
                                actions[currentAction].itemsToActive[0].SetActive(false);

                                if (actions[currentAction].itemsToActive[1] != null)
                                {
                                    actions[currentAction].itemsToActive[1].SetActive(false);
                                }

                            }

                            ghostEffect.isActive = false;

                            currentAction++;
                            if (currentAction >= actions.Length)
                            {
                                currentAction = 0;
                            }

                            stopTeleport = true;
                            CheckAtkNumber();
                            ResetCD();

                            actionCounter = Random.Range(actions[currentAction].actionLength1, actions[currentAction].actionLength2);


                            if (BossAnimationController.instance != null)
                            {
                                BossAnimationController.instance.anim.SetBool("isMoving", false);
                              
                            }
                        }
                    }
                    else
                    {

                        if (actions[currentAction].itemsToActive.Length > 0)
                        {
                            actions[currentAction].itemsToActive[0].SetActive(false);

                            if (actions[currentAction].itemsToActive[1] != null)
                            {
                                actions[currentAction].itemsToActive[1].SetActive(false);
                            }

                        }

                        if (ghostEffect != null)
                        {
                            ghostEffect.isActive = false;
                        }

                        currentAction++;
                        if (currentAction >= actions.Length)
                        {
                            currentAction = 0;
                        }

                        stopTeleport = true;
                        CheckAtkNumber();
                        ResetCD();

                        actionCounter = Random.Range(actions[currentAction].actionLength1, actions[currentAction].actionLength2);


                        if (BossAnimationController.instance != null)
                        {
                            BossAnimationController.instance.anim.SetBool("isMoving", false);
                            BossAnimationController.instance.playMovingSound = true;
                        }
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        

        currentHealth -= damage;
        theBody.color = Color.red;
        Invoke("ResetColor", 0.05f);


        if (currentHealth <= 0)
        {
            if (LevelManager.instance.isTallBoss)
            {
                PlayerController.instance.mainCam.GetComponent<CameraController>().resetCam();
            }
            if (BossAnimationController.instance != null)
            {
                BossAnimationController.instance.anim.SetBool("isMoving", false);
            }

            if (unlockChar1 && ContentManager.instance.unlockedChars[codeToUnlock] != 1)
            {
                ContentManager.instance.unlockedChars[codeToUnlock] = 1;
            }
            else if (unlockChar2 && ContentManager.instance.unlockedChars[codeToUnlock] != 2)
            {
                ContentManager.instance.unlockedChars[codeToUnlock] = 2;
                Message.SetActive(true);
            }

            Physics2D.IgnoreLayerCollision(8, 12, false);
            Physics2D.IgnoreLayerCollision(8, 15, false);
            Physics2D.IgnoreLayerCollision(7, 8, false);

            canMove = false;
            //gameObject.SetActive(false);
            //ExpManager.instance.CollectExp(expPoints);

            if (expPoints > 0 && !expGained)
            {
                expGained = true;
                //if exp boost active (expPoints*2)
                ExpManager.instance.CollectExp(expPoints);

                if (PlayerController.instance.expBuff)
                {
                    if (SkinManager.instance.currentSkinCode != 0)
                    {
                        var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.4f).ToString() + "xp";
                    }
                    else
                    {
                        var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.3f).ToString() + "xp";
                    }
                }
                else
                {
                    if (SkinManager.instance.currentSkinCode != 0)
                    {
                        var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.1f).ToString() + "xp";
                    }
                    else
                    {
                        var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        expPop.GetComponentInChildren<Text>().text = "+" + expPoints.ToString() + "xp";
                    }

                }


            }

            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().enabled = true;
                anim.SetBool("isDead", true);
            }

            else if (BossAnimationController.instance != null)
            {
                if (lastBoss && LevelManager.instance.storyCode == 0)
                {
                    BossAnimationController.instance.anim.SetBool("isDead2", true);
                }
                else
                {
                    BossAnimationController.instance.anim.SetBool("isDead", true);
                }
            }

            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            if (levelExit.GetComponent<LevelExit>().isCenter)
            {
                if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
                {

                    PlayerController.instance.transform.position += new Vector3(4f, 0f, 0f);
                }
            }
            else
            {
                if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
                {
                    if (Vector3.Distance(transform.position, levelExit.transform.position) < 2f)
                    {
                        levelExit.transform.position += new Vector3(6f, 0f, 0f);
                    }
                    else
                    {
                        levelExit.transform.position += new Vector3(4f, 0f, 0f);
                    }

                }
                if (Vector3.Distance(transform.position, levelExit.transform.position) < 2f)
                {

                    if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
                    {
                        levelExit.transform.position += new Vector3(6f, 0f, 0f);
                    }
                    else
                    {
                        levelExit.transform.position += new Vector3(4f, 0f, 0f);
                    }
                }
            }
            if(ghostEffect != null)
            {
                ghostEffect.Deactivate = true;
            }

       
            UIController.instance.bossHealthSlider.gameObject.SetActive(false);
            levelExit.SetActive(true);
            healthDrop.SetActive(true);
           
        }
        else
        {
            if (!animFin)
            {
                if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
                {
                    if (ChargingEffect.instance != null)
                    {
                        if (ChargingEffect.instance.isEnemy)
                        {
                            Destroy(ChargingEffect.instance.gameObject);
                        }
                    }

                    currentSequence++;
                    actions = sequences[currentSequence].actions;
                    currentAction = 0;
                    actionCounter = Random.Range(actions[currentAction].actionLength1, actions[currentAction].actionLength2);

                    if (currentSequence == sequences.Length - 1)
                    {
                        secondPhase = true;
                        theBody.sprite = newSprite;

                        if (BossAnimationController.instance != null)
                        {
                            if (BossAnimationController.instance.Idle1 == true && theBody.sprite == newSprite)
                            {
                                BossAnimationController.instance.Idle1 = false;
                                BossAnimationController.instance.Idle2 = true;
                                BossAnimationController.instance.anim.SetBool("Idle1", false);
                            }


                        }

                    }

                }
            }

        }

        UIController.instance.bossHealthSlider.value = currentHealth;
    }

    public void ChangePhase()
    {
        if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
        {
            if (ChargingEffect.instance != null)
            {
                if (ChargingEffect.instance.isEnemy)
                {
                    Destroy(ChargingEffect.instance.gameObject);
                }
            }

            currentSequence++;
            actions = sequences[currentSequence].actions;
            currentAction = 0;
            actionCounter = Random.Range(actions[currentAction].actionLength1, actions[currentAction].actionLength2);

            if (currentSequence == sequences.Length - 1)
            {
                secondPhase = true;
                theBody.sprite = newSprite;

                if (BossAnimationController.instance != null)
                {
                    if (BossAnimationController.instance.Idle1 == true && theBody.sprite == newSprite)
                    {
                        BossAnimationController.instance.Idle1 = false;
                        BossAnimationController.instance.Idle2 = true;
                        BossAnimationController.instance.anim.SetBool("Idle1", false);
                    }


                }

            }

        }

    }

    void ResetColor()
    {
        theBody.color = originColor;
    }

    public void CheckAtkNumber()
    {
        if(actions[currentAction].attackNum == 1)
        {
            actions[currentAction].Attack1 = true;
        }
        if (actions[currentAction].attackNum == 2)
        {
            actions[currentAction].Attack2 = true;
        }
        if (actions[currentAction].attackNum == 3)
        {
            actions[currentAction].Attack3 = true;
        }
        if (actions[currentAction].attackNum == 4)
        {
            actions[currentAction].Attack4 = true;
        }
        if (actions[currentAction].attackNum == 5)
        {
            actions[currentAction].Attack5 = true;
        }
        if (actions[currentAction].attackNum == 6)
        {
            actions[currentAction].Attack6 = true;
        }
        if (actions[currentAction].attackNum == 7)
        {
            actions[currentAction].Attack7 = true;
        }
        if (actions[currentAction].attackNum == 8)
        {
            actions[currentAction].Attack8 = true;
        }
        if (actions[currentAction].attackNum == 9)
        {
            actions[currentAction].Attack9 = true;
        }
        if (actions[currentAction].attackNum == 10)
        {
            actions[currentAction].Attack10 = true;
        }
     

    }

    public void ResetCD()
    {
        if(actions[currentAction].resetCD == 1)
        {
            actions[currentAction].resetFireTime = true;
        }
    }

    public void ReleaseCharge()
    {
        if (ChargingEffect.instance != null)
        {
            ChargingEffect.instance.CreateBlast();
        }
    }

    public void Teleport()
    {
        if (actions[currentAction].teleportToPlayer)
        {
            var playerSpot = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, 0f);
            transform.position = playerSpot += new Vector3(0f, 2f, 0f);
            stopTeleport = false;
       
         
        }
        else
        {
            transform.position = actions[currentAction].pointToMove.position;
        }
    }

    public void MoveAgain()
    {
        canMove = true;
    }

    public void DamagePop(int number)
    {
        if (number != 0)
        {
            var randomPos = Random.insideUnitSphere * 0.5f;
            var damagePop = Instantiate(PlayerController.instance.damageUI, transform.position + randomPos, Quaternion.Euler(0f, 0f, 0f));
            damagePop.GetComponent<DamageUI>().damageNumber.text = number.ToString();
        }
    }

    public void DamagePop2(int number)
    {
        if (number != 0)
        {
            var randomPos = Random.insideUnitSphere * 0.5f;
            var damagePop = Instantiate(PlayerController.instance.damageUI, transform.position + randomPos, Quaternion.Euler(0f, 0f, 0f));
            damagePop.GetComponent<DamageUI>().critNumber.text = number.ToString();
        }
    }

    public void EndStartAnimation()
    {
        endStartAnim = true;
        theBody.enabled = true;
        theBody.sortingLayerName = "Player";
        theBody.sortingOrder = 2;

        if (theShadow != null)
        {
            theShadow.enabled = true;
        }

        rangeFromPlayer = 100;

        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().offset = new Vector2(0f, 0f);
    }

 
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength1;
    public float actionLength2;
    public int attackNum;
    public int resetCD;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public bool moveToPoints;
    public bool teleportToPoint, teleportToPlayer;
    public Transform pointToMove;

    public bool shouldShoot, shouldShootPlayer;
    public GameObject itemToShoot, itemToShootPlayer;
    public float timeBetweenShots;
    public bool resetFireTime, resetShotCount, noGhost;
    public Transform[] shotPoints;
    public Transform mainShotPoint;
    public GameObject[] itemsToActive;
    public bool phaseSwitch;
    public bool noTransition;
    public bool skipAction;

    public bool Attack1, Attack2, Attack3, Attack4, Attack5, Attack6, Attack7, Attack8, Attack9, Attack10;


}


[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;

}
