using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JumpPower : MonoBehaviour
{
    [SerializeField] RectTransform tr;
    [SerializeField] float charge_Speed = 5.0f;
    [SerializeField] float power_ratio = 30.0f;

    [SerializeField] Transform player_tr;

    public void JumpPowerPrint(Vector2 playerPos)
    {
        tr.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        if(charge_Speed < 0.0f) {  charge_Speed *= -1; }
        
        gameObject.SetActive(true);

    }

    public float GetJumpingPower()
    {
        if (tr.localScale.x <= 0.0f)
        {
            Debug.Log("레전드 상홍 발생!!" + tr.localScale.x);
        }

        return tr.localScale.x * power_ratio;
    }

    public void JumpPowerOff()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 playerPos = new Vector2(player_tr.position.x, player_tr.position.y + 0.75f);

        Vector2 pos = Camera.main.WorldToScreenPoint(playerPos);
        transform.position = new Vector2(pos.x, pos.y);

        if (tr.localScale.x > 1.0f)
        {
            tr.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            charge_Speed *= -1.0f;
        }
        else if (tr.localScale.x < 0.0f)
        {
            tr.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            charge_Speed *= -1.0f;
        }
           
        tr.localScale += new Vector3(charge_Speed, 0f, 0f) * Time.deltaTime;

    }
}
