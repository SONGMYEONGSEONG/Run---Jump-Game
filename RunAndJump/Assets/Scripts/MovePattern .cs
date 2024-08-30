using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePattern : MonoBehaviour
{
    [SerializeField] float avoidSpeed = 2.0f;

    int dir = 1;
    private Vector3 originPos;
    private float moveRange = 1.0f;

    private void Start()
    {
        originPos = transform.localPosition;
    }

    private void Update()
    {
        if(Mathf.Abs(transform.localPosition.x - originPos.x) >= moveRange) dir *= -1;
         
        transform.localPosition += dir * ((Vector3.right  * avoidSpeed)) * Time.deltaTime;
    }
}
