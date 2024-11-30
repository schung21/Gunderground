using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreWall : MonoBehaviour
{
    public static IgnoreWall instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }


}
