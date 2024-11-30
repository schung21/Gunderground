using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    //public string levelToLoad;
    public static LevelExit instance;
    public bool isBonus, isCenter, isInvinc;
    //public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        instance = this;
        Invoke("Activate", 2f);
        isInvinc = false;
    }

    // Update is called once per frame
  /*  void Update()
    {
       
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            isInvinc = true;

            if(SceneManager.GetActiveScene().name == "Tutorial")
            {
                TutorialController.instance.cantMove = true;
                Invoke("DestroyPlayer", 1f);
            }
            else if(LevelManager.instance.nextLevel == "0" && SceneManager.GetActiveScene().name != "Tutorial")
            {
                Invoke("DestroyPlayer", 1f);
            }
            if (PlayerController.instance.isMole && PlayerController.instance.dashCounter > 0)
            {
                PlayerController.instance.anim.Play("End Dash", -1, 0f);
                PlayerController.instance.dashCounter = 0;
                PlayerController.instance.ResetMole();
            }

            Invoke("LevelSave", 1f);
            
            UIController.instance.startFadeToBlack();
            TimerController.instance.timerGoing = false;
            PlayerController.instance.canMove = false;
            
            if (EnemyController.instance != null)
            {
                EnemyController.instance.canMove = false;
            }

            Physics2D.IgnoreLayerCollision(6, 9);
            Physics2D.IgnoreLayerCollision(6, 8);

            GameObject[] bullet = GameObject.FindGameObjectsWithTag("Bullets");
            Physics2D.IgnoreLayerCollision(6, 10);

            foreach (GameObject bullets in bullet)
            {
                Destroy(bullets);
            }

            if (!isBonus)
            {
                StartCoroutine(LevelManager.instance.LevelEnd());
            }
            else
            { 
                StartCoroutine(LevelManager.instance.BonusLevel());
            }
        
        }

    }

    public void LevelSave()
    {
        LevelManager.instance.SaveLevel();
        CharTracker.instance.SavePlayer();
    }
    
    public void Activate()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void DestroyPlayer()
    {
        Destroy(PlayerController.instance.gameObject);
    }
   
}
