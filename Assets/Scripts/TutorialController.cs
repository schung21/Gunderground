using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Animator anim;
    public static TutorialController instance;
    public List<GameObject> enemies;
    public GameObject levelExit, firstText, lastText;
    public bool endPhase1, startPhase2, cantMove;


 
    // Start is called before the first frame update
    void Start()
    {
        //cantMove = true;
        instance = this;
        //Invoke("PlayerMove", 48f);
    

    }

    // Update is called once per frame
    void Update()
    {
        UIController.instance.stickType = 1;

        if (cantMove == true)
        {
            PlayerController.instance.canMove = false;
            SkillButton.instance.enabled = false;
            UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
            SkillButton.instance.enabled = true;
            UIController.instance.grenadeButton.GetComponent<Button>().enabled = true;
            
        }

        for(int i = enemies.Count - 1; i >= 0; i--)
        {
           
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }

            if(enemies.Count == 8)
            {
                if (!startPhase2)
                {
                    endPhase1 = true;
                }
            }

            if (enemies.Count == 0)
            {
                EndLesson();
            }
        }
   
        if (endPhase1 == true)
        {
            anim.SetTrigger("Next");
            Invoke("ActiveUlt", 6f);
            endPhase1 = false;
            startPhase2 = true;
        }

    }

    public void OpenExit()
    {

        levelExit.SetActive(true);
    }

    public void PlayerMove()
    {
        cantMove = false;
       
    }

    public void EndLesson()
    {
        anim.enabled = false;
        firstText.SetActive(false);
        Invoke("OpenExit", 2f);
        LevelManager.instance.tutorialCode = 1;
        lastText.SetActive(true);

    }

    public void ActiveUlt()
    {
      
        UIController.instance.ultButton.GetComponent<Button>().interactable = true;
        UIController.instance.ultButtonGlow.SetActive(true);
    }


  
/*
    public void DestroyEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject a in enemies)
        {
            a.GetComponent<EnemyController>().DamageEnemy(99999);
        }
    }*/

}
