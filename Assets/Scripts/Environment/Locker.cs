using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public static Locker instance;
    public GameObject Message, Message2;
    public bool isLocker;
    public Sprite oldSprite, newSprite;
    public GameObject audio1, audio2;

    private bool CloseMsg2;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        CloseMsg2 = true;

        CheckSkills();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            Message.SetActive(true);

            isLocker = true;

            UIController.instance.interactButton.SetActive(true);

            GetComponent<SpriteRenderer>().sprite = newSprite;

            audio1.SetActive(true);
            audio2.SetActive(false);

            Message2.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            Message.SetActive(false);

            isLocker = false;

            UIController.instance.interactButton.SetActive(false);

            GetComponent<SpriteRenderer>().sprite = oldSprite;

            audio1.SetActive(false);
            audio2.SetActive(true);

            CheckSkills();
        }
    }

    public void CheckSkills()
    {
        if (PlayerController.instance.isBunny)
        {
            int level = ExpManager.instance.Levels[0];
            int skillLvl = 0;

            for (int i = 0; i < TraitManager.instance.Bunny.Count; i++)
            {
                skillLvl += 5;

                if (level >= skillLvl && TraitManager.instance.Bunny[i] == 0)
                {
                    Message2.SetActive(true);
                }

            }
        }
        if (PlayerController.instance.isMole)
        {
            int level = ExpManager.instance.Levels[1];
            int skillLvl = 0;

            for (int i = 0; i < TraitManager.instance.Mole.Count; i++)
            {
                skillLvl += 5;

                if (level >= skillLvl && TraitManager.instance.Mole[i] == 0)
                {
                    Message2.SetActive(true);
                }

            }
        }
        if (PlayerController.instance.isRaccoon)
        {
            int level = ExpManager.instance.Levels[2];
            int skillLvl = 0;

            for (int i = 0; i < TraitManager.instance.Raccoon.Count; i++)
            {
                skillLvl += 5;

                if (level >= skillLvl && TraitManager.instance.Raccoon[i] == 0)
                {
                    Message2.SetActive(true);
                }

            }
        }

    }
}
