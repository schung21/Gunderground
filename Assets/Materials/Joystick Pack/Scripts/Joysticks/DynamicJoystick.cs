using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{

   
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        //background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(Vector3.Distance(ScreenPointToAnchoredPosition(eventData.position), background.anchoredPosition) > 120)
        {
            //Debug.Log(Vector3.Distance(ScreenPointToAnchoredPosition(eventData.position), background.anchoredPosition));
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        }
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);

        if (HandleRange == 0.5f)
        {
            base.OnPointerDown(eventData);
            FireButton.instance.pressed = true;
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        //FireButton.instance.pressed = false;
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}