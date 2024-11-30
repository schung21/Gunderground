using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    public List<Button> skins;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var buttons in skins)
        {
            // buttons.onClick.AddListener(delegate { UIController.instance.confirmSkillWindow(buttons.GetComponent<SkillUnlock>().skillInfo, buttons.GetComponent<SkillUnlock>().Cost); });
            buttons.onClick.AddListener(delegate
            {
                UIController.instance.skinParam(buttons.GetComponent<SkillUnlock>().Lock, buttons.GetComponent<SkillUnlock>().Cost, buttons.GetComponent<SkillUnlock>().skillLvl);
            });

        }
    }

}
