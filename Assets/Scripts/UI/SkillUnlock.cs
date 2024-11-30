using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlock : MonoBehaviour
{
    public static SkillUnlock instance;
    public int skillNumber, skillLvl;
    public int Cost;
    public string skillInfo;
    public Text text;
    public GameObject Lock;

    private void Start()
    {
        if (text != null)
        {
            skillInfo = text.text;
        }
    }
    private void Update()
    {
        if (text != null)
        {
            skillInfo = text.text;
        }
        
    }

}
