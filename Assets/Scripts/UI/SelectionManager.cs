using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    public Animator anim;
    public GameObject[] lockedImg, unlockedImg;
    public CharacterSelector[] charSelector;
    public PlayerController[] selectedPlayer;
    public Button[] charButtons;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        UIController.instance.DisableButtons();

        var a = ContentManager.instance.unlockedChars;

        if(a[0] == 2)
        {
            lockedImg[0].SetActive(false);
            unlockedImg[0].SetActive(true);
        }
        else if (a[0] != 2)
        {
            charButtons[0].interactable = false;
        }

        if (a[1] == 2)
        {
            lockedImg[1].SetActive(false);
            unlockedImg[1].SetActive(true);
        }
        else if (a[1] != 2)
        {
            charButtons[1].interactable = false;
        }

        if (a[2] == 2)
        {
            lockedImg[2].SetActive(false);
            unlockedImg[2].SetActive(true);
        }
        else if (a[2] != 2)
        {
            charButtons[2].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        var a = ContentManager.instance.unlockedChars;

        if (a[0] == 2)
        {
            lockedImg[0].SetActive(false);
            unlockedImg[0].SetActive(true);
        }
        else if (a[0] != 2)
        {
            charButtons[0].interactable = false;
        }

        if (a[1] == 2)
        {
            lockedImg[1].SetActive(false);
            unlockedImg[1].SetActive(true);
        }
        else if (a[1] != 2)
        {
            charButtons[1].interactable = false;
        }

        if (a[2] == 2)
        {
            lockedImg[2].SetActive(false);
            unlockedImg[2].SetActive(true);
        }
        else if (a[2] != 2)
        {
            charButtons[2].interactable = false;
        }
    }

    public void EndScreen()
    {
     
        UIController.instance.EnableButtons();
        UIController.instance.DeActiveSelect();
        
    }

    public void BunnySelect()
    {
        anim.SetTrigger("Close");
        Instantiate(UIController.instance.buttonConfirm, transform.position, transform.rotation);
    }

    public void MoleSelect()
    {
        if (!lockedImg[0].activeInHierarchy)
        {

            anim.SetTrigger("Close");
            CharacterSelectManager.instance.change = true;
            CharacterSelector.instance.SelectFromMenu(selectedPlayer[0], charSelector[0]);

            Locker.instance.Message2.SetActive(false);
            Locker.instance.CheckSkills();
        }

        Instantiate(UIController.instance.buttonConfirm, transform.position, transform.rotation);
    }
    public void MercSelect()
    {
        if (!lockedImg[1].activeInHierarchy)
        {
            anim.SetTrigger("Close");
            CharacterSelectManager.instance.change = true;
            CharacterSelector.instance.SelectFromMenu(selectedPlayer[1], charSelector[1]);

            Locker.instance.Message2.SetActive(false);
            Locker.instance.CheckSkills();
        }

        Instantiate(UIController.instance.buttonConfirm, transform.position, transform.rotation);
    }

    public void CaptainSelect()
    {
        if (!lockedImg[2].activeInHierarchy)
        {
            anim.SetTrigger("Close");
            CharacterSelectManager.instance.change = true;
            CharacterSelector.instance.SelectFromMenu(selectedPlayer[2], charSelector[2]);

            Locker.instance.Message2.SetActive(false);
            Locker.instance.CheckSkills();
        }

        Instantiate(UIController.instance.buttonConfirm, transform.position, transform.rotation);
    }

}
