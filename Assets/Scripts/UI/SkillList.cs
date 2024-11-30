using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillList : MonoBehaviour
{

    public List<Button> Skills;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var buttons in Skills)
        {
           // buttons.onClick.AddListener(delegate { UIController.instance.confirmSkillWindow(buttons.GetComponent<SkillUnlock>().skillInfo, buttons.GetComponent<SkillUnlock>().Cost); });
            buttons.onClick.AddListener(delegate { UIController.instance.skillParam(buttons.GetComponent<SkillUnlock>().Lock, buttons.GetComponent<SkillUnlock>().skillNumber,
                  buttons.GetComponent<SkillUnlock>().Cost, buttons.GetComponent<SkillUnlock>().skillLvl, buttons.GetComponent<SkillUnlock>().skillInfo); });
        
        }
    }


}
