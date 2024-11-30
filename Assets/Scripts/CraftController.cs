using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftController : MonoBehaviour
{

    public static CraftController instance;
    public string gunName;
    public int cost;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate {UIController.instance.craftGun(cost, gunName);});
    }

 
}
