using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TouchRightPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void TouchPadMoveHandler();
    public static event TouchPadMoveHandler OnTouchRightPad;

    bool isTouch = false;

    private void Update()
    {
        if (isTouch)
        {
            OnTouchRightPad.Invoke();
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
