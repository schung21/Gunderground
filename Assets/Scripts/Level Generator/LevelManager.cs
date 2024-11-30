using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    public GameObject[] raidMobs, raidHordes, raidElites, defenseMobs, defenseElites;
    public GameObject[] shopItems;
    public GameObject Player, levelExit, Artifact, Companion, specialShop;

    public float waitToLoad = 5f;

    public string nextLevel;

    public string[] raidLevels;

    public string[] bonusLevels;

    [Header("Data/Collectibles")]
    public int currentCoins;
    public int currentGems;
    public int currentParts;
    public int currentKeys;
    public int currentBuys;
    public int currentRunes;
    public int tutorialCode;
    public int storyCode;
    public string savedLevel;

    public Transform startPoint;
    public Transform[] randomStartPoint;
    public Transform[] randomEndPoint;
    public Transform[] randomArtPoint, randomShopPoint, randomCompanionPoint;

    public Transform[] spawnPoints;
    

    //public Camera mainCamera;
    public bool isTitle, isBoss, isRaid, isRounds, isDefense, isCamp, isWarZone, isWideBoss, isTallBoss, isChangeLight, isLastLvl;
    [HideInInspector]
    public bool normal, hard, chaos;

    public float spawnTime;
    private float spawnTimeSeconds, Seconds;

    [HideInInspector]
    public int comboCount;
    public int expBonus;

    [Header("Messages")]
    public GameObject unlockMsg;

    [Header("Pathfinder")]
    public GameObject Pathfinder;
    public bool dontScan;

    [Header("Cutscenes")]
    public bool isCutscene;

    private int rounds;
    public List<GameObject> enemies;
    private bool roundGoing;
    public int totalRounds, totalExp;
    public bool cantSaveLevel;

    [Header("Canvas")]
    public CanvasScaler canvasScaler;

    [HideInInspector]
    public double playTime;

    [Header("AdManagers")]
    public GameObject adManager;

    // Start is called before the first frame update
  /*  private void Awake()
    {
      
        //Screen.SetResolution(1920, 1080, true);
        //Screen.SetResolution(Screen.width, Screen.height, true);
        Application.targetFrameRate = 61;

    }*/

    void Start()
    {
       
        storyCode = CharTracker.instance.storyCode;

        Physics2D.IgnoreLayerCollision(8, 7, false);

        if (!dontScan)
        {
            Invoke("CreatePathfind", 0.01f);
        }

        if (dontScan)
        {
            Pathfinder.SetActive(false);
        }
        

        if (SceneManager.GetActiveScene().name != "Title Scene")
        {
            if (SceneManager.GetActiveScene().name == "0")
            {
               
                currentCoins = 0;
                CharTracker.instance.currentCoins = 0;
                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                UIController.instance.ultButton.GetComponent<Button>().enabled = false;
                Physics2D.IgnoreLayerCollision(8, 15, false);

            }
            else if (isCamp)
            {
                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
                UIController.instance.ultButton.GetComponent<Button>().enabled = false;
                
            }
            else
            {
                UIController.instance.grenadeButton.GetComponent<Button>().enabled = true;
                UIController.instance.ultButton.GetComponent<Button>().enabled = true;
    
            }

          
        }

        if (PlayerController.instance != null)
        {
            if (isWideBoss)
            {
                PlayerController.instance.mainCam.orthographicSize = 10f;

            }
            if (isBoss)
            {
                PlayerController.instance.mainCam.orthographicSize = 9f;
               
            }
            else if (!isBoss && !isWideBoss && !isTallBoss)
            {
                PlayerController.instance.mainCam.orthographicSize = 7.5f;
                PlayerController.instance.mainCam.GetComponent<CameraController>().resetCam();
            }

            if (randomStartPoint.Length != 0)
            {
                int randomNum = Random.Range(0, randomStartPoint.Length);

                PlayerController.instance.transform.position = randomStartPoint[randomNum].position;

                if (CompanionController.instance != null)
                {
                    GameObject[] companions = GameObject.FindGameObjectsWithTag("Companion");

                    foreach (var a in companions)
                    {
                        if (!a.GetComponent<CompanionController>().isUnit)
                        {
                            a.transform.position = randomStartPoint[randomNum].position;
                        }
                    }

                }
            }
            else
            {
                PlayerController.instance.transform.position = startPoint.position;

                if (CompanionController.instance != null)
                {
                   
                    GameObject[] companions = GameObject.FindGameObjectsWithTag("Companion");

                    foreach (var a in companions)
                    {
                        if (!a.GetComponent<CompanionController>().isUnit)
                        {
                            a.transform.position = startPoint.position;
                        }
                    }

                }
            }

            if(CompanionController.instance != null && isDefense)
            {
                Physics2D.IgnoreLayerCollision(19, 8, false);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(19, 8);
            }

            PlayerController.instance.canMove = true;
        }

        if (randomEndPoint.Length != 0)
        {
            int randomNum = Random.Range(0, randomEndPoint.Length);

            if (Vector3.Distance(randomEndPoint[randomNum].transform.position, PlayerController.instance.transform.position) < 3)
            {
                randomNum += 1;

                if (randomNum >= randomEndPoint.Length)
                {
                    randomNum = 0;
                    Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
                }
                else
                {
                    Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
                }
            }
            else
            {
                Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
            }
        }

        if (randomArtPoint.Length != 0)
        {
            int randomNum = Random.Range(0, randomArtPoint.Length);

            Instantiate(Artifact, randomArtPoint[randomNum].transform.position, randomArtPoint[randomNum].transform.rotation);

        }
        if (randomCompanionPoint.Length != 0)
        {
            int randomNum = Random.Range(0, randomCompanionPoint.Length);

            Instantiate(Companion, randomCompanionPoint[randomNum].transform.position, randomCompanionPoint[randomNum].transform.rotation);

        }
        if (randomShopPoint.Length != 0)
        {
            int randomNum = Random.Range(0, randomShopPoint.Length);

            Instantiate(specialShop, randomShopPoint[randomNum].transform.position, randomShopPoint[randomNum].transform.rotation);

        }


        currentCoins = CharTracker.instance.currentCoins;
        currentGems = CharTracker.instance.currentGems;
        currentParts = CharTracker.instance.currentParts;
        currentBuys = CharTracker.instance.currentBuys;
        currentRunes = CharTracker.instance.currentRunes;


        instance = this;

        if (UIController.instance != null)
        {
            UIController.instance.coins.text = "x" + currentCoins.ToString();
            UIController.instance.gems.text = "x" + currentGems.ToString();
            UIController.instance.parts.text = "x" + currentParts.ToString();
            UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
            //UIController.instance.ammo2.text = "x" + PlayerController.instance.ammo2.ToString();
            UIController.instance.bomb1.text = "x" + PlayerController.instance.bomb1.ToString();

            if (isCamp && !isCutscene)
            {
                UIController.instance.runes.text = "x" + currentRunes.ToString();
            }
        }

        if (isRaid || isDefense)
        {
            UIController.instance.comboText.gameObject.SetActive(true);
            UIController.instance.comboNumber.gameObject.SetActive(true);
        }
        else if (!isRaid && !isCamp && !isTitle)
        {
            comboCount = 0;
            UIController.instance.comboNumber.text = comboCount.ToString();
            UIController.instance.comboText.gameObject.SetActive(false);
            UIController.instance.comboNumber.gameObject.SetActive(false);
            UIController.instance.comboEXP.gameObject.SetActive(false);
        }

        //reset temp buff
        if (PlayerController.instance != null)
        {
            PlayerController.instance.isDual = false;
            PlayerController.instance.buffDuration = 30f;
        }

        if (!cantSaveLevel)
        {
            if (SceneManager.GetActiveScene().name != "0")
            {
                if (!PlayerController.instance.dontSaveLvl)
                {
                    playTime = PlayerController.instance.playTime;
                    savedLevel = SceneManager.GetActiveScene().name;
                    //Invoke("SaveLevel", 1f);
                }

            }
        }

    }
    // Update is called once per frame
    void Update()
    {

        if (Seconds > 0)
        {
            Seconds -= Time.deltaTime;
        }
        //Raid Level Spawns
        if (isRaid)
        {
            if (PlayerHealth.instance.currentHealth > 0)
            {
                if (spawnTimeSeconds > 0)
                {
                    spawnTimeSeconds -= Time.deltaTime;
                }
                else if (TimerController.instance.timeValue > 0)
                {

                    StartCoroutine(SpawnMob());
                    spawnTimeSeconds = spawnTime;

                    if (TimerController.instance.timeValue <= 30f && TimerController.instance.timeValue > 29f)
                    {
                        StartCoroutine(SpawnElite());

                    }
                    else
                    {
                        StopCoroutine(SpawnElite());
                    }

                }
                else if (TimerController.instance.timeValue <= 1)
                {
                    StopCoroutine(SpawnMob());
                }
            }

        }
        if (isDefense)
        {
            if (PlayerHealth.instance.currentHealth > 0)
            {
                if (spawnTimeSeconds > 0)
                {
                    spawnTimeSeconds -= Time.deltaTime;
                }
                else if (TimerController.instance.timeValue > 0)
                {
                    StartCoroutine(SpawnMob());
                    spawnTimeSeconds = spawnTime;

                }
                else if (TimerController.instance.timeValue <= 1)
                {
                    StopCoroutine(SpawnMob());
                }
            }
        }
        if (isRounds)
        {
            if (!UIController.instance.fadeScreen.gameObject.activeInHierarchy)
            {
                if (spawnTimeSeconds > 0)
                {
                    spawnTimeSeconds -= Time.deltaTime;
                    roundGoing = false;
                }
                else
                {
                    if (roundGoing == false)
                    {
                        int random = Random.Range(0, raidMobs.Length);
                        int random2 = Random.Range(0, raidElites.Length);

                        if ((totalRounds - 1) == rounds)
                        {
                            Instantiate(raidElites[random2], spawnPoints[0].transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }
                        else
                        {
                            Instantiate(raidMobs[random], spawnPoints[0].transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }

                        StartCoroutine(AddEnemies());

                        roundGoing = true;

                    }
                    else if (roundGoing)
                    {

                        for (int i = enemies.Count - 1; i >= 0; i--)
                        {

                            if (enemies[i] == null)
                            {
                                enemies.RemoveAt(i);
                            }

                            if (enemies.Count == 0)
                            {
                                rounds += 1;

                                if (rounds == totalRounds)
                                {
                                    int exp;

                                    if (PlayerController.instance.expBuff)
                                    {
                                        exp = Mathf.RoundToInt(totalExp * 1.3f);
                                    }
                                    else
                                    {
                                        exp = totalExp;
                                    }

                                    UIController.instance.comboEXP.text = "EXP +" + exp.ToString();
                                    UIController.instance.comboEXP.gameObject.SetActive(true);
                                    ExpManager.instance.CollectExp(exp);

                                    levelExit.SetActive(true);
                                }
                                else
                                {
                                    spawnTimeSeconds = spawnTime;
                                }
                            }
                        }
                    }
                }
            }
        }

      /*  if(LevelGeneratorEndless.instance != null)
        {
            if (LevelGeneratorEndless.instance.spawnShop == true)
            {
                if (shopItems.Length == 0)
                {
                    shopItems = GameObject.FindGameObjectsWithTag("Shop Item");
                }
            }
        }*/

    }

    public IEnumerator LevelEnd()
    {

        if (SceneManager.GetActiveScene().name == "0")
        {
            //waitToLoad = 2f;
        }
        yield return new WaitForSeconds(waitToLoad);

        CharTracker.instance.currentCoins = currentCoins;
        CharTracker.instance.currentGems = currentGems;
        CharTracker.instance.currentHealth = PlayerHealth.instance.currentHealth;
        CharTracker.instance.maxHealth = PlayerHealth.instance.maxHealth;
     

        Scene scene = SceneManager.GetActiveScene();


        /*  int nextLevelAdd = int.Parse(scene.name) + 1;

          nextLevel = nextLevelAdd.ToString();*/

        if (raidLevels.Length <= 0)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else if (raidLevels.Length > 0)
        {
            int random = Random.Range(0, raidLevels.Length);

            SceneManager.LoadScene(raidLevels[random]);
        }

        if (TimerController.instance != null)
        {
            TimerController.instance.timerGoing = true;
        }

    }

    public IEnumerator LoadSavedLevel(string level)
    {

        if (SceneManager.GetActiveScene().name == "0")
        {
            //waitToLoad = 2f;
        }
        yield return new WaitForSeconds(waitToLoad);

        CharTracker.instance.currentCoins = currentCoins;
        CharTracker.instance.currentGems = currentGems;
        CharTracker.instance.currentHealth = PlayerHealth.instance.currentHealth;
        CharTracker.instance.maxHealth = PlayerHealth.instance.maxHealth;

        SceneManager.GetActiveScene();

        SceneManager.LoadScene(level);

    }

    public IEnumerator BonusLevel()
    {
        
        yield return new WaitForSeconds(waitToLoad);

        CharTracker.instance.currentCoins = currentCoins;
        CharTracker.instance.currentGems = currentGems;
        CharTracker.instance.currentHealth = PlayerHealth.instance.currentHealth;
        CharTracker.instance.maxHealth = PlayerHealth.instance.maxHealth;


        Scene scene = SceneManager.GetActiveScene();


        if (bonusLevels.Length <= 0)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else if (bonusLevels.Length > 0)
        {
            int random = Random.Range(0, bonusLevels.Length);

            SceneManager.LoadScene(bonusLevels[random]);
        }


        TimerController.instance.timerGoing = true;
    }

    public void GetRunes(int amount)
    {
        currentRunes += amount;
        //UIController.instance.runes.text = "x" + currentRunes.ToString();
        CharTracker.instance.SavePlayer();

    }

    public void GetGems(int amount)
    {
        currentGems += amount;
        UIController.instance.gems.text = "x" + currentGems.ToString();
        CharTracker.instance.SavePlayer();
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;
    }

    public void GetKeys(int amount)
    {
        currentKeys += amount;
    }

    public void GetBuys()
    {
        currentBuys += 1;

        if(currentBuys < 20) 
        {
            CharTracker.instance.SavePlayer();
        }
        else if (currentBuys == 20)
        {
            if (ContentManager.instance.unlockedChars[1] != 1)
            {
                ContentManager.instance.unlockedChars[1] = 2;
                unlockMsg.SetActive(true);

                CharTracker.instance.SavePlayer();
            }
            
        }
       
    }
    public void GetParts(int amount)
    {
        Seconds = 3f;
        currentParts += amount;

        if (!isCamp)
        {
            UIController.instance.partsUI.gameObject.SetActive(true);
            Invoke("HidePartsUI", 3f);
        }

        CharTracker.instance.SavePlayer();
    }


    public void SpendCoins(int amount)
    {
        currentCoins -= amount;
        if (currentCoins < 0)
        {
            currentCoins = 0;
        }
    }
    public void SpendGems(int amount)
    {
        currentGems -= amount;
        if (currentGems < 0)
        {
            currentGems = 0;
        }
    }


    public void restart()
    {
        savedLevel = "";
        CharTracker.instance.SaveLevel();
        CharTracker.instance.ClearTemp();

        Time.timeScale = 1f;

        SceneManager.LoadScene("0");
        //Load exp UI/scene
        Destroy(PlayerController.instance.gameObject);
    }

    public IEnumerator SpawnMob()
    {
        int randomNumb = Random.Range(0, 11);   

        int random1 = Random.Range(0, raidMobs.Length);
        int random2 = Random.Range(0, raidHordes.Length);

        int random3 = Random.Range(0, defenseMobs.Length);
        int random4 = Random.Range(0, defenseElites.Length);



        if (isRaid)
        {
            foreach (Transform t in spawnPoints)
            {
                if (randomNumb > 3)
                { 
                    Instantiate(raidMobs[random1], t.position, t.rotation);
                }
                else
                {
                    Instantiate(raidHordes[random2], t.position, t.rotation);
                }
            }
            

        }
        else if (isDefense)
        {
            //Debug.Log(TimerController.instance.timeDiff);
            Instantiate(defenseMobs[random3], spawnPoints[0].position, Quaternion.Euler(0f, 0f, 0f));


            if (TimerController.instance.timeDiff > 60)
            {

                Instantiate(defenseMobs[random3], spawnPoints[1].position, Quaternion.Euler(0f, 0f, 0f));

                if (TimerController.instance.timeDiff < 61)
                {
                    StartCoroutine(SpawnDefElite());
                }
            }
            if (TimerController.instance.timeDiff > 120)
            {

                Instantiate(defenseMobs[random3], spawnPoints[2].position, Quaternion.Euler(0f, 0f, 0f));

                if (TimerController.instance.timeDiff < 121)
                {
                    StartCoroutine(SpawnDefElite());
                }

            }
            if (TimerController.instance.timeDiff > 180)
            {

                Instantiate(defenseMobs[random3], spawnPoints[3].position, Quaternion.Euler(0f, 0f, 0f));

                if (TimerController.instance.timeDiff < 182)
                {
                    StartCoroutine(SpawnDefElite());
                }

            }
            /*   if (TimerController.instance.timeDiff > 4)
               {
                   int random = Random.Range(0, 11);

                   if (random > 7)
                   {
                       Instantiate(defenseElites[random4], spawnPoints[4].position, Quaternion.Euler(0f, 0f, 0f));
                   }
                   else
                   {
                       Instantiate(defenseMobs[random3], spawnPoints[4].position, Quaternion.Euler(0f, 0f, 0f));
                   }
               }*/

        }

        yield return null;
    }

    public IEnumerator SpawnElite()
    {
        int random = Random.Range(0, raidElites.Length);

        if (isRaid)
        {
            foreach (Transform t in spawnPoints)
            {
                Instantiate(raidElites[random], t.position, t.rotation);
            }
        }
 
        yield return null;
    }

    public IEnumerator SpawnDefElite()
    {
        yield return new WaitForSeconds(5f);

        int random = Random.Range(0, defenseElites.Length);

        foreach (Transform t in spawnPoints)
        {
            Instantiate(defenseElites[random], t.position, t.rotation);
        }


    }

    public void Bundle1Purchase()
    {
        instance.currentGems += 100;
        UIController.instance.gems.text = "x" + instance.currentGems.ToString();
        CharTracker.instance.SavePlayer();
    }
    public void Bundle2Purchase()
    {
        instance.currentGems += 500;
        UIController.instance.gems.text = "x" + instance.currentGems.ToString();
        CharTracker.instance.SavePlayer();
    }

    public void CreateExit()
    {
        int randomNum = Random.Range(0, randomEndPoint.Length);

        if (Vector3.Distance(randomEndPoint[randomNum].transform.position, PlayerController.instance.transform.position) < 3)
        {
            randomNum += 1;

            if (randomNum >= randomEndPoint.Length)
            {
                randomNum = 0;
                Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
            }
            else
            {
                Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
            }
        }
        else
        {
            Instantiate(levelExit, randomEndPoint[randomNum].transform.position, randomEndPoint[randomNum].transform.rotation);
        }
    }

    public void AddCombo(int exp)
    {
        UIController.instance.anim.SetTrigger("ComboUp");

        comboCount += exp;

        UIController.instance.comboNumber.text = comboCount.ToString();

        if(comboCount >= 100 && comboCount < 200)
        {
            UIController.instance.comboNumber.color = Color.yellow;
        }
        if(comboCount >= 200)
        {
            UIController.instance.comboNumber.color = Color.red;
        }
    }

    public void HidePartsUI()
    {
        if (Seconds <= 0)
        {
            UIController.instance.partsUI.gameObject.SetActive(false);
            
        }
    }

    public void CreatePathfind()
    {
        Pathfinder.GetComponent<AstarPath>().Scan();
    }

    public IEnumerator AddEnemies()
    {
        yield return new WaitForSeconds(3f);

        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject a in foundEnemies)
        {
            enemies.Add(a);
        }

        roundGoing = true;

    }

    public void SaveLevel()
    {
        CharTracker.instance.SaveLevel();
    }
}
