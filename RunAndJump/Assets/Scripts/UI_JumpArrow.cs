using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JumpArrow: MonoBehaviour
{
    [SerializeField] RectTransform tr;
    [SerializeField] float rotate_speed = 60.0f;

    public void JumpArrowPrint(Vector2 playerPos)
    {
        gameObject.SetActive(true); 
    }

    public void JumpArrowOff()
    {
        gameObject.SetActive(false);
    }

    public Vector2 GetJumpingDir()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return pos;
    }

    private void Update()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = Camera.main.WorldToScreenPoint(mousepos);
        tr.position = new Vector2(pos.x, pos.y);

    }
}

