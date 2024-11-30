using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D theRB;
    public Transform gunArm;

    private Camera theCam;
    private CameraController camera;

    public Animator anim;

    public float fireRate, playerCritRate, playerDamage, gunCrit1, gunCrit2, critDmg1, critDmg2;
    public int playerHealthBonus;
    

    public SpriteRenderer theBody, theShadow;
    public GameObject redScreen, noBullet, noBomb;
    [SerializeField]
    public bool outOfBomb;

    public float activeMoveSpeed, buffDuration; //bulletHealth

    public int[] Ammo;
    public int ammo1, ammo2, bomb1;
    
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvinc = .5f;

    [Header("Ultimate")]
    public float ultCooldown;
    public float ultLength;

    [HideInInspector]
    public float dashCoolCounter, initCooldown;
    [HideInInspector]
    public float dashCounter, ultCounter, ultCoolCounter;
    [HideInInspector]
    public float oldPos;


    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    //[HideInInspector]
    public int currentGun, nextGun;
    public bool isDual;
    [HideInInspector]
    public bool isShooting, isKnockedBack;

    public bool isBunny, isMole, isFox, isRaccoon, isCpt, isUltActive;

    public GameObject AOEDmg, playerEffects, dizzyEffect, Grenade, grenadePin, ultAura, Shockwave, groundCrack, heldGuns;
   
    [Header("Mole")]
    public GameObject earthquakeEffect;
    public float spawnSeconds;
    public float resetSeconds;
    [Header("Raccoon")]
    public GameObject[] Decoy; 
    public GameObject[] spawnObjects;
    public GameObject[] spawnObjects2;
    [HideInInspector]
    public bool longerDecoy;

    public Material flashEffect;
    public Sprite deathSprite, ogSprite;
    
    public Material currentMaterial;

    public Camera mainCam;

    private GameObject nearestEnemy = null;
    private GameObject[] bosses = null;
    private GameObject[] enemies = null;

    public bool useSkill, changeGun, changeDefaultGun, buyItem, isAutoAim;

    public int gunCounter = 0, gunsHeld;
    public GhostEffect trailEffect;
   

    //[HideInInspector]
    public int reviveCount = 2;
 
    [HideInInspector]
    public bool gunDisabled, confirmBuy, confirmAdBuy, expBuff, partsBuff;

    public GameObject damageUI, lvlUpText, expText;
    public Light playerLight;

    private GameObject ultCrack, moleUltCrack;

    public double playTime;

    [Header("Audio")]
    public GameObject gunSwitch;
    public GameObject sound1;

    [SerializeField]
    public bool canSpawnEffect;

    [Header("Dust Effect")]
    public GameObject dustEffect;
    public Transform dustPosition;

    [Header("Buff Popup")]
    public GameObject buff1;
    public GameObject buff2;

    [Header("Add On Buffs")]
    public GameObject drone1;
    public GameObject iceStar;

    public bool faceRight;

    private bool buffed;

    [Header("Gamepad")]
    public bool isGamepad;
    public float inputDeadZone;
    public PlayerInput input = null;

    [SerializeField]
    public Vector2 moveVector = Vector2.zero;
    [SerializeField]
    public Vector2 aimVector = Vector2.zero;

    //[HideInInspector]
    public bool dontSaveLvl;
   

    //public Collider2D isColliding;
    //existing before start
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        input = new PlayerInput();

    }
    //Gamepad
    #region Gamepad

    private void OnEnable()
    {

        input.Enable();
        input.Player.Move.performed += OnMovePerform;
        input.Player.Move.canceled += OnMoveCancel;

        input.Player.Fire.performed += OnFire;
        input.Player.Fire.canceled += OnFireCancel;

        input.Player.Look.performed += OnAimPerform;
        input.Player.Look.canceled += OnAimCancel;

        input.Player.Dash.performed += OnDash;
        input.Player.Dash.canceled += OnDashCancel;

        input.Player.Switch.performed += OnSwitch;

        input.Player.SwitchDefault.performed += OnSwitchDefault;

        input.Player.Interact.performed += OnInteract;

        input.Player.Grenade.performed += OnBomb;

        input.Player.Ultimate.performed += OnUltimate;

        input.Player.Pause.performed += OnPause;

    }
    private void OnDisable()
    {

        input.Disable();
        input.Player.Move.performed -= OnMovePerform;
        input.Player.Move.canceled -= OnMoveCancel;

        input.Player.Fire.performed -= OnFire;
        input.Player.Fire.canceled -= OnFireCancel;

        input.Player.Look.performed -= OnAimPerform;
        input.Player.Look.canceled -= OnAimCancel;

        input.Player.Dash.performed -= OnDash;
        input.Player.Dash.canceled -= OnDashCancel;

        input.Player.Switch.performed -= OnSwitch;

        input.Player.SwitchDefault.performed -= OnSwitchDefault;

        input.Player.Interact.performed -= OnInteract;

        input.Player.Grenade.performed -= OnBomb;

        input.Player.Ultimate.performed -= OnUltimate;

        input.Player.Pause.performed -= OnPause;

    }
    private void OnMovePerform(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }
    private void OnAimPerform(InputAction.CallbackContext value)
    {
        aimVector = value.ReadValue<Vector2>();
    }
    private void OnMoveCancel(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
    private void OnAimCancel(InputAction.CallbackContext value)
    {
        aimVector = Vector2.zero;
    }
    private void OnFire(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (!LevelManager.instance.isCamp)
            {
                FireButton.instance.pressed = true;
            }
        }
    }
    private void OnFireCancel(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            FireButton.instance.pressed = false;
        }
    }
    private void OnDash(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (!LevelManager.instance.isCamp)
            {
                useSkill = true;
            }
        }
    }
    private void OnDashCancel(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {

        }
    }
    private void OnSwitch(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (UIController.instance.switchGunButton.GetComponent<Button>().enabled)
            {
                changeGun = true;
            }
        }
    }
    private void OnSwitchCancel(InputAction.CallbackContext obj)
    {
      
    }
    private void OnSwitchDefault(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (UIController.instance.defaultButton.GetComponent<Button>().enabled)
            {
                changeDefaultGun = true;
            }
        }
    }
    private void OnSwitchDefaultCancel(InputAction.CallbackContext obj)
    {
       
    }
    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (UIController.instance.interactButton.GetComponent<Button>().enabled && UIController.instance.interactButton.GetComponent<Button>().image.enabled)
            {
                UIController.instance.interactButton.GetComponent<PickupButton>().Interact();
            }
        }
    }
    private void OnInteractCancel(InputAction.CallbackContext obj)
    {
      
    }
    private void OnBomb(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (UIController.instance.grenadeButton.GetComponent<Button>().enabled)
            {
                UIController.instance.throwGrenade();
            }
        }
    }
    private void OnBombCancel(InputAction.CallbackContext obj)
    {
     
    }
    private void OnUltimate(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (UIController.instance.ultButton.GetComponent<Button>().enabled && UIController.instance.ultButton.GetComponent<Button>().interactable)
            {
                UIController.instance.activeUlt();
            }
        }
    }
    private void OnUltimateCancel(InputAction.CallbackContext obj)
    {

    }
    private void OnPause(InputAction.CallbackContext obj)
    {
        if (isGamepad)
        {
            if (LevelManager.instance.isCamp)
            {
                if (!UIController.instance.isPaused)
                {
                    if (UIController.instance.pauseButton.activeInHierarchy)
                    {
                        UIController.instance.Pause();
                    }
                }
                else if (!UIController.instance.chooseTypeWindow.activeInHierarchy &&
                    !GoogleSaveManager.instance.cover.activeInHierarchy && !GoogleSaveManager.instance.cloudNotice.activeInHierarchy
                    && !GoogleSaveManager.instance.notLoggedIn.activeInHierarchy)
                {
                    UIController.instance.Resume();
                }
            }
            else
            {
                if (!UIController.instance.isPaused)
                {
                    if (UIController.instance.pauseButton.activeInHierarchy)
                    {
                        UIController.instance.Pause();
                    }
                }
                else if (!UIController.instance.chooseTypeWindow.activeInHierarchy)
                {
                    UIController.instance.Resume();
                }
            }
        
        }
    }
    private void OnPauseCancel(InputAction.CallbackContext obj)
    {
       
    }

    #endregion
    //End of Gamepad

    // Start is called before the first frame update
    void Start()
    {
        dontSaveLvl = false;
        canSpawnEffect = true;
        isShooting = true;

        theCam = Camera.main;
        InvokeRepeating("OldPosition", 0.2f, 0.2f);

        activeMoveSpeed = moveSpeed;

        //isColliding = GetComponent<Collider2D>();

        UIController.instance.ammo1.text = "x" + Ammo[currentGun].ToString();
       //UIController.instance.ammo2.text = "x" + ammo2.ToString();
        UIController.instance.bomb1.text = "x" + bomb1.ToString();

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        //UIController.instance.nextGun.sprite = availableGuns[nextGun].gunUI;

        useSkill = false;

        initCooldown = dashCooldown;

        if (LevelManager.instance.isChangeLight)
        {
            playerLight.intensity = 1.5f;
        }
        else
        {
            playerLight.intensity = 2.5f;
        }

        if (LevelManager.instance.isCamp)
        {
            playTime = 0;
        }

        faceRight = true;
        buffed = false;

        StartCoroutine(RuneBuff());

    }

    public void RoundCrits()
    {
        if (Convert.ToString(gunCrit1).Contains("0.09999"))
        {
            gunCrit1 = 0.1f;
        }

        if (Convert.ToString(gunCrit2).Contains("0.09999"))
        {
            gunCrit2 = 0.1f;
        }

        if (Convert.ToString(playerDamage).Contains("0.09999"))
        {
            playerDamage = 0.1f;
        }
    }

    private IEnumerator RuneBuff()
    {
        yield return new WaitForSeconds(0.3f);

        RuneController.instance.ApplyPlayer();

        RoundCrits();
    }

    private void OldPosition()
    {
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.isCutscene)
        {
            if (UIController.instance.gamepadOn)
            {
                input.Player.Move.Enable();
            }
            else if (!UIController.instance.gamepadOn)
            {
                input.Player.Disable();
            }
        }

        if (Gamepad.all.Count > 0 && !Gamepad.current.displayName.Contains("uinput") && !Gamepad.current.description.product.Contains("uinput"))
        {
            
            isGamepad = true;
            UIController.instance.DisableJoystick1();

            /*  if (!LevelManager.instance.isCutscene && SceneManager.GetActiveScene().name != "Tutorial")
              {
                  isGamepad = true;
                  UIController.instance.DisableJoystick1();
              }
              else
              {
                  isGamepad = false;
              }*/
        }
        else
        {
            isGamepad = false;

            if (LevelManager.instance.isCamp)
            {
                if (LevelManager.instance.isCutscene)
                {
                    if (canMove)
                    {
                        UIController.instance.EnableJoystickCutscene();
                    }
                }
                else
                {
                    if (!UIController.instance.isPaused && !UIController.instance.selectScreen.activeInHierarchy && canMove && !UIController.instance.quitting)
                    {
                        if (!UIController.instance.joyStick.GetComponent<Image>().raycastTarget)
                        {
                            UIController.instance.EnableJoystick1();
                        }
                    }
                    else
                    {
                        if (UIController.instance.joyStick.GetComponent<Image>().raycastTarget)
                        {
                            UIController.instance.EnableJoystick1();
                        }
                    }
                }

              
            }
            else
            {
                if (!UIController.instance.isPaused && canMove && !UIController.instance.quitting)
                {
                    if (!UIController.instance.joyStick.GetComponent<Image>().raycastTarget)
                    {
                        UIController.instance.EnableJoystick1();
                    }
                }
                else
                {
                    if (UIController.instance.joyStick.GetComponent<Image>().raycastTarget)
                    {
                        UIController.instance.EnableJoystick1();
                    }
                }
            }
        }

        if (ArtifactManager.instance != null && !UIController.instance.isPaused)
        {
            if (ArtifactManager.instance.hasDrone)
            {
                if (PlayerHealth.instance.currentHealth > 0)
                {
                    drone1.SetActive(true);
                }
                else if (PlayerHealth.instance.currentHealth <= 0)
                {
                    drone1.SetActive(false);
                }
            }
            else if (!ArtifactManager.instance.hasDrone)
            {
                drone1.SetActive(false);
            }

            if (ArtifactManager.instance.hasIceStar)
            {
                if (PlayerHealth.instance.currentHealth > 0)
                {
                    iceStar.SetActive(true);
                }
                else if (PlayerHealth.instance.currentHealth <= 0)
                {
                    iceStar.SetActive(false);
                }
            }
            else if (!ArtifactManager.instance.hasIceStar)
            {
                iceStar.SetActive(false);
            }

        }

        if (canMove && !UIController.instance.isPaused)
        {
            if (!LevelManager.instance.isCamp)
            {
                playTime += Time.deltaTime;
            }

          
            //*********for PC*********

            /* moveInput.x = Input.GetAxisRaw("Horizontal");
             moveInput.y = Input.GetAxisRaw("Vertical");

             moveInput.Normalize();

             theRB.velocity = moveInput * activeMoveSpeed;

             Vector3 mousePos = Input.mousePosition;
            // Vector3 mousePos = new Vector3(UIController.instance.joyStick.Direction.x, 0.0f, UIController.instance.joyStick.Direction.y);
             Vector3 screenPoint = theCam.WorldToScreenPoint(transform.localPosition);


             //character facing direction
             if (mousePos.x < screenPoint.x)
             {

                 transform.localScale = new Vector3(-1f, 1f, 1f);
                 gunArm.localScale = new Vector3(-1f, -1f, 1f);

                 //gunArm2.localScale = new Vector3(-1f, -1f, 1f);


             }
             else if (mousePos.x > screenPoint.x)
             {

                 transform.localScale = Vector3.one;
                 gunArm.localScale = Vector3.one;
                 // gunArm2.localScale = Vector3.one;

             }

             //rotate gun arm
             Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
             float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
             gunArm.rotation = Quaternion.Euler(0, 0, angle);*//*

             /****************/

            //for phone

            if (!isGamepad)
            {
                moveInput.x = UIController.instance.joyStick.Horizontal;
                moveInput.y = UIController.instance.joyStick.Vertical;
            }

            if (isGamepad)
            {

                moveInput.x = moveVector.x;
                moveInput.y = moveVector.y;

            }
        
          
            Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
            movementDirection.Normalize();


            //maintain consistent speed when moving diagonally

            moveInput.Normalize();


            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);

            theRB.velocity = moveInput * activeMoveSpeed;

            if (isBunny)
            {
                if (TraitManager.instance != null)
                {
                    if (TraitManager.instance.Bunny[3] == 1)
                    {
                        if (theRB.velocity != new Vector2(0, 0))
                        {
                            if (!buffed)
                            {
                                playerDamage += 0.1f;
                                buffed = true;
                            }

                        }
                        else
                        {
                            if (buffed)
                            {
                                playerDamage -= 0.1f;
                                buffed = false;
                            }

                        }
                    }
                }
            }


            //character facing direction


            //rotate gun arm
            float angle = 0f;

            if (!isGamepad)
            {
                angle = Mathf.Atan2(UIController.instance.joyStick2.Direction.y, UIController.instance.joyStick2.Direction.x) * Mathf.Rad2Deg;

                if (UIController.instance.joyStick2.Direction.x < 0f)
                {

                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    gunArm.localScale = new Vector3(-1f, -1f, 1f);
                    drone1.GetComponentInChildren<ParticleSystem>().gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);

                    if (trailEffect != null)
                    {
                        trailEffect.isTurned = true;
                    }

                    faceRight = false;

                }
                else if (UIController.instance.joyStick2.Direction.x > 0f)
                {

                    transform.localScale = Vector3.one;
                    gunArm.localScale = Vector3.one;
                    drone1.GetComponentInChildren<ParticleSystem>().gameObject.transform.localScale = Vector3.one;

                    if (trailEffect != null)
                    {
                        trailEffect.isTurned = false;
                    }

                    faceRight = true;
                }


                if (angle != 0f)
                {

                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                }
            }
            if (isGamepad)
            {
                //aimVector.Normalize();

                if (Mathf.Abs(aimVector.x) > 0.5f || Mathf.Abs(aimVector.y) > 0.5f)
                {
      
                    angle = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;

                    if (aimVector.x < 0f)
                    {

                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, -1f, 1f);
                        drone1.GetComponentInChildren<ParticleSystem>().gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);

                        if (trailEffect != null)
                        {
                            trailEffect.isTurned = true;
                        }

                        faceRight = false;

                    }
                    else if (aimVector.x > 0f)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;
                        drone1.GetComponentInChildren<ParticleSystem>().gameObject.transform.localScale = Vector3.one;

                        if (trailEffect != null)
                        {
                            trailEffect.isTurned = false;
                        }

                        faceRight = true;

                        if(gunArm.rotation.z < 1f)
                        {
                            gunArm.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        }
                    }

                    if (angle != 0)
                    {

                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    }
                }
            }

            //****auto aim system*****//

            if (isAutoAim == true)
            {
                gunArm.gameObject.SetActive(true);
                //enemy + boss autoaim

                bosses = GameObject.FindGameObjectsWithTag("Boss");
                enemies = GameObject.FindGameObjectsWithTag("Enemy");

                float bestAngle = -1f;

                var distance = float.MaxValue;

                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, enemy.transform.position);
                        nearestEnemy = enemy;
                        Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                        vectorToEnemy.Normalize();
                        float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;
                        /*
                                                Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                                                vectorToEnemy.Normalize();
                                                //var distance = Vector3.Distance(transform.position, enemy.transform.position);
                                                //float angleToEnemy = Vector3.Dot(transform.forward, vectorToEnemy);
                                                float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;*/


                        if (Gun.instance.isPenetrate)
                        {
                            if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= 5f)
                            {

                                nearestEnemy = enemy;
                                bestAngle = angleToEnemy;


                                if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                }
                                else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));


                                }
                            }
                            else if (nearestEnemy == null)
                            {

                                /*if (moveInput.x < 0f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                    //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                                }
                                else if (moveInput.x > 0f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    // gunArm2.localScale = Vector3.one;


                                }*/
                                if (UIController.instance.joyStick2.Direction.x < 0f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                }
                                else if (UIController.instance.joyStick2.Direction.x > 0f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;

                                }
                                if (angle != 0)
                                {

                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                                }
                            }
                        }

                        if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= 5f)
                        {
                            nearestEnemy = enemy;
                            bestAngle = angleToEnemy;



                            if (vectorToEnemy.x < 0f)//&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));


                            }
                            else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));


                            }
                        }


                        else if (nearestEnemy == null)
                        {

                            /*  if (moveInput.x < 0f)
                              {

                                  transform.localScale = new Vector3(-1f, 1f, 1f);
                                  gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                  //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                              }
                              else if (moveInput.x > 0f)
                              {

                                  transform.localScale = Vector3.one;
                                  gunArm.localScale = Vector3.one;
                                  // gunArm2.localScale = Vector3.one;


                              }*/
                            if (UIController.instance.joyStick2.Direction.x < 0f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);

                            }
                            else if (UIController.instance.joyStick2.Direction.x > 0f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;


                            }
                            if (angle != 0)
                            {

                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                            }
                        }
                    }
                }
                foreach (GameObject boss in bosses)
                {

                    if (Vector3.Distance(transform.position, boss.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, boss.transform.position);
                        nearestEnemy = boss;
                        Vector3 vectorToEnemy = boss.transform.position - transform.position;
                        vectorToEnemy.Normalize();
                        float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;

                        if (Gun.instance.isPenetrate)
                        {
                            if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= 5f)
                            {

                                nearestEnemy = boss;
                                bestAngle = angleToEnemy;


                                if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, boss.transform.position) <= 10f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                }
                                else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, boss.transform.position) <= 10f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));


                                }
                            }
                            else if (nearestEnemy == null)
                            {

                                /*if (moveInput.x < 0f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                    //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                                }
                                else if (moveInput.x > 0f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    // gunArm2.localScale = Vector3.one;


                                }*/
                                if (UIController.instance.joyStick2.Direction.x < 0f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                }
                                else if (UIController.instance.joyStick2.Direction.x > 0f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;


                                }
                                if (angle != 0)
                                {

                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                                }
                            }
                        }
                        if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= 5f)
                        {

                            nearestEnemy = boss;
                            bestAngle = angleToEnemy;


                            if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, boss.transform.position) <= 7f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));


                            }
                            else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, boss.transform.position) <= 7f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));



                            }


                        }

                        else if (nearestEnemy == null)
                        {

                            /*if (moveInput.x < 0f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                            }
                            else if (moveInput.x > 0f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                // gunArm2.localScale = Vector3.one;


                            }*/
                            if (UIController.instance.joyStick2.Direction.x < 0f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);

                                //gunArm2.localScale = new Vector3(-1f, -1f, 1f);


                            }
                            else if (UIController.instance.joyStick2.Direction.x > 0f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                // gunArm2.localScale = Vector3.one;

                            }
                            if (angle != 0)
                            {

                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                            }
                        }
                    }
                }

                if ((nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 5f &&
                    Gun.instance.isPenetrate == false && bosses.Length == 0) ||
                    (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 5f
                    && Gun.instance.isPenetrate == true && bosses.Length == 0) ||
                    (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 5f &&
                    Gun.instance.isPenetrate == false && bosses.Length > 0) ||
                    (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 5f
                    && Gun.instance.isPenetrate == true && bosses.Length > 0))
                {
                    /*if (moveInput.x < 0f)
                    {

                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, -1f, 1f);

                        //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                    }
                    else if (moveInput.x > 0f)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;
                        // gunArm2.localScale = Vector3.one;


                    }*/
                    if (UIController.instance.joyStick2.Direction.x < 0f)
                    {

                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, -1f, 1f);

                        //gunArm2.localScale = new Vector3(-1f, -1f, 1f);


                    }
                    else if (UIController.instance.joyStick2.Direction.x > 0f)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;
                        // gunArm2.localScale = Vector3.one;

                    }
                    if (angle != 0)
                    {

                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    }
                }
                /*  if (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 8f
                    && Gun.instance.isPenetrate == true && bosses.Length == 0)
                  {
                      nearestEnemy = null;
                  }
                  if (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 7f &&
                     Gun.instance.isPenetrate == false && bosses.Length > 0)
                  {
                      nearestEnemy = null;
                  }
                  if (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > 10f
                      && Gun.instance.isPenetrate == true && bosses.Length > 0)
                  {
                      nearestEnemy = null;
                  }*/

                if (enemies.Length == 0 && bosses.Length == 0)
                {

                    /*if (moveInput.x < 0f)
                    {

                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, -1f, 1f);

                        //gunArm2.localScale = new Vector3(-1f, -1f, 1f);

                    }
                    else if (moveInput.x > 0f)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;
                        // gunArm2.localScale = Vector3.one;


                    }*/
                    if (UIController.instance.joyStick2.Direction.x < 0f)
                    {

                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, -1f, 1f);

                        //gunArm2.localScale = new Vector3(-1f, -1f, 1f);


                    }
                    else if (UIController.instance.joyStick2.Direction.x > 0f)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;
                        // gunArm2.localScale = Vector3.one;

                    }
                    if (angle != 0)
                    {

                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    }
                }

            }

            //end of AutoAim

            if (isDual)
            {
                if (availableGuns[currentGun].gun2 != null)
                {
                    availableGuns[currentGun].gun2.gameObject.SetActive(true);
                }
                //activate second ammo > if it runs out, setactive false/isdual false
            }
            else if (!isDual)
            {
                if (availableGuns[currentGun].gun2 != null)
                {
                    availableGuns[currentGun].gun2.gameObject.SetActive(false);
                }

            }

            if (changeGun == true)//Input.GetKeyDown(KeyCode.Tab))
            {

                //FixedJoystick.instance.tapped = false;
                //gunCounter = 0;
                if (currentGun == 0 && availableGuns.Count > 1)
                {
                    
                    currentGun = 1;

                    if(availableGuns.Count > 2)
                    {
                        nextGun = currentGun + 1;
                    }
                    else
                    {
                        nextGun = 1;
                    }
                   
                    SwitchGun();

                    changeGun = false;

                }

                else if (availableGuns.Count > 2)
                {

                    currentGun++;

                    nextGun = currentGun + 1;

                    if(nextGun >= availableGuns.Count)
                    {
                        nextGun = 1;
                    }

                    if (currentGun >= availableGuns.Count)
                    {
                        currentGun = 1;
                        nextGun = currentGun + 1;
                    }

                    SwitchGun();

                    changeGun = false;
                }
                else
                {
                    changeGun = false;
                }
            }
            if (changeDefaultGun == true)
            {
                if (currentGun != 0 && availableGuns.Count > 0)
                {

                    nextGun = 1;
                    currentGun = 0;
                    

                    SwitchGun();

                   
                }
                else if (currentGun == 0)
                {
                    if (Gun.instance.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name.Contains("Pistol"))
                    {
                        Gun.instance.anim.Play("Pistol_Recoil(2)", -1, 0f);
                    }


                    changeDefaultGun = false;
                }
            }

            //Skill

            if (isUltActive == true && ultCounter <= 0 && ultCoolCounter <= 0)
            {
                Instantiate(Shockwave, transform.position, transform.rotation);

                if (transform.localScale.x == -1)
                {
                    Instantiate(groundCrack, new Vector3(transform.position.x - 0.35f, transform.position.y, 0f), transform.rotation);
                }
                else
                {
                    Instantiate(groundCrack, transform.position, transform.rotation);
                }
                
                ultAura.SetActive(true);

                if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < 0.5f)
                {
                    PlayerHealth.instance.invincCount = 0.5f;
                }

                ultCounter = ultLength;

                if (isBunny)
                {
                    buffMsg();
                    activeMoveSpeed = moveSpeed + 2;
                    if (TraitManager.instance.Bunny[4] == 1)
                    {
                        playerCritRate = playerCritRate + 0.20f;
                    }
                    else
                    {
                        playerCritRate = playerCritRate + 0.15f;
                    }

                    if (TraitManager.instance.Bunny[5] == 1)
                    {
                        dashCooldown = 0.5f;
                    }
                    else
                    {
                        dashCooldown = 0.2f;
                    }

                }
                if(isRaccoon)
                {
                    int random = UnityEngine.Random.Range(0, spawnObjects.Length);
                    Instantiate(spawnObjects[random], theBody.transform.position, theBody.transform.rotation);

                    if(TraitManager.instance.Raccoon[5] == 1)
                    {
                        int random2 = UnityEngine.Random.Range(0, spawnObjects2.Length);
                        Instantiate(spawnObjects2[random2], theBody.transform.position, theBody.transform.rotation);
                    }
                }

            }
            if (ultCounter > 0)
            {
                UIController.instance.ultButton.GetComponent<Button>().interactable = false;
                UIController.instance.ultButtonGlow.SetActive(false);
                ultCounter -= Time.deltaTime;

                if (isMole)
                {

                    if (spawnSeconds > 0f)
                    {
                        spawnSeconds -= Time.deltaTime;
                    }
                    else
                    {
                        StartCoroutine(Earthquake());
                        spawnSeconds = resetSeconds;

                    }
                }


                if (ultCounter <= 0)
                {

                    ultCoolCounter = ultCooldown;
                    UIController.instance.ultTimer.gameObject.SetActive(false);
                    UIController.instance.Ult.gameObject.SetActive(false);

                    if (UIController.instance.dualGun.isActiveAndEnabled)
                    {
                        UIController.instance.dualGun.gameObject.transform.position = TimerController.instance.oldPosition;
                        UIController.instance.buffTimer.gameObject.transform.position = TimerController.instance.oldPosition2;
                    }
                    if (isBunny || isRaccoon)
                    {
                        EndUlt();
                    }
                    if (isMole)
                    {
                        EndUlt();
                        StopCoroutine(Earthquake());
                        spawnSeconds = resetSeconds;
                    }

                }
            }
            if (ultCoolCounter > 0)
            {
                ultCoolCounter -= Time.deltaTime;
                UIController.instance.ultButton.GetComponent<Button>().interactable = false;
                UIController.instance.ultCooldown.gameObject.SetActive(true);
                UIController.instance.ultCooldown.text = Mathf.RoundToInt(ultCoolCounter).ToString();
                UIController.instance.ultButtonGlow.SetActive(false);


                if (ultCoolCounter <= 0)
                {
                    UIController.instance.ultButton.GetComponent<Button>().interactable = true;
                    UIController.instance.ultButtonGlow.SetActive(true);
                    UIController.instance.ultCooldown.gameObject.SetActive(false);
                }
            }

            if (useSkill == true)//Input.GetKeyDown(KeyCode.Space))
            {
                if (!isGamepad)
                {
                    isShooting = true;
                    useSkill = false;
                    SkillButton.instance.enabled = false;
                    //if (mousePos.x > screenPoint.x)
                    if (UIController.instance.joyStick2.Direction.x >= 0f)
                    {

                        if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);

                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash4");
                                }
                                else
                                {
                                    anim.SetTrigger("dash2");
                                }


                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }

                                    //gunArm.gameObject.SetActive(false);
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");


                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }
                                if (TraitManager.instance.Raccoon[3] == 1)
                                {

                                    Instantiate(Decoy[2], transform.position, transform.rotation);

                                }
                                else
                                {
                                    Instantiate(Decoy[0], transform.position, transform.rotation);
                                }
                            }


                        }
                        else if (dashCoolCounter <= 0 && dashCounter <= 0)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash3");
                                }
                                else
                                {
                                    anim.SetTrigger("dash");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }
                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[2], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[0], transform.position, transform.rotation);
                                }
                            }
                        }

                    }
                    //else if (mousePos.x < screenPoint.x)
                    else if (UIController.instance.joyStick2.Direction.x < 0f)
                    {

                        if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                        {

                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash3");

                                }
                                else
                                {
                                    anim.SetTrigger("dash");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }

                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[3], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[1], transform.position, transform.rotation);
                                }

                            }



                        }
                        else if (dashCoolCounter <= 0 && dashCounter <= 0)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash4");
                                }
                                else
                                {
                                    anim.SetTrigger("dash2");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;
                            }

                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }

                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[3], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[1], transform.position, transform.rotation);
                                }

                            }


                        }
                    }
                }
                if (isGamepad)
                {
                    isShooting = true;
                    useSkill = false;
                    SkillButton.instance.enabled = false;
                    //if (mousePos.x > screenPoint.x)
                    if (aimVector.x >= 0f)
                    {

                        if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);

                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash4");
                                }
                                else
                                {
                                    anim.SetTrigger("dash2");
                                }


                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }

                                    //gunArm.gameObject.SetActive(false);
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");


                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }
                                if (TraitManager.instance.Raccoon[3] == 1)
                                {

                                    Instantiate(Decoy[2], transform.position, transform.rotation);

                                }
                                else
                                {
                                    Instantiate(Decoy[0], transform.position, transform.rotation);
                                }
                            }


                        }
                        else if (dashCoolCounter <= 0 && dashCounter <= 0)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived &&   PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash3");
                                }
                                else
                                {
                                    anim.SetTrigger("dash");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }
                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[2], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[0], transform.position, transform.rotation);
                                }
                            }
                        }

                    }
                    //else if (mousePos.x < screenPoint.x)
                    else if (aimVector.x < 0f)
                    {

                        if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                        {

                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash3");

                                }
                                else
                                {
                                    anim.SetTrigger("dash");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;

                            }
                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }

                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[3], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[1], transform.position, transform.rotation);
                                }

                            }



                        }
                        else if (dashCoolCounter <= 0 && dashCounter <= 0)
                        {
                            activeMoveSpeed = dashSpeed;
                            dashCounter = dashLength;
                            //PlayerHealth.instance.invincCount = 1f;
                            if (!PlayerHealth.instance.justRevived && PlayerHealth.instance.invincCount < dashInvinc)
                            {
                                PlayerHealth.instance.MakeInvincible(dashInvinc);
                            }

                            Physics2D.IgnoreLayerCollision(6, 8);
                            Physics2D.IgnoreLayerCollision(6, 9);
                            Physics2D.IgnoreLayerCollision(6, 16);
                            Physics2D.IgnoreLayerCollision(6, 15);
                            Physics2D.IgnoreLayerCollision(6, 17);
                            if (isBunny)
                            {

                                if (TraitManager.instance.Bunny[5] == 1)
                                {
                                    anim.SetTrigger("dash4");
                                }
                                else
                                {
                                    anim.SetTrigger("dash2");
                                }
                            }
                            if (isMole)
                            {
                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);

                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }


                                anim.SetTrigger("dash");

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);

                                theBody.sortingLayerName = "Objects";
                                theBody.sortingOrder = -2;
                                theShadow.sortingLayerName = "Objects";
                                theShadow.sortingOrder = -2;
                            }

                            if (isRaccoon)
                            {

                                isShooting = true;
                                UIController.instance.switchGunButton.GetComponent<Button>().enabled = false;
                                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                                UIController.instance.defaultButton.GetComponent<Button>().enabled = false;
                                UIController.instance.interactButton.GetComponent<Button>().enabled = false;

                                if (availableGuns[currentGun].isCharged)
                                {
                                    gunDisabled = true;
                                    StopCoroutine(availableGuns[currentGun].ReleaseCharge());
                                    availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    availableGuns[currentGun].anim.SetBool("isFiring", false);


                                    if (availableGuns[currentGun].timeBetweenShots > 2)
                                    {
                                        availableGuns[currentGun].anim.SetBool("isFiring2", false);
                                        availableGuns[currentGun].shotCounter = 0;
                                    }

                                    Invoke("DeactivateGun", 1f);
                                }
                                else
                                {
                                    availableGuns[currentGun].GetComponent<Gun>().enabled = false;
                                    if (availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
                                    {
                                        availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
                                    }
                                    else
                                    {
                                        availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                                    }

                                    if (availableGuns[currentGun].subBody != null)
                                    {
                                        availableGuns[currentGun].subBody.enabled = false;
                                        availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;

                                        if (availableGuns[currentGun].subBody2 != null)
                                        {
                                            availableGuns[currentGun].subBody2.enabled = false;

                                            if (availableGuns[currentGun].subBody3 != null)
                                            {
                                                availableGuns[currentGun].subBody3.enabled = false;
                                            }
                                        }

                                    }
                                    if (availableGuns[currentGun].subGun != null)
                                    {
                                        availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = false;
                                    }
                                }

                                Physics2D.IgnoreLayerCollision(6, 3);
                                Physics2D.IgnoreLayerCollision(6, 20);
                                anim.SetTrigger("dash");

                                if (DecoyController.instance != null)
                                {
                                    DecoyController.instance.Destroy();

                                }

                                if (TraitManager.instance.Raccoon[3] == 1)
                                {
                                    Instantiate(Decoy[3], transform.position, transform.rotation);
                                }
                                else
                                {
                                    Instantiate(Decoy[1], transform.position, transform.rotation);
                                }

                            }
                        }
                    }
                }
            }
            if (dashCounter > 0)
            {

                dashCounter -= Time.deltaTime;

                UIController.instance.dashButton.GetComponent<Button>().interactable = false;

                if (dashCounter <= 0)
                {
                    if (!isMole)
                    {
                        Invoke("CollideAgain", 0.5f);

                        if (isRaccoon)
                        {
                            availableGuns[currentGun].anim.SetBool("isSpin", false);
                            availableGuns[currentGun].canShoot = true;
                            Invoke("MoveAgain", 0.5f);
                        }

                        if (isBunny)
                        {
                            if (TraitManager.instance.Bunny[3] == 1)
                            {
                                //playerDamage -= 0.2f;
                            }
                        }
                    }

                    if (isMole)
                    {
                        //canMove = false;
                        if (TraitManager.instance.Mole[3] != 1)
                        {
                            Instantiate(AOEDmg, theBody.transform.position, theBody.transform.rotation);
                            Instantiate(playerEffects, theBody.transform.position, theBody.transform.rotation);
                            PlayerHealth.instance.MakeInvincible(0.7f);
                        }
                        else if(TraitManager.instance.Mole[3] == 1)
                        {
                            if (transform.localScale.x == -1)
                            {
                                Instantiate(groundCrack, new Vector3(transform.position.x - 0.35f, transform.position.y, 0f), transform.rotation);
                            }
                            else
                            {
                                Instantiate(groundCrack, transform.position, transform.rotation);
                            }
                        
                            Instantiate(playerEffects, theBody.transform.position, theBody.transform.rotation);
                            PlayerHealth.instance.MakeInvincible(0.7f);
                        }


                        ResetMole();

                    }

                    if (isUltActive == true)
                    {
                        if (isBunny)
                        {
                            activeMoveSpeed = moveSpeed + 2;
                            
                        }
                        else
                        {
                            activeMoveSpeed = moveSpeed;
                        }
                    }
                    else
                    {
                        activeMoveSpeed = moveSpeed;
                    }


                    dashCoolCounter = dashCooldown;

                }
            }
            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
                SkillButton.instance.enabled = true;

                UIController.instance.dashTimer.gameObject.SetActive(true);
                UIController.instance.dashTimer.text = Mathf.RoundToInt(dashCoolCounter).ToString();

                if(dashCoolCounter <= 0)
                {
                    UIController.instance.dashButton.GetComponent<Button>().interactable = true;
                    UIController.instance.dashTimer.gameObject.SetActive(false);
                }
            }


            //moving animation + start timer

            if (moveInput != Vector2.zero)
            {

                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

        }

        else

        {

            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);

            if (isBunny)
            {
                if (TraitManager.instance != null)
                {
                    if (TraitManager.instance.Bunny[3] == 1)
                    {

                        if (buffed)
                        {
                            playerDamage -= 0.1f;
                            buffed = false;
                        }

                    }
                }
            }


        }


    }
    public void SwitchGun()
    {
        
        Instantiate(gunSwitch, transform.position, transform.rotation);
        gunCounter = 0;
        availableGuns[currentGun].canShoot = false;
        availableGuns[currentGun].rapidFire = false;
        //bulletHealth = availableGuns[currentGun].initBulletHealth;
        availableGuns[currentGun].anim.SetBool("isSpin", false);

        if (availableGuns[currentGun].GetComponentInChildren<PlayerBullet>() != null)
        {
            if (availableGuns[currentGun].GetComponentInChildren<PlayerBullet>().isLasting)
            {
                Destroy(availableGuns[currentGun].GetComponentInChildren<PlayerBullet>().gameObject);
            }
        }

        foreach (Gun theGun in availableGuns)
        {

            theGun.gameObject.SetActive(false);
            Gun.instance.anim.keepAnimatorStateOnDisable = true;
            availableGuns[currentGun].anim.SetBool("isSpin", false);
      

            if (isDual)
            {
                if (theGun.gun2 != null)
                {
                    theGun.gun2.gameObject.SetActive(false);
                }

            }

            UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
            UIController.instance.nextGun.sprite = availableGuns[nextGun].gunUI;

        }
       
        availableGuns[currentGun].gameObject.SetActive(true);
        availableGuns[currentGun].anim.SetBool("isSpin", false);
        //StartCoroutine("fireAgain");
        availableGuns[currentGun].canShoot = true;

        if(availableGuns[currentGun].isCharged)
        {
            availableGuns[currentGun].shotCounter = 0;
        }

        if (isDual)
        {
            if (availableGuns[currentGun].gun2 != null)
            {
                availableGuns[currentGun].gun2.gameObject.SetActive(true);
            }
        }

        AddFR();

      /*  if(isBunny && TraitManager.instance.Bunny[3] == 1)
        {
            availableGuns[currentGun].costPerShot = Mathf.RoundToInt(availableGuns[currentGun].costPerShot - (availableGuns[currentGun].costPerShot * 0.1f));
        }*/

    }

    public void ResetMole()
    {
        Invoke("CollideAgain", 0.7f);
        Invoke("MoveAgain", 0.5f);
        Physics2D.IgnoreLayerCollision(6, 3, false);
        Physics2D.IgnoreLayerCollision(6, 20, false);

        availableGuns[currentGun].anim.SetBool("isSpin", false);
        availableGuns[currentGun].canShoot = true;

        theBody.sortingLayerName = "Player";
        theBody.sortingOrder = 0;
        theShadow.sortingLayerName = "Player";
        theShadow.sortingOrder = 0;

        activeMoveSpeed = moveSpeed;
    }

    public void CollideAgain()
    {
        Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(6, 9, false);
        Physics2D.IgnoreLayerCollision(6, 16, false);
        //Physics2D.IgnoreLayerCollision(6, 12, false);
        Physics2D.IgnoreLayerCollision(6, 15, false);
        Physics2D.IgnoreLayerCollision(6, 17, false);
        Physics2D.IgnoreLayerCollision(6, 3, false);
        Physics2D.IgnoreLayerCollision(6, 20, false);

    }

    public void MoveAgain()
    {
        
        Gun.instance.anim.keepAnimatorStateOnDisable = true;
        Gun.instance.canShoot = true;
        Gun.instance.gameObject.GetComponent<Animator>().SetBool("isSpin", false);
        if (Gun.instance.canShoot == true && UIController.instance.Disabled.gameObject.activeInHierarchy)
        {
            UIController.instance.Disabled.gameObject.SetActive(false);
        }
        gunArm.gameObject.SetActive(true);

        if (isMole || isRaccoon)
        {
            if (availableGuns[currentGun].isCharged)
            {
                availableGuns[currentGun].GetComponent<Gun>().enabled = true;
                availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = true;
                Invoke("EnableGun", 1f);
            }

            if (availableGuns[currentGun].subBody != null)
            {
                availableGuns[currentGun].subBody.enabled = true;

                if (availableGuns[currentGun].subBody2 != null)
                {
                    availableGuns[currentGun].subBody2.enabled = true;

                    if (availableGuns[currentGun].subBody3 != null)
                    {
                        availableGuns[currentGun].subBody3.enabled = true;
                    }
                }

            }

            if (availableGuns[currentGun].subGun != null)
            {
                availableGuns[currentGun].subGun.GetComponent<Gun>().enabled = true;
            }

        }

        availableGuns[currentGun].GetComponent<Gun>().enabled = true;
        if(availableGuns[currentGun].GetComponent<SpriteRenderer>() != null)
        {
            availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            availableGuns[currentGun].GetComponentInChildren<SpriteRenderer>().enabled = true;
        }


        if(isMole || isRaccoon)
        {
            UIController.instance.switchGunButton.GetComponent<Button>().enabled = true;
            UIController.instance.grenadeButton.GetComponent<Button>().enabled = true;
            UIController.instance.defaultButton.GetComponent<Button>().enabled = true;
            UIController.instance.interactButton.GetComponent<Button>().enabled = true;
        }


        canMove = true;

    }

    public IEnumerator Stunned(float duration)
    {
       
        isKnockedBack = true;
        canMove = false;

        dizzyEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        isKnockedBack = false;
        canMove = true;

        dizzyEffect.SetActive(false);

    }

    /*   public IEnumerator Knockback(float knockbackDuration, float knockbackForce, Transform obj)
       {
           float timer = 0;

           while(knockbackDuration > timer)
           {
               timer += Time.deltaTime;
               Vector2 direction = (obj.transform.position - this.transform.position).normalized;
               theRB.AddForce(-direction * knockbackForce);
           }

           yield return 0;
       }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shop Item"))
        {
            other.GetComponent<ShopItem>().isInZone = true;
            PickupManager.instance.inShopZone = true;

            UIController.instance.interactButton.gameObject.SetActive(true);

        }
        if (other.gameObject.CompareTag("Pickups"))
        {
            StartCoroutine(flashColor());
        }
    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shop Item"))
        {
            other.GetComponent<ShopItem>().isInZone = false;
            PickupManager.instance.inShopZone = false;

            UIController.instance.interactButton.gameObject.SetActive(false);
        }
    }

    public IEnumerator flashColor()
    {
   
        theBody.color = new Color(245, 255, 0, 255);
     
        yield return new WaitForSeconds(0.1f);

        theBody.color = new Color(255, 255, 255, 255);

    }

    public IEnumerator fireAgain()
    {
        
        yield return new WaitForSeconds(0.5f);
        availableGuns[currentGun].canShoot = true;
    }

    public void EndUlt()
    {
        ultAura.SetActive(false);
        isUltActive = false;

        if (activeMoveSpeed != dashSpeed)
        {
            activeMoveSpeed = moveSpeed;
        }
        if (isBunny)
        {
            dashCooldown = initCooldown;
            if (TraitManager.instance.Bunny[4] == 1)
            {
                playerCritRate = playerCritRate - 0.20f;
            }
            else
            {
                playerCritRate = playerCritRate - 0.15f;
            }
        }
        if (isMole)
        {
           
        }
    }

    public IEnumerator Earthquake()
    {
        if (transform.localScale.x == -1)
        {
            Instantiate(earthquakeEffect, new Vector3(transform.position.x - 0.35f, transform.position.y, 0f), transform.rotation);
        }
        else
        {
            Instantiate(earthquakeEffect, transform.position, transform.rotation);
        }


        yield return null;
    }

    public void EnableGun()
    {
        gunDisabled = false;
    }

    public void DeactivateGun()
    {
        availableGuns[currentGun].GetComponent<Gun>().enabled = false;

        if (availableGuns[currentGun].isDual)
        {
            availableGuns[currentGun].GetComponentInChildren<Gun>().enabled = false;
        }
    }

    public void AddAmmo(int ammoNum)
    {
        Ammo[1] += ammoNum;
        Ammo[2] += ammoNum;
        Ammo[3] += ammoNum;
        Ammo[4] += ammoNum;

        if (ArtifactManager.instance.hasBandolier)
        {

            if(Ammo[1] > 1500)
            {
                Ammo[1] = 1500;
            }
            if (Ammo[2] > 1500)
            {
                Ammo[2] = 1500;
            }
            if (Ammo[3] > 1500)
            {
                Ammo[3] = 1500;
            }
            if (Ammo[4] > 1500)
            {
                Ammo[4] = 1500;
            }

        }
        else if (!ArtifactManager.instance.hasBandolier)
        {

            if (Ammo[1] > 1000)
            {
                Ammo[1] = 1000;
            }
            if (Ammo[2] > 1000)
            {
                Ammo[2] = 1000;
            }
            if (Ammo[3] > 1000)
            {
                Ammo[3] = 1000;
            }
            if (Ammo[4] > 1000)
            {
                Ammo[4] = 1000;
            }
        }

        UIController.instance.ammo1.text = "x" + Ammo[currentGun].ToString();
    }

    public void StopGunAnim()
    {
        if (availableGuns[currentGun].isCharged)
        {
            gunDisabled = true;
            StopCoroutine(availableGuns[currentGun].ReleaseCharge());
            availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = false;
            availableGuns[currentGun].anim.SetBool("isFiring", false);

            if (availableGuns[currentGun].timeBetweenShots > 2)
            {
                availableGuns[currentGun].anim.SetBool("isFiring2", false);
                availableGuns[currentGun].shotCounter = 0;
            }

            Invoke("DeactivateGun", 0.5f);
        }
    }

    public void StartGunAnim()
    {
        if (availableGuns[currentGun].isCharged)
        {
            Gun.instance.anim.keepAnimatorStateOnDisable = true;
            Gun.instance.canShoot = true;
            Gun.instance.gameObject.GetComponent<Animator>().SetBool("isSpin", false);

            availableGuns[currentGun].GetComponent<Gun>().enabled = true;
            availableGuns[currentGun].GetComponent<SpriteRenderer>().enabled = true;
            Invoke("EnableGun", 1f);
        }
    }

    public void AfterDash()
    {
        Instantiate(AOEDmg, theBody.transform.position, theBody.transform.rotation);
    }
    public void CantMove()
    {
        canMove = false;
    }
    public void CanMove()
    {
        if (UIController.instance.joyStick.GetComponent<Image>().raycastTarget)
        {
            canMove = true;
        }
    }

    public void playSound1()
    {
        Instantiate(sound1, transform.position, transform.rotation);
    }

    public void createSmoke()
    {
        var a = Instantiate(dustEffect, dustPosition.position, dustPosition.rotation);
        
        if(faceRight == true)
        {
            a.transform.localScale = new Vector3(-a.transform.localScale.x, a.transform.localScale.y, 1f);
        }
        else
        {
            Instantiate(dustEffect, dustPosition.position, dustPosition.rotation);
        }
    }
   
    public void buffMsg()
    {
        if (buff1 != null)
        {
            Instantiate(buff1, transform.position, transform.rotation);
        }

        if(buff2 != null)
        {
            Instantiate(buff2, transform.position, transform.rotation);
        }
    }

    public void AddFR()
    {
        availableGuns[currentGun].BuffFR();

        if(availableGuns[currentGun].subGun != null)
        {
            availableGuns[currentGun].subGun.GetComponent<Gun>().BuffFR();
        }
    }
}




