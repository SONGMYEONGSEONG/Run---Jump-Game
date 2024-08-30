using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TouchLeftPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void TouchPadMoveHandler();
    public static event TouchPadMoveHandler OnTouchLeftPad;

    bool isTouch = false;

    private void Update()
    {
        if(isTouch)
        {
            OnTouchLeftPad.Invoke();
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ��ư���� ���� ���� �� ȣ��˴ϴ�.
        Debug.Log("Button released!");
        isTouch = false;
    }


}
