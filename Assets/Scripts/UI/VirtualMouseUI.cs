using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{

    [SerializeField] private RectTransform canvasRectTranform;
    public GameObject cursor;

    private void Update()
    {
        transform.localScale = Vector3.one * 1f / canvasRectTranform.localScale.x;

        if(transform.localScale.x > 2f)
        {
            cursor.transform.localScale = transform.localScale / 4f;
        }
        else if (transform.localScale.x > 1f)
        {
            cursor.transform.localScale = transform.localScale / 2f;
        }
        else
        {
            cursor.transform.localScale = Vector3.one;
        }
       

    }


}
