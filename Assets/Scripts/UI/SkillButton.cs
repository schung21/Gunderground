using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SkillButton : Button
{
    public static SkillButton instance;

    protected override void Awake()
    {
        instance = this;
    }
    protected override void Start()
    {
        onClick.AddListener(useSkill);
    }

    public void useSkill()
    {
        if (SceneManager.GetActiveScene().name != "0")
        {
            PlayerController.instance.useSkill = true;
        }
    }

}
