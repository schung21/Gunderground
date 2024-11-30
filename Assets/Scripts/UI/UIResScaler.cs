using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResScaler : MonoBehaviour
{

    [Header("Canvas Scaling")]
    CanvasScaler canvasScaler;


    private void Awake()
    {   
        canvasScaler = GetComponent<CanvasScaler>();
    }

    void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        //canvasScaler.referenceResolution *= new Vector2((canvasScaler.referenceResolution.x / Screen.width), (Screen.height / canvasScaler.referenceResolution.y));
    }


}
