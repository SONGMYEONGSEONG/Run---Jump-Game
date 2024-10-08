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
        // 버튼에서 손을 뗐을 때 호출됩니다.
        Debug.Log("Button released!");
        isTouch = false;
    }

}
